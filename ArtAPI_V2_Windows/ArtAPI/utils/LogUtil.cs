using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using ArtAPI.info;

namespace ArtAPI.utils
{
    public class AtomicBoolean
    {
        private const int TRUE_VALUE = 1;
        private const int FALSE_VALUE = 0;
        private int zeroOrOne = FALSE_VALUE;

        public AtomicBoolean() : this(false) { }

        public AtomicBoolean(bool initialValue)
        {
            this.Value = initialValue;
        }

        /// <summary>
        /// Provides (non-thread-safe) access to the backing value
        /// </summary>
        public bool Value
        {
            get
            {
                return zeroOrOne == TRUE_VALUE;
            }
            set
            {
                zeroOrOne = (value ? TRUE_VALUE : FALSE_VALUE);
            }
        }

        /// <summary>
        /// Attempt changing the backing value from true to false.
        /// </summary>
        /// <returns>Whether the value was (atomically) changed from false to true.</returns>
        public bool FalseToTrue()
        {
            return SetWhen(true, false);
        }

        /// <summary>
        /// Attempt changing the backing value from false to true.
        /// </summary>
        /// <returns>Whether the value was (atomically) changed from true to false.</returns>
        public bool TrueToFalse()
        {
            return SetWhen(false, true);
        }

        /// <summary>
        /// Attempt changing from "whenValue" to "setToValue".
        /// Fails if this.Value is not "whenValue".
        /// </summary>
        /// <param name="setToValue"></param>
        /// <param name="whenValue"></param>
        /// <returns></returns>
        public bool SetWhen(bool setToValue, bool whenValue)
        {
            int comparand = whenValue ? TRUE_VALUE : FALSE_VALUE;
            int result = Interlocked.CompareExchange(ref zeroOrOne, (setToValue ? TRUE_VALUE : FALSE_VALUE), comparand);
            bool originalValue = result == TRUE_VALUE;
            return originalValue == whenValue;
        }
    }

	public	interface	CBLogDisplay
	{
		// 멤버 앞에 접근제한자 사용 안함
		int		Disp(char level, string tag, string time, string msg);
	}

	public	interface	CBLogNetwork
	{
		// 멤버 앞에 접근제한자 사용 안함
		int		LogTrans(char level, string tag, string time, string msg);
	}

	/*
	로그레벨은
	DEBUG > INFO > WARN > ERROR > FATAL 순 입니다.
 
	# Log Level
	(1)TRACE : 가장 상세한 정보를 나타낼 때 사용한다.
	(2)DEBUG : 일반 정보를 상세히 나타낼 때 사용한다.
	(3)INFO  : 일반 정보를 나타낼 때 사용한다.
	(4)WARN  : 에러는 아니지만 주의할 필요가 있을 때 사용한다.
	(5)ERROR : 일반 에러가 일어 났을 때 사용한다.
	(6)FATAL : 가장 크리티컬한 에러가 일어 났을 때 사용한다.
	*/
    public	class	LogUtil {
		static readonly object _lock = new object();

		public	static	string	mLogPath	= "/";
		public	static	int		mLevelW		= 0;		
		public	static	int		mLevelD		= 0;		
		public	static	int		mLevelN		= 0;

		public	static	CBLogDisplay	mDisp		= null;
		public	static	CBLogNetwork	mTrans		= null;

		public	static	void	LogT(string tag, string msg) { Log(1, tag, msg);}
		public	static	void	LogD(string tag, string msg) { Log(2, tag, msg);}
		public	static	void	LogI(string tag, string msg) { Log(3, tag, msg);}
		public	static	void	LogW(string tag, string msg) { Log(4, tag, msg);}
		public	static	void	LogE(string tag, string msg) { Log(5, tag, msg);}
		public	static	void	LogF(string tag, string msg) { Log(6, tag, msg);}

		public	static	void	LogEx(LogInfo info) {
			LogEx(info.mLevel, info.mTag, info.mTime, info.mMsg);
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		private	static	void	LogEx(int level, string tag, string time, string msg) {
			if (level >= mLevelD && mDisp != null) {
				mDisp.Disp(LogInfo.mLevelString[level-1], tag, time, msg);
			}

			if (level >= mLevelN && mTrans != null) {
				mTrans.LogTrans(LogInfo.mLevelString[level-1], tag, time, msg);
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public	static	void	Log(int level, string tag, string msg) {
			Console.WriteLine("{0},{1}, {2}",  LogInfo.mLevelString[level-1], tag, msg);
			
			lock (_lock) {
				DateTime dtNow = DateTime.Now;
				string strDate = dtNow.ToString("yyyyMMdd");
				string strPath = String.Format(".\\{0}\\FireFly_{1}.log", mLogPath, strDate);

				string strDir  = Path.GetDirectoryName(strPath);
				DirectoryInfo diDir = new DirectoryInfo(strDir);

				if (!diDir.Exists) {
					diDir.Create();
					diDir = new DirectoryInfo(strDir);
				}

				LogEx(level, tag, dtNow.ToString("hh;mm:ss"), msg);

				if (diDir.Exists && level >= mLevelW) {
					System.IO.StreamWriter swStream = File.AppendText(strPath);
					string strLog = String.Format("{0}|{1}|{2,-6}|{3}", LogInfo.mLevelString[level-1], dtNow.ToString("hh:mm:ss"), tag, msg);
					swStream.WriteLine(strLog);
					swStream.Close(); ;
				}	
			}
		}
	}
}
