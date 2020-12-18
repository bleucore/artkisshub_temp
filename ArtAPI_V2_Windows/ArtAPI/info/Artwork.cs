using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.info
{
    public  class   Artwork
    {
        public  string  _id;

        public  string  owner;

        public  string  artistkey;
        public  string  title;
        public  string  year;

        public  string  size;
        public  string  material;

        public  string  createdAt;
        public  string  updatedAt;

        public  string  image;

        public void Parse(JObject json) {
            try {
                _id         = GetValue(json["_id"]);
                owner       = GetValue(json["owner"]);
                artistkey   = GetValue(json["artistkey"]);
                title       = GetValue(json["title"]);
                year        = GetValue(json["year"]);
                size        = GetValue(json["size"]);
                material    = GetValue(json["material"]);
                createdAt   = GetValue(json["createdAt"]);
                updatedAt   = GetValue(json["updatedAt"]);
            } catch (Exception e) {
            }
        }

        public string GetValue(JToken token) {
            if (token != null) return token.ToString();
            return "";
        }
    }
}
