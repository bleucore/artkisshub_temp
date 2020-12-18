using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.info
{
	public	class	AliveInfo
	{
		public	DateTime	mTime;
		public	string		mCCUID;

		public	override	string	ToString() {
			return	string.Format($"CCU-ID : {mCCUID}, time : {mTime}");
		}
	}
}
