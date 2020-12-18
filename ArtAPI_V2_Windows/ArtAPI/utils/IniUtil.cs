using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace ArtAPI.utils
{
	public	class	IniUtil
	{

		public	string	GetString(string section, string key, string def, string file) {
			arUtil.INIHelper ini = new arUtil.INIHelper(file);

			StringBuilder str_temp = new StringBuilder();
			GetPrivateProfileString(section, key, def, str_temp, 1000, file);
			return	ini.get_Data(section, key, def);

			/*
			byte[]	bytes	= Encoding.UTF8.GetBytes(str_temp.ToString());
			string	decoding	= Encoding.UTF8.GetString(bytes);

			Console.WriteLine("decoding : {0}", decoding);
			

			return	Encoding.UTF8.GetString(bytes);
			*/
		}

        [DllImport("kernel32")]
        private static	extern	int		GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
                                                        int size, string filePath);
		[DllImport("kernel32")]
		public	static	extern	uint	GetPrivateProfileInt( string lpAppName, string lpKeyName, int nDefault, string lpFileName );

		[DllImport("kernel32")]
		public static extern int GetPrivateProfileSectionNames(byte[] lpszReturnBuffer, int nSize, string lpFileName);
	}
}
