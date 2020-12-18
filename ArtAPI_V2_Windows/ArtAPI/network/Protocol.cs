
using Newtonsoft.Json.Linq;
using System;

using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace ArtAPI.network
{
	public class PHead
	{
		public	const int HEAD_LEN = 9;

		public	byte[] op_code	= new byte[2];
		public	byte[] src_id	= new byte[1];
		public	byte[] dst_id	= new byte[1];
		public	byte[] payload	= new byte[2];
		public	byte[] reserved = new byte[3];

		public PHead() {
			reserved[0]	= 0x58;
			reserved[1] = 0x58;
			reserved[2] = 0x58;
		}

		public	bool	IsEqual(string op_code) {
			byte[] code = Encoding.UTF8.GetBytes(op_code);
			//byte[] code = Encoding.Unicode.GetBytes(op_code);
			if(code[0] == this.op_code[0] &&code[1] == this.op_code[1]) {
				return	true;
			}
			return	false;
		}

		public	void	Set(byte[] bytes, int index) {
			op_code[0]	= bytes[index + 0];
			op_code[1]	= bytes[index + 1];
			src_id[0]	= bytes[index + 2];
			dst_id[0]	= bytes[index + 3];

			payload[0]	= bytes[index + 4];
			payload[1]	= bytes[index + 5];

			reserved[0] = bytes[index + 6];
			reserved[1] = bytes[index + 7];
			reserved[2] = bytes[index + 8];
		}

		public	short	GetPayloadLen() 
		{
			short len = BitConverter.ToInt16(payload, 0);
			len = IPAddress.NetworkToHostOrder(len);
			return	len;
		}

		public	void	SetPayloadLen(short len) 
		{
			short length = IPAddress.HostToNetworkOrder(len);
			byte[] bytes = BitConverter.GetBytes(length);
			System.Array.Copy(bytes, payload, payload.Length);
		}

		public byte[] GetBytes()
		{
			byte[] retv = new byte[9];

			retv[0]		= op_code[0];
			retv[1]		= op_code[1];
			retv[2]		= src_id[0];
			retv[3]		= dst_id[0];
			retv[4]		= payload[0];
			retv[5]		= payload[1];
			retv[6]		= reserved[0];
			retv[7]		= reserved[1];
			retv[8]		= reserved[2];

			return retv;
		}

		public void Clone(PHead header)
		{
			op_code[0]	= header.op_code[0];
			op_code[1]	= header.op_code[1];
			src_id[0]	= header.src_id[0];
			dst_id[0]	= header.op_code[0];

			payload[0]	= header.payload[0];
			payload[1]	= header.payload[1];

			reserved[0] = header.reserved[0];
			reserved[1] = header.reserved[1];
			reserved[2] = header.reserved[2];
		}

		public	override	string	ToString() {
			return string.Format("op_code : {0}, dst_id : {1}", Encoding.Default.GetString(op_code), Encoding.Default.GetString(dst_id));
		}
	}

	public	class	Protocol
	{
		public const string OP_NON = "NN";
		public const string OP_RES = "RS";
		public const string OP_GET = "GE";     // GET value
		public const string OP_SET = "SE";     // SET value
		public const string OP_HSK = "HS";     // Hand shake
		public const string OP_PTZ = "PT";
		public const string OP_EVT = "EV";      // 이벤트
		public const string OP_IMG = "IM";      // 이미지

		PHead	header	= new PHead();
		JObject payload = new JObject();
		byte[]	image	= null;

		//byte[] len		= new byte[2];

		public	Protocol(Protocol protocol) {
			header.Clone(protocol.header);
		}

		public	Protocol(string op_code)
		{
			byte[] code	= null;
			if (op_code.Length < 2) code = Encoding.UTF8.GetBytes(OP_NON);
			else	code = Encoding.UTF8.GetBytes(op_code);

			//if (op_code.Length < 2) code = Encoding.Unicode.GetBytes(OP_NON);
			//else	code = Encoding.Unicode.GetBytes(op_code);
			header.op_code[0] = code[0];
			header.op_code[1] = code[1];
			//header.src_id[0] = Global.mSiteInfo.CurIP[3];
			//header.dst_id[0] = Global.mSiteInfo.CurIP[3];
		}

		public	Protocol(byte[] op_code)
		{
			header.op_code[0] = op_code[0];
			header.op_code[1] = op_code[1];
			//header.src_id[0] = Global.mSiteInfo.CurIP[3];
			//header.dst_id[0] = Global.mSiteInfo.CurIP[3];
		}

		public	byte[]	GetImage()
		{
			return	this.image;
		}

		public	void	SetImage(byte[] image)
		{
			this.image	= image;
		}

		public	void	AddPayload(string key, JArray val)
		{
			var value	= payload[key];
			if (value == null)	payload.Add(key, val);
			else				payload[key] = val;
		}

		public	void	AddPayload(string key, string val)
		{
			var value	= payload[key];
			if (value == null)	payload.Add(key, val);
			else				payload[key] = val;
		}

		public	JToken	GetValuePayload(string key) {
			return	payload.GetValue(key);
		}

		public	void	ClearPayload() {
			payload.RemoveAll();
		}

		public	bool	Parse(byte[] data)
		{
			if (data.Length < PHead.HEAD_LEN) return false;

			header.Set(data, 0);
			if (header.IsEqual(OP_IMG))
			{
				image = new byte[data.Length - PHead.HEAD_LEN];
				System.Array.Copy(data, PHead.HEAD_LEN, image, 0, image.Length);
				return true;
			}
			else
			{
				short	payload_len	= header.GetPayloadLen();
				//String	temp	= Encoding.UTF8.GetString(data, PHead.HEAD_LEN, data.Length - PHead.HEAD_LEN);
				String	temp	= Encoding.UTF8.GetString(data, PHead.HEAD_LEN, payload_len);
				//String	temp	= Encoding.Unicode.GetString(data, PHead.HEAD_LEN, payload_len);
				Console.WriteLine("Client > " + temp);

				int image_len	= data.Length - PHead.HEAD_LEN - payload_len;
				if (image_len > 0) 
				{
					image	= new byte[image_len];
					System.Array.Copy(data, data.Length - image_len, image, 0, image.Length);
				}

				return ParsePayload(temp);
			}
		}

		public	bool	ParsePayload(string json)
		{
			payload = JObject.Parse(json);
			return true;
		}

		public	void	SetOK()
		{
			payload["ErrCode"]	= "0000";
		}

		public	void	SetError(string err_code, string err_msg)
		{
			payload["ErrCode"]	= err_code;
			payload["ErrMsg"]	= err_msg;
		}

		public	bool	IsOK()
		{
			try {
				string errCode = GetValuePayload("ErrCode").ToString();
				if (errCode == "0000")	return	true;
			} catch (Exception ignore) {}
			return	false;
		}


		// 1. 정말 이럴줄 모르고 ㅠㅠㅠ
		//    UTF-8일경우 BOM(인코딩 임 확인) 이 두;에 0xEF, 0xBB, 0xBF 3byte가 더 들어가서
		//    안드로이드와 통신을 하는데 한참 걸림...
		public	byte[]	Build()
		{
			if (header.IsEqual(OP_IMG)) {
				int length = image.Length + PHead.HEAD_LEN;
				byte[] data = new byte[length + 4];

				length = IPAddress.HostToNetworkOrder(length);
				byte[] bytes = BitConverter.GetBytes(length);

				byte[] hBytes = header.GetBytes();

				System.Array.Copy(bytes	, data, 4);
				System.Array.Copy(hBytes, 0, data, 4, hBytes.Length);
				System.Array.Copy(image	, 0, data, 4 + PHead.HEAD_LEN, image.Length);

				return data;
			} else {
				Console.WriteLine("Json str :" + payload.ToString());

				byte[] temp = Encoding.UTF8.GetBytes(payload.ToString());
				//byte[] temp = Encoding.Unicode.GetBytes(payload.ToString());
				header.SetPayloadLen((short)temp.Length);

				int length = temp.Length + PHead.HEAD_LEN;
				if (image != null)	length	= length + image.Length;
				byte[] data = new byte[length + 4];

				length = IPAddress.HostToNetworkOrder(length);
				byte[] bytes = BitConverter.GetBytes(length);

				header.SetPayloadLen((short)temp.Length);
				byte[] hBytes = header.GetBytes();

				System.Array.Copy(bytes, data, 4);
				System.Array.Copy(hBytes, 0, data, 4, hBytes.Length);
				System.Array.Copy(temp, 0, data, 4 + PHead.HEAD_LEN, temp.Length);

				if (image != null) {
					System.Array.Copy(image, 0, data, 4 + PHead.HEAD_LEN + temp.Length, image.Length);
				}

				return data;
			}
		}

		public	override string	ToString()
		{
			return string.Format("header : {0}, payload : {1}", header.ToString(), payload.ToString());
		}
	}
}
