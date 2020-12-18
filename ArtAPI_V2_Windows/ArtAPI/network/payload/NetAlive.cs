using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ArtAPI.info;
using ArtAPI.utils;

namespace ArtAPI.network.payload
{
	public	class	NetAlive {
		private	string	id	= "";
		DateUtil	dateUtil	= new DateUtil();

		public	void	SetPayload(Protocol req) {
			req.AddPayload("cmd"	, "alive");
			req.AddPayload("id"		, Global.mDeviceID);
			req.AddPayload("time"	, dateUtil.ToString());
		}

		public	AliveInfo	Parse(Protocol protocol) {
			AliveInfo	info	= new AliveInfo();
			string		time	= protocol.GetValuePayload("time").ToString();
			info.mTime	= dateUtil.ToDateTime(time);

			return	info;
		}

		//public	NetAlive(string id) {
		//	this.id	= id;
		//}
	}
}
