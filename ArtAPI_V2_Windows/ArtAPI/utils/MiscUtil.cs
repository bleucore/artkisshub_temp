using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.utils
{
	class MiscUtil
	{
		public	enum	CarType
		{
			None	= 0,
			Sedan	= 1,		// 승용(Sedan)	: 01 ~ 69 
			Van		= 2,		// 승합(Van)	: 70 ~ 79
			Bus		= 3,		// 버스(Bus)	: 72, 73
			Truck	= 4,		// 화물(Truck)	: 80 ~ 97
			Pick	= 5,		// 특수(Pick)	: 98, 99

			Etc		= 99
		}

		public	string	GetValue4CarType(string car_no) {
			CarType	type	= GetCarType(car_no);
			switch(type) {
			case CarType.None:		return	"0";
			case CarType.Sedan:		return	"1";
			case CarType.Van:		return	"2";
			case CarType.Bus:		return	"3";
			case CarType.Truck:		return	"4";
			case CarType.Pick:		return	"5";
			}
			return	"9";
		}

		public	CarType	GetCarType(string car_no) {
			
			string	temp	= car_no;

			int		idx	= IsHangul(temp);

			while(idx == 0) {
				temp	= temp.Substring(1, temp.Length-1);
				idx		= IsHangul(temp);
			} 
			if (idx > 3 )				return	CarType.None;

			string	head	= temp.Substring(0, idx);
			
			int.TryParse(head, out int type);

			if (type >= 100)					return	CarType.Sedan;
			if (type >= 01 && type <= 69)		return	CarType.Sedan;
			if (type == 72 || type == 73)		return	CarType.Bus;
			if (type >= 70 && type <= 79)		return	CarType.Van;
			if (type == 98 || type == 99)		return	CarType.Pick;
			if (type >= 80 && type <= 97)		return	CarType.Truck;

			return	CarType.Etc;
		}

		public	int		IsHangul(string str) {
			char[] inputchars = str.ToCharArray();
			var sb = new StringBuilder();
			for (int idx = 0; idx < inputchars.Length; idx++)
			{
				if (char.GetUnicodeCategory(inputchars[idx]) == UnicodeCategory.OtherLetter)
				{
					return	idx;
				}
			}

			return	0;
		}

        static void ListFtpDirectory(string url, NetworkCredential credentials)
        {
            WebRequest listRequest = WebRequest.Create(url);
            listRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            listRequest.Credentials = credentials;

            List<string> lines = new List<string>();

            using (WebResponse listResponse = listRequest.GetResponse())
            using (Stream listStream = listResponse.GetResponseStream())
            using (StreamReader listReader = new StreamReader(listStream)) {
                while (!listReader.EndOfStream) {
                    string line = listReader.ReadLine();
                    lines.Add(line);
                }
            }

            foreach (string line in lines) {
                string[] tokens =
                    line.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
                string name = tokens[8];
                string permissions = tokens[0];

                if (permissions[0] == 'd') {
                    Console.WriteLine($"Directory {name}");

                    string fileUrl = url + name;
                    ListFtpDirectory(fileUrl + "/", credentials);
                } else {
                    Console.WriteLine($"File {name}");
                }
            }
        }

    }
}
