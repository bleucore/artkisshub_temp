using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using ArtAPI.process;
using ArtAPI.utils;
using ArtAPI.network;

namespace ArtAPI
{
	// ----------------------------------------------------------
	//               for single instance
	// ----------------------------------------------------------
	public	sealed	class	GateWay {

		private BlockingCollection<Protocol> dataItems = new BlockingCollection<Protocol>(100);

		public	ProcAlive		mProcAlive		= new ProcAlive();
		// 이벤트 처리
		public	ProcState		mProcState		= new ProcState();
		public	ProcEvent		mProcEvent		= new ProcEvent();


		private static	readonly GateWay instance = new GateWay();
		public	static	GateWay		Instance {
			get {
				return instance;
			}
		}

		private GateWay()
		{
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public	Protocol Proccess(Protocol req) 
		{
			Protocol	response	= null;
			string		cmd			= "None";
			try { 
				cmd		= req.GetValuePayload("cmd").ToString();
				switch(cmd) {
					case "alive":
						response	= mProcAlive.Exec(req);
						break;
					case "handshake":
						break;
					case "control":
						break;
					case "stream":
						break;
					case "state":
						response	= mProcState.Exec(req);
						break;
					case "event":
						response	= mProcEvent.Exec(req);
						break;
				}
			} catch(Exception e) {
			}

			if (response == null) {
				response	= new Protocol(Protocol.OP_RES);
				response.SetError("0011", "cmd error["+cmd+"]");
			}
			return	response;
		}

		public	int		Init() {		// 1. 내부 처리를 위하여

			// 1. DB를 읽어와서 .....

			// 2. 각 Adapter 세팅....
			LogUtil.LogD("GateWay", "Motion SocketClient thread start");

			// 3. Protocol buffer
            // --------------------------------------------------------------------
            //
            // --------------------------------------------------------------------
            Thread motionThr = new Thread(new ThreadStart(delegate() // thread 생성
			{
				// ThreadStart 델리게이트를 통해 해당 Thread 가 실행할 작업내용을 선언
				// ThreadStart는 Java의 Runnable 인터페이스와 비슷한 개념이다.

				LogUtil.LogD("GateWay", "Motion SocketClient thread start");

				string cmd = "test1";
				byte[] receiverBuff = new byte[8192];
 
				LogUtil.LogD("GateWay","Motion server : Connected... Enter Q to exit");
 
				// Q 를 누를 때까지 계속 Echo 실행
				//while ((cmd = Console.ReadLine()) != "Q")
				while(!dataItems.IsCompleted) {
					Protocol data = null;
					// Blocks if dataItems.Count == 0.
					// IOE means that Take() was called on a completed collection.
					// Some other thread can call CompleteAdding after we pass the
					// IsCompleted check but before we call Take. 
					// In this example, we can simply catch the exception since the 
					// loop will break on the next iteration.
					try {
						data = dataItems.Take();
						LogUtil.LogD("GateWay", "Gesture data : " + data);
					}
					catch (InvalidOperationException) { }
 
					LogUtil.LogD("GateWay", "Gesture data : " + data);
					/*
					this.Invoke(new Action(delegate() {	// Form 상속
                        if ( ret < 0 ) {
                            Console.WriteLine(string.Format("----------------> GestureProc : error {0} ", ret));
                        }
                    }));
					Thread.Sleep(1000);
					*/
				}

			}));
			motionThr.Start(); // 스레드 시작

			return	0;
		}

		public	void	put(Protocol protocol) {
			dataItems.Add(protocol);
            Console.WriteLine("Add:{0} Count={1}", protocol, dataItems.Count);
		}


	}
}
