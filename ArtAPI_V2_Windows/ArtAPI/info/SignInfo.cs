using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.info
{
    public  class   SignInfo
    {
        public  string  artist_id;      
        public  string  sign_id;        // DocuSign id

        public  string  type;           // 계약 종류 -> 콜라보, 용역, 작품 구매
        public  string  title;          // 계약몇

        public  string  s_date;         // 계약 시작일
        public  string  e_date;         // 계약 종료일

        public   List<CompInfo>     comps   = new List<CompInfo>();

        public  string  desc;           // 계약 설명

        public  string  pay_type;       // 계약금 지급 방법
        public  string  amount;         // 계약 금액


        public bool    Parse(JObject obj)
        {
            if (obj == null)        return  false;

            artist_id = obj["artist_id"].ToString();
            sign_id = obj["sign_id"].ToString();
            type    = obj["type"].ToString();
            title   = obj["title"].ToString();
            s_date  = obj["s_date"].ToString();
            e_date  = obj["artist_id"].ToString();

            sign_id = obj["artist_id"].ToString();

            return true;
        }

        public  static  JObject MakeTestData(string artist_id, string sign_id, string prefix)
        {
            JObject obj = new JObject();
            obj["artist_id"]  = artist_id;
            obj["sign_id"]  = sign_id;
            obj["type"]     = "콜라보";
            obj["title"]    = "대전행사";
            obj["s_date"]   = prefix + " -> s_date";
            obj["e_date"]   = prefix + " -> e_date";
            obj["pay_type"] = prefix + " -> pay_type";
            obj["amount"]   = prefix + " -> amount";
            obj["desc"]     = prefix + " -> desc";

            JObject j1 = CompInfo.MakeTestDataToJSON("Test 1");
            JObject j2 = CompInfo.MakeTestDataToJSON("Test 2");
            JObject j3 = CompInfo.MakeTestDataToJSON("Test 3");

            JArray  arr = new JArray();
            arr.Add(j1);
            arr.Add(j2);
            arr.Add(j3);

            obj["comps"]    = arr;

            Console.WriteLine("{0}", obj.ToString());

            return  obj;
        }
    }
}
