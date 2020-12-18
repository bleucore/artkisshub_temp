using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAPI.network;

namespace ArtAPI.process
{
	public	class	ProcEvent {
		public Protocol Exec(Protocol req)
		{
			/*
			string kind = req.GetValuePayload("kind").ToString();
			switch (kind)
			{
				case "section_cut":
					//return SectionCut(req);
				//case "get":
					//return Get(req);
			}
			*/

			return null;
		}

		public	void	SectionCut()
		{
			
		}

	}
}
