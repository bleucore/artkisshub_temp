using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.info
{
	public	class	LogInfo
	{
		public	static	char[]	mLevelString	= {'T', 'D', 'I', 'W', 'E', 'F'};

		public	int		mLevel		= 0;
		public	string	mTag		= "";
		public	string	mTime		= "";
		public	string	mMsg		= "";

		public	static	int		ToLevelString(char level) {
			switch(level) {
			case 'T':	return	1;
			case 'D':	return	2;
			case 'I':	return	3;
			case 'W':	return	4;
			case 'E':	return	5;
			case 'F':	return	6;
			}
			return	0;
		}
		
		public	LogInfo() {}

		public	LogInfo(char level, string tag, string time, string msg) {
			mLevel	= ToLevelString(level);
			mTag	= tag;
			mTime	= time;
			mMsg	= msg;
		}

		public	LogInfo(int level, string tag, string time, string msg) {
			mLevel	= level;
			mTag	= tag;
			mTime	= time;
			mMsg	= msg;
		}
	}

}
