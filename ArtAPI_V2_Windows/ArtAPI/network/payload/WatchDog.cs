using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.network.payload
{
	public	class	WatchDog {

		public void SetInfo(Protocol request)
		{
			request.AddPayload("cmd"	, "event");
			request.AddPayload("kind"	, "info");				// or event
			request.AddPayload("항목1"	, "테스트1");           // or event
		}

		public void SetControl(Protocol request) {
			request.AddPayload("cmd"	, "event");
			request.AddPayload("kind"	, "control");			// or event
			request.AddPayload("항목1"	, "테스트1");			// or event
		}
	}
}
