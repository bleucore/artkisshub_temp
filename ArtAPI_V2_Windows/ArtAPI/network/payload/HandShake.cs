using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.network.payload
{
	// Client가 요청을 한다...
	public class HandShake
	{
		public	bool	mIsHandShake	= false;

		public	bool	IsMainHandle(Protocol protocol)
		{
			try {
				string cmd	= protocol.GetValuePayload("cmd").ToString();
				if (cmd != "handshake")		return	false;
				string mode	= protocol.GetValuePayload("mode").ToString();
				if (mode == "control")		return	true;
			}
			catch (Exception e) { }
			return false;
		}

		public	bool	IsEventHandle(Protocol protocol)
		{
			try {
				string cmd	= protocol.GetValuePayload("cmd").ToString();
				if (cmd != "handshake")		return	false;
				string mode = protocol.GetValuePayload("mode").ToString();
				if (mode == "event")		return	true;
			}
			catch (Exception e) { }
			return false;
		}

		public	string	GetCryptoKey(Protocol protocol)
		{
			try {
				string	key	= protocol.GetValuePayload("crypto_key").ToString();
				return	key;
			}
			catch (Exception e) { }
			return "";
		}

		public	void	SetReqCtrl(Protocol request)
		{
			request.AddPayload("cmd"	, "handshake");
			request.AddPayload("mode"	, "control");			// or event
		}

		public	void	SetReqEvent(Protocol request)
		{
			request.AddPayload("cmd"	, "handshake");
			request.AddPayload("mode"	, "event");          // or event
		}

		public	void	SetResCtrl(Protocol response) {
			response.AddPayload("cmd"	, "handshake");
			response.AddPayload("mode"	, "control");          // or event
			response.SetOK();                                //
		}

	}
}
