using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.utils
{
	public	class	DateUtil
	{
		public const string STD_CRACK_FILE_FORMAT	= @"yyyyMMddHHmmssff";
		public const string STD_DATE_TIME_FORMAT	= @"yyyy-MM-dd HH:mm:ss";
		public const string STD_DATE_ONLY_FORMAT	= @"yyyy-MM-dd";
		public const string STD_DATE_ONLY_NO_DASH	= @"yyyyMMdd";
		public const string STD_TIME_ONLY_NO_DASH	= @"HHmmss";
		public const string STD_TIME_ONLY_FORMAT	= @"HH:mm:ss";


		// 			DateTime dti03 = Convert.ToDateTime("2015-11-13 12:23:34");


		public	DateTime	ToDateTime(string dateStr)
		{
			return DateTime.Parse(dateStr);
		}

		public	DateTime	ToTimeOnlyNoDash(string timeStr)
		{
			return	DateTime.ParseExact(timeStr, STD_TIME_ONLY_NO_DASH, null);
		}

		public	DateTime	ToTimeOnly(string timeStr)
		{
			return	DateTime.ParseExact(timeStr, STD_TIME_ONLY_FORMAT, null);
		}

		public	DateTime	ToDateOnlyNoDash(string dateStr)
		{
			return	DateTime.ParseExact(dateStr, STD_DATE_ONLY_NO_DASH, null);
		}

		public	string	ToStringOnlyTimeNoDash()
		{
			return DateTime.Now.ToString(STD_TIME_ONLY_NO_DASH);
		}

		public	string	ToStringOnlyDateNoDash()
		{
			return DateTime.Now.ToString(STD_DATE_ONLY_NO_DASH);
		}

		public	string	ToStringMilliseconds()
		{
			return DateTime.Now.ToString(STD_CRACK_FILE_FORMAT);
		}

		public	string	ToString()
		{
			return DateTime.Now.ToString(STD_DATE_TIME_FORMAT);
		}

		public	string ToString(DateTime date)
		{
			return date.ToString(STD_DATE_TIME_FORMAT);
		}

		public	string ToStringOnlyTime()
		{
			return DateTime.Now.ToString(STD_TIME_ONLY_FORMAT);
		}

		public	string ToStringOnlyTime(DateTime date)
		{
			return date.ToString(STD_TIME_ONLY_FORMAT);
		}

		public	string ToStringOnlyDate()
		{
			return DateTime.Now.ToString(STD_DATE_ONLY_FORMAT);
		}

		public	string ToStringOnlyDate(DateTime date)
		{
			return date.ToString(STD_DATE_ONLY_FORMAT);
		}
	}
}
