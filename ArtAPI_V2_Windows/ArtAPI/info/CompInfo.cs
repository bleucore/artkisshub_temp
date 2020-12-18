using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.info
{
    public  class   CompInfo
    {
        public  string  comp_id;

        public  string  name;
        public  string  b_no;
        public  string  phone;
        public  string  address;

        public bool Parse(JObject obj)
        {
            if (obj == null) return false;

            comp_id = obj["comp_id"].ToString();
            name    = obj["name"].ToString();
            b_no    = obj["b_no"].ToString();
            phone   = obj["phone"].ToString();
            address = obj["address"].ToString();

            return true;
        }

        public static  JObject MakeTestDataToJSON(string prefix)
        {
            JObject obj = new JObject();
            obj["comp_id"]  = prefix + " : comp_id";
            obj["name"]     = prefix + " : name";
            obj["b_no"]     = prefix + " : b_no";
            obj["phone"]    = prefix + " : phone";
            obj["address"]  = prefix + " : address";
            return  obj;
        }
    }
}
