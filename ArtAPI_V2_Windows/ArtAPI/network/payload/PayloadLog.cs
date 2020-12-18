using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAPI.info;

namespace ArtAPI.network.payload
{
	public	class	PayloadLog {
		public	void	SetPayload(Protocol req, LogInfo info) {
			req.AddPayload("cmd"	, "event");
			req.AddPayload("kind"	, "log");
			req.AddPayload("tag"	, info.mTag);
			req.AddPayload("time"	, info.mTime);
			req.AddPayload("level"	, info.mLevel.ToString());
			req.AddPayload("message", info.mMsg);
		}
	}
}
