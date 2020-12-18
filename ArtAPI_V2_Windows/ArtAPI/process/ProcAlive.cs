using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAPI.info;
using ArtAPI.network;
using ArtAPI.network.payload;

namespace ArtAPI.process
{
	public	class	ProcAlive {
		public	Protocol	Exec(Protocol req) {
			NetAlive	payload		= new NetAlive();
			AliveInfo	alive		= payload.Parse(req);

			// ------------------------------------------------
			// Alive에서 시간을 알아온다...
			// ------------------------------------------------
			Console.WriteLine("Alive : {0}", alive.ToString());

			Protocol	res			= new Protocol(Protocol.OP_RES);
			res.AddPayload("Name", "ProcAlive");

			// 시간을 얻어온다.....
			payload.SetPayload(res);

			res.SetOK();
			return	res;
		}
	}
}
