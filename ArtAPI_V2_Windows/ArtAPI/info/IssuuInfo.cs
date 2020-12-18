using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.info
{
    public  class   IssuuInfo
    {
        public  Artist  mArtist;

        public  string  main_image;
        public  string  main_title;

        public  string  artist_note;
        public  string  artist_intro;
        public  string  group_exibition;

        public  List<IssuuDoc>   docs    = new List<IssuuDoc>();


        public  string  MakeTest()
        {
            JObject test    = new JObject();


            return  "";
        }


        public  int     Parse(string json)
        {
            return  0;
        }

    }
}
