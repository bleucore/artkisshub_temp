using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAPI.utils;

namespace ArtAPI.info
{
	public	class	RecordInfo
	{
		public	Dictionary<string, DateTb>	mRecDates	= new Dictionary<string, DateTb>();
		DateUtil					dateUtil	= new DateUtil();

		public	class	MinTb {
			public	int		min	= 0x00;
			public	byte	exist	= 0x00;
			public	byte[]	secs	= new byte[60];

			public	void	Set(int minute, int sec, byte val) {
				min			= minute;
				exist		= 0x01;
				secs[sec]	= val;
			}

			public	string	ToOneLineString() {
				return	min.ToString() + " : " + string.Join(",", secs);
			}
		}

		public	class	HourTb {
			public	int		hour	= 0x00;
			public	byte	exist	= 0x00;
			public	MinTb[]	mins	= new MinTb[60];

			public	void	Set(DateTime time, byte val) {
				hour		= time.Hour;
				exist		= 0x01;
				var	min		= mins[time.Minute];
				if (mins[time.Minute] == null)	mins[time.Minute]	= new MinTb();

				mins[time.Minute].Set(time.Minute, time.Second, val);
			}

			public	void	Clear() {
				for(int idx = 0; idx < 60; idx++) {
					if (mins[idx] != null)		mins[idx]	= null;
				} 
			}

			public	void	ToConsole() {
				Console.WriteLine("hour : {0}", hour);
				foreach(var min in mins) {
					if (min != null)
						Console.WriteLine(min.ToOneLineString());
				}
			}
		};

		public	class	DateTb
		{
			public	DateTime		mDate;
			public	List<Size>		mDrawRecs	= new List<Size>();
			public	HourTb[]		mHours		= new HourTb[24];

			public	bool	Set(DateTime sTime, DateTime eTime, byte val) {
				if (sTime == null || eTime == null)		return	false;

				Console.WriteLine($"{eTime.Ticks - sTime.Ticks}");
				Size	size	= GetSize(sTime, eTime);
				mDrawRecs.Add(size);
				while (!eTime.Equals(sTime)) {
					if (mHours[sTime.Hour] == null)		mHours[sTime.Hour]	= new HourTb();
					mHours[sTime.Hour].Set(sTime, val);
					sTime	= sTime.AddSeconds(1);
				}
				Console.WriteLine($"{sTime}, {eTime}");
				return	true;
			}

			public	void	Clear() {
				for(int idx = 0; idx < 24; idx++) {
					if (mHours[idx] == null)	continue;
					mHours[idx].Clear();
					mHours[idx]	= null;
				} 
			}

			public	DateTb(DateTime date) {
				mDate	= date;
			}

			public	Size	GetSize(DateTime sTime, DateTime eTime) {
				return	new Size(sTime.Hour * 60 * 60 + sTime.Minute * 60 + sTime.Second, 
									eTime.Hour * 60 * 60 + eTime.Minute * 60 + eTime.Second);
			}

			public	void	ToConsole() {
				Console.WriteLine("date : {0}", mDate.ToString());
				foreach(var hour in mHours) {
					if (hour != null)	hour.ToConsole();
				}
			}
		}

		public	void	Clear() {
			List<string> keys	= new List<string>(mRecDates.Keys);
			for(int idx = 0; idx < keys.Count; idx++) {
				mRecDates.TryGetValue(keys[idx], out var dateTb);
				dateTb.Clear();
			}
			mRecDates.Clear();
		}

		public	bool	Set(DateTime date, string[] tuple, byte val) {
			if (tuple == null || tuple.Length < 3)		return	false;

			string	dateStr	= date.ToString("yyyyMMdd");
			/*
			// 날자\\CamID_시간.ts 
			// 1. 현재 맨뒤의 Directory 얻어서 날자로 Parsing
			string fullPath		= Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar);
			string dateStr		= fullPath.Split(Path.DirectorySeparatorChar).Last();
			*/
			DateTime	sTime	= dateUtil.ToTimeOnlyNoDash(tuple[1]);
			DateTime	eTime	= dateUtil.ToTimeOnlyNoDash(tuple[2]);

			if (mRecDates.ContainsKey(dateStr)) {
				DateTb	dateTb	= mRecDates[dateStr];
				dateTb.Set(sTime, eTime, val);
			} else {
				DateTb	dateTb	= new DateTb(date);
				dateTb.Set(sTime, eTime, val);
				mRecDates.Add(dateStr, dateTb);
			}
			return	true;
		}

		public	void	ToConsole() {
			foreach(var date in mRecDates) {
				date.Value.ToConsole();
			}
		}
	}
}
