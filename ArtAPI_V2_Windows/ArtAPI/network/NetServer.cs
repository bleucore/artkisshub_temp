using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnyCCU;
using ArtAPI.info;
using ArtAPI.network;
using ArtAPI.network.payload;
using ArtAPI.utils;
using ArtAPI.network;

// https://docs.microsoft.com/ko-kr/dotnet/standard/collections/thread-safe/blockingcollection-overview
namespace ArtAPI.library
{
	public	class	NetServer : CBLogNetwork
	{
		public	static	bool	mAliveMode		= false;
		public	static	int		mAliveTime;
		public	static	int		mReadTimeout	= 0;

		private	Mutex			mtx			= new Mutex(false, "NetServer");

		private TcpListener		_server;
		private Boolean			_isRunning;

		private int				mTransSeq		= 0;		// client에서 trans할때 마다 하나씩 증가.

		private int				mMainPort;


		// connection들 모음.... 
		private	List<ConnInfo>	mHandles	= new List<ConnInfo>();

		// Thread로 쓸때없는 conn은 제거를 한다..... 

		public	ConnInfo	FindConnInfo(string device_id) {
			mtx.WaitOne();
			foreach(ConnInfo info in mHandles) {
				if (info.mDeviceID == device_id) {
					mtx.ReleaseMutex();
					return	info;
				}
			}
			mtx.ReleaseMutex();
			return	null;
		}

		public int LogTrans(char level, string tag, string time, string msg)
		{
			//throw new NotImplementedException();
			LogInfo		info	= new LogInfo(level, tag, time, msg);
			Protocol	log		= new Protocol(Protocol.OP_EVT);
			PayloadLog	payload	= new PayloadLog();
			payload.SetPayload(log, info);

			return	BroadCast(log);
		}

		public	int		BroadCast(Protocol req) {

			foreach (ConnInfo info in mHandles) {
				if (info != null) {
					try { 
						// Send data
						Protocol	res	= new Protocol(req);

						byte[] hData = req.Build();
						if (hData.Length > 130)		Console.WriteLine(HexDump.Dump(hData, 130));
						else						Console.WriteLine(HexDump.Dump(hData));
						
						info.mEventWriter.BaseStream.Write(hData, 0, hData.Length);
						//sWriter.BaseStream.Write(sData, 0, 22);
						info.mEventWriter.Flush();

						// 응답을 받을지 결정을 하자.....
						Byte[]	nLen	= new Byte[4];

						// reads from stream
						//Console.WriteLine("before read : " + HexDump.Dump(nLen, nLen.Length));
						int retv = nRead(info.mEventHandle, info.mEventReader, ref nLen);
						Console.WriteLine("rData : read bytes is {0}", retv);

						if (retv > 0)
						{
							Console.WriteLine(HexDump.Dump(nLen, nLen.Length));

							int len = BitConverter.ToInt32(nLen, 0);
							//Console.WriteLine("rData : network Len is {0}", len);

							len = IPAddress.NetworkToHostOrder(len);
							Console.WriteLine("rData : host order  is {0}", len);

							if (len < 0 || len > 200000048) continue;

							Byte[] rData = new Byte[len];
							retv = nRead(info.mEventHandle, info.mEventReader, ref rData);
							Console.WriteLine(HexDump.Dump(rData, rData.Length));

							//string rStr = Encoding.UTF8.GetString(rData);
							//string rStr = Encoding.Unicode.GetString(rData);
							//Console.WriteLine("rStr ->" + rStr);
							if (res.Parse(rData)) {
								// 전달 성공...
								if (res.IsOK()) {
									//Console.WriteLine("rStr ->" + rStr);
									return	1;
								}
								return	-5;
							}
						} else  {
							// 응답이 없을경우 처리....
							// 동일한것을 다시 보낸다....
							return	-2;
						}
					} catch(Exception e) {
						return	-3;
					}
				}
				return	-4;
			}
			return -1;
		}


		public	int		nRead(TcpClient client, StreamReader reader, ref byte[] bytes) {
			int		retv	= 0;
			int		nRead	= 0;
			while (nRead < bytes.Length)
			{
				try {
					retv = reader.BaseStream.Read(bytes, nRead, bytes.Length - nRead);
					if (retv > 0) {
						nRead += retv;
					} else {
						if (!IsSocketConnected(client)) {
							return	-1;
						}
					}
				} catch(Exception e) {
					if (!IsSocketConnected(client)) {
						return	-2;
					}
				}
			}
			return	nRead;
		}

		public	void	HandleClient(object obj)
		{
			// retrieve client from parameter passed to thread
			TcpClient	client		= (TcpClient)obj;

			// sets two streams
			StreamWriter sWriter	= new StreamWriter(client.GetStream(), new UTF8Encoding(false));
			StreamReader sReader	= new StreamReader(client.GetStream(), new UTF8Encoding(false));
			//StreamWriter sWriter	= new StreamWriter(client.GetStream(), new UnicodeEncoding());
			//StreamReader sReader	= new StreamReader(client.GetStream(), new UnicodeEncoding());

			if (mReadTimeout != 0)
				sReader.BaseStream.ReadTimeout = mReadTimeout;

			// you could use the NetworkStream to read and write, 
			// but there is no forcing flush, even when requested
			Boolean		bIsConnected	= true;

			HandShake	handShake	= new HandShake();

			Protocol	rMsg		= new Protocol("");
			Protocol	sMsg		= new Protocol("");

			byte[]		nLen		= new byte[4];
			
			ConnInfo	mConnInfo	= null;

			// HandShake 과정을 진행하고 Key를 받는다....

			while (bIsConnected) {
				
				// reads from stream
				int retv	= nRead(client, sReader, ref nLen);
				Console.WriteLine("rData : read bytes is {0}", retv);
				if (mConnInfo != null)			mConnInfo.Touch();
				if (retv > 0) {
					Console.WriteLine(HexDump.Dump(nLen, nLen.Length));

					int		len = BitConverter.ToInt32(nLen, 0);
					//Console.WriteLine("rData : network Len is {0}", len);
					
					len = IPAddress.NetworkToHostOrder(len);
					Console.WriteLine("rData : host order  is {0}", len);
					
					if (len < 0 || len > 200000048)		continue;

					byte[]	rData	= new byte[len];
					retv	= nRead(client, sReader, ref rData);
					Console.WriteLine(HexDump.Dump(rData, rData.Length));

					//string rStr	= Encoding.UTF8.GetString(rData);
					//string rStr	= Encoding.Unicode.GetString(rData);
					//Console.WriteLine("rStr ->" + rStr);

					if (rMsg.Parse(rData)) {
						Console.WriteLine(rMsg.ToString());
						if (!handShake.mIsHandShake) { 
							if (handShake.IsMainHandle(rMsg)) {
								string device_id		= rMsg.GetValuePayload("device_id").ToString();

								// 1. 지금 연결을 Control Handle로 하고
								// 2. Crypto 키를 발급하고 전송한다....
								mConnInfo				= new ConnInfo();
								mConnInfo.mCryptoKey	= Guid.NewGuid().ToString();
								mConnInfo.mDeviceID		= device_id;

								sMsg.AddPayload("crypto_key", mConnInfo.mCryptoKey);
								handShake.SetResCtrl(sMsg);

								// Send data
								byte[]	hData			= sMsg.Build();
								Console.WriteLine(HexDump.Dump(hData, hData.Length));

								try {
									sWriter.BaseStream.Write(hData, 0, hData.Length);
									//sWriter.BaseStream.Write(sData, 0, 22);
									sWriter.Flush();

									mConnInfo.mCtrlHandle	= client;
									mConnInfo.mCtrlReader	= sReader;
									mConnInfo.mCtrlWriter	= sWriter;

									mtx.WaitOne();
									mHandles.Add(mConnInfo);
									mtx.ReleaseMutex();

									handShake.mIsHandShake = true;
								} catch(Exception e) {
									mConnInfo.mCtrlHandle	= null;
									Console.WriteLine(e.Message);
								}
								continue;
							}

							if (handShake.IsEventHandle(rMsg)) {
								string	device_id	= rMsg.GetValuePayload("device_id").ToString();
								// event이면
								mConnInfo		= FindConnInfo(device_id);
								if (mConnInfo == null) {
									// Ctrl handle이 없어요. 다시 Handshake하세요....
									sMsg.ClearPayload();
									sMsg.SetError("1002", "Not Ctrl handle");
								} else {
									mConnInfo.mEventHandle	= client;
									mConnInfo.mEventReader	= sReader;
									mConnInfo.mEventWriter	= sWriter;

									handShake.mIsHandShake	= true;

									sMsg.ClearPayload();
									sMsg.SetOK();
								}
								
								try {
									// Send data
									byte[] hData = sMsg.Build();
									Console.WriteLine(HexDump.Dump(hData, hData.Length));
									sWriter.BaseStream.Write(hData, 0, hData.Length);
									//sWriter.BaseStream.Write(sData, 0, 22);
									sWriter.Flush();

									// 여기서 끝이어야 한다...
									// Event recv가 따로 ....
									Console.WriteLine("NetServer : Event recv가 따로 ....");
									bIsConnected	= false;
								} catch(Exception e) {
									mConnInfo.mCtrlHandle	= null;
									Console.WriteLine(e.Message);
								}
								break;
								//continue;
							}
						} else {
							//if (!rMsg.IsOK()) {
							//	LogUtil.LogD("LOG", rMsg.ToString());
							//	continue;
							//}
							if (Global.mGateWay != null) {
								sMsg	= Global.mGateWay.Proccess(rMsg);
							}
						}
					} else {
						sMsg.SetError("E01", "message parse error");
					}

					try {
						// Send data
						byte[]	sData	= sMsg.Build();
						if(sData.Length > 300) Console.WriteLine(HexDump.Dump(sData, 300));
						else Console.WriteLine(HexDump.Dump(sData, sData.Length));
						sWriter.BaseStream.Write(sData, 0, sData.Length);
						//sWriter.BaseStream.Write(sData, 0, 22);
						sWriter.Flush();
					} catch(Exception e) {
						mConnInfo.mCtrlHandle	= null;
						Console.WriteLine(e.Message);
					}

					System.Array.Clear(rData, 0, rData.Length);
				} else {
					// Socket이 끊어 졌는지... 확인을 한다.....
					if (!IsSocketConnected(client)) {
						Console.WriteLine("NetServer : TcpClient Socket이 끊어짐");
						bIsConnected	= false;

						// 해당하는 채널을 전체 Close;
						mtx.WaitOne();
						if (mConnInfo != null)	mConnInfo.Close();
						mtx.ReleaseMutex();
					}
				}
			}
		}

		public	NetServer(int port)
		{
			_server		= new TcpListener(IPAddress.Any, port);
			_server.Start();

			_isRunning = true;


			Console.WriteLine("Server ---------------> start[" + port + "]");

			Thread t	= new Thread(new ParameterizedThreadStart(LoopClients));
			t.Start();

			Thread m	= new Thread(new ParameterizedThreadStart(ConnMonitor));
			m.Start(this);

			// Log를 전송한다.
			LogUtil.mTrans	= this;
		}

		public	void	LoopClients(object obj)
		{
			while (_isRunning)
			{
				// wait for client connection
				TcpClient newClient = _server.AcceptTcpClient();

				// client found.
				// create a thread to handle communication
				Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
				t.Start(newClient);
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		static	bool	IsSocketConnected(TcpClient client)
		{
			IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
			try {
				TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections().Where(x => x.LocalEndPoint.Equals(client.Client.LocalEndPoint) && x.RemoteEndPoint.Equals(client.Client.RemoteEndPoint)).ToArray();

				if (tcpConnections != null && tcpConnections.Length > 0) {
					TcpState stateOfConnection = tcpConnections.First().State;
					if (stateOfConnection == TcpState.Established) {
						return true;            // Connection is OK
					}
				}
			} catch(Exception e) {
				Console.WriteLine(e.Message);
			}
			return false;
		}

		public	void	ConnMonitor(object obj) {
			NetServer	netServer	= (NetServer)obj;

			bool	isRemove	= false;

			while(true) {

				for(int idx = 0; idx < netServer.mHandles.Count; idx++) {
					ConnInfo	info	= netServer.mHandles[idx];
					try {
						isRemove	= false;

						if (mAliveMode && info.ValidAlive(DateTime.Now, mAliveTime)) {
							isRemove	= true;
						}

						if (info.mCtrlHandle == null) {
							isRemove	= true;
						}

						if (isRemove) {
							// Close시키고 삭제한다.
							netServer.mtx.WaitOne();
							info.Close();
							netServer.mHandles.RemoveAt(idx);
							netServer.mtx.ReleaseMutex();
						}
					} catch(Exception e) {}
				}
				Thread.Sleep(1000);
			}
		}

		public	static	string	GetLocalIP()
		{
			string localIP = "Not available, please check your network seetings!";
			IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (IPAddress ip in host.AddressList) {
				if (ip.AddressFamily == AddressFamily.InterNetwork) {
					localIP = ip.ToString();
					break;
				}
			}
			return localIP;
		}

		public	void	TestEndian()
		{
			short val = 1234;
			Console.WriteLine("Val is {0}", val);

			val = IPAddress.HostToNetworkOrder(val);

			Console.WriteLine("Val is {0}", val);
			Console.WriteLine("Little endian? " + BitConverter.IsLittleEndian);

			byte[] bytes = BitConverter.GetBytes(val);
			foreach (byte b in bytes)
			{
				Console.WriteLine(b);
			}

			short len = BitConverter.ToInt16(bytes, 0);
			Console.WriteLine("Len is {0}", len);
			len = IPAddress.NetworkToHostOrder(len);

			Console.WriteLine("Len is {0}", len);
		}

		[DllImport("kernel32", CharSet = CharSet.Auto)]
		public static extern Int32 GetLastError();

	}
}
