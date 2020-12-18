using ArtAPI.utils;
using ArtAPI.info;
using ArtAPI.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;

namespace ArtAPI.info
{
	public	class	RaceInfo
	{
		public	string		mType;		// SRace => S, End -> E

		public	DateTime	mTime;
		public	int			mDelay;			// 
		public	int			mActionDelay;

		public	string		mRoad;		// 1, 2, 3, 갓길  <- 이것 때문에 글자로 함.

		public	string		mCarNo;
		public	string		mCarType;

		public	int			mSpeed;

		public	bool		mIsPair;

		public	string		mFileName;

		DateUtil	dateUtil	= new DateUtil();
		MiscUtil	miscUtil	= new MiscUtil();

		private	Timer		mActionTimer	= new Timer();
		public	CrackInfo	mCrackInfo		= new CrackInfo();

        public	delegate	void	ActionDele(Object observer);

		public	ActionDele	mActionDele		= null;

		public	void	StopAction() {
			if (mActionTimer != null)	mActionTimer.Stop();
		}

		public	bool	GoAction(DateTime time) {
			// 시간차를 second로 구하고 1000 을 곱하여 사용.
			TimeSpan dateDiff = mTime - time;
			if (dateDiff.TotalMilliseconds > 0) {
				mActionTimer.Interval	= dateDiff.TotalMilliseconds;
				mActionTimer.Start();
				return	true;
			}

			return	false;
		}

		public	bool	GoAction() {
			// 시간차를 second로 구하고 1000 을 곱하여 사용.
			mActionTimer.Interval	= mActionDelay;
			mActionTimer.Start();
			return	true;
		}

		public	void	TimerAction(Object sender, ElapsedEventArgs e) {
			mActionTimer.Stop();
			if (mActionDele != null)	mActionDele(this);
		}

		public	RaceInfo(ActionDele dele) {
			mActionDele	= dele;
			mActionTimer.Elapsed	+= new ElapsedEventHandler(TimerAction);
		}

		public	bool	Load(string section, string file) {
			IniUtil		iniUtil		= new IniUtil();
			
			if (section.Substring(0,1) == "S")		mType	= "S";
			if (section.Substring(0,1) == "E")		mType	= "E";

			if (string.IsNullOrEmpty(mType))		return	false;

			StringBuilder str_temp = new StringBuilder();
			GetPrivateProfileString(section, "time", "", str_temp, 1000, file);
			if (string.IsNullOrEmpty(str_temp.ToString()))		return	false;
			try {
				mTime		= dateUtil.ToDateTime(str_temp.ToString());
			} catch(Exception e) {
				mTime		= DateTime.Now;
			}

			mDelay			= (int)GetPrivateProfileInt(section, "DelayTime", 0, file);
			mActionDelay	= mDelay;

			GetPrivateProfileString(section, "RoadNum"	, "1", str_temp, 1000, file);
			mRoad		= str_temp.ToString();
			GetPrivateProfileString(section, "CarNo"	, "", str_temp, 1000, file);
			mCarNo		= iniUtil.GetString(section, "CarNo", "", file);

			mCarType	= miscUtil.GetValue4CarType(mCarNo);

			if (string.IsNullOrEmpty(mCarNo))		return	false;
			//mCarNo		= str_temp.ToString();
			mSpeed		= (int)GetPrivateProfileInt(section, "Speed", 0, file);

			GetPrivateProfileString(section, "FileName"	, "", str_temp, 1000, file);
			mFileName	= str_temp.ToString();
			return	true;
		}

        [DllImport("kernel32")]
        private static	extern	int		GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
		[DllImport("kernel32")]
		public	static	extern	uint	GetPrivateProfileInt( string lpAppName, string lpKeyName, int nDefault, string lpFileName );
	}
}
