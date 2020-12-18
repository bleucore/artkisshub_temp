using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.utils
{
	public	static	class StaticUtil
	{
		public static bool ToBoolean(this string value)
		{
			switch (value.ToLower())
			{
				case  "yes":
					return true;
				case  "no":
					return false;
				case  "true":
					return true;
				case "t":
					return true;
				case "1":
					return true;
				case "0":
					return false;
				case "false":
					return false;
				case "f":
					return false;
				default:
					throw new InvalidCastException("You can't cast that value to a bool!");
			}
		}	
	}
}
