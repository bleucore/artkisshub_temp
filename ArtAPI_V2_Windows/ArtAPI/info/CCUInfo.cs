using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ArtAPI.info
{
	public	class	CCUInfo
	{
		public	string	mID			= "";
		public	string	mSite		= "";
		public	string	mAddr		= "";			// 주소

		public	int		mRoadNum	= 0;		// 단속 차선
		public	int		mCutSpeed	= 0;		// 단속 속도

		public	string	mStatus		= "";

		public	CCUInfo(string id) {
			mID	= id;
		}

		public	void	Load(string file) {
			StringBuilder strValue = new StringBuilder(255);
			GetPrivateProfileString(mID, "Site"		, "", strValue, 255, file);
			mSite		= strValue.ToString();

			GetPrivateProfileString(mID, "RoadNum"	, "0", strValue, 255, file);
			if (!Int32.TryParse(strValue.ToString(), out mRoadNum))
				mRoadNum	= 0;
			
			GetPrivateProfileString(mID, "CutSpeed"	, "0", strValue, 255, file);
			if (!Int32.TryParse(strValue.ToString(), out mCutSpeed))
				mCutSpeed	= 0;

			GetPrivateProfileString(mID, "Address"	, "", strValue, 255, file);
			mAddr		= strValue.ToString();

			mStatus		= "Ready";
		}

		public	void	Save(string file) {
		}

		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
														int size, string filePath);
	}
}