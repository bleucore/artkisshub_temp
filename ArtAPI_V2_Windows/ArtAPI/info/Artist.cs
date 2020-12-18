using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.info
{
    public  class   Artist
    {
        public  string  _id;
        public  string  userid;

        public  string  blog;
        public  string  facebook;
        public  string  ex_career;
        public  string  ex_group_career;
        public  string  Inputvalues;
        public  string  edulevel;
        public  string  homepage;
        public  string  instagram;
        //public  string  userkey;
        public  string  profileimgurl;
        public  string  localname;
        public  string  awards;
        public  string  usercountryname;
        public  string  englishname;
        public  string  email;
        public  string  profileurl;
        public  string  mobile;

        public  List<Artwork>   mArtwork    = new List<Artwork>();

        /*
        "contact" -> 연락처:"031-992-4400",
        "period":"2020.12.21 - 2020.12.26",
        "businesshour":"평일 10:00 - 18:00 | 휴관일 없음",
        "gallery":"부산광역시청 전시실/Busan MetroTown CityHall Gallery",
        "entryfee":"무료",
        "address":"부산광역시 연제구 연산동 중앙대로 1001",
        "profileimgurl":"https://images.unsplash.com/photo-1588612568467-a6b245a1f4a5?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=800&q=60",
        "location":"부산/Busan",
        "website":"http://www.bieaf.org",
        "likes":"1923",
        */
        // 전시회 Title
        //public static string[] titles = { "연락처", "전시기간", "입장시간", "전시장", "입장료", "주소", "지역", "홈페이지", "좋아요" };

        /*
        public  string  profileimgurl; // 프로필 이미지
        public  string  localname; //작가명(현지어)
        public  string  englishname; //작가명(영문)
        public  string  edulevel; // 학력
        public  string  ex_career; //개인전 경력
        public  string  ex_group_career; //그룹전 경력 
        public  string  awards; // 수상경력

        public  string  email; // 이메일
        public  string  mobile; //연락처(모바일)
        public  string  usercountryname; //국가

        public  string  facebook; //작가 페이스북 
        public  string  instagram; //작가 인스타그램
        public  string  blog; //작가 블로그 
        public  string  homepage; // 홈페이지
        */

        public  static  string[]    titles = { "작가명", "(영문)", "학력", "개인전 경력", "그룹전 경력", "수상경력",
                                    "이메일", "연락처(모바일)", "국가", "페이스북", "인스타그램", "블로그", "홈페이지"};

        public  Artist(string _id)
        {
            this._id    = _id;
        }


        public void    Parse(JObject json)
        {
            try { 
                _id         = GetValue(json["_id"]);
                userid      = GetValue(json["userid"]);
                /*
                contact     = GetValue(json["contact"]);
                period = GetValue(json["period"]);
                businesshour = GetValue(json["businesshour"]);
                gallery = GetValue(json["gallery"]);
                entryfee = GetValue(json["entryfee"]);
                address = GetValue(json["address"]);
                location = GetValue(json["location"]);
                website = GetValue(json["website"]);
                likes = GetValue(json["like"]);
                */
                facebook    = GetValue(json["facebook"]);
                ex_career   = GetValue(json["ex_career"]);
                //ex_group_career = GetValue(json["ex_group_career"]);
                Inputvalues = GetValue(json["Inputvalues"]);
                edulevel    = GetValue(json["edulevel"]);
                homepage    = GetValue(json["homepage"]);
                instagram   = GetValue(json["instagram"]);
                profileimgurl = GetValue(json["profileimgurl"]);
                localname   = GetValue(json["localname"]);
                awards      = GetValue(json["awards"]);
                usercountryname = GetValue(json["usercountryname"]);
                englishname = GetValue(json["englishname"]);
                email       = GetValue(json["email"]);
                mobile      = GetValue(json["mobile"]);

                //profileimg  = 
                JObject profile = (JObject)json["profileimg"];
                if (profile != null)
                    profileurl  = GetValue(profile["url"]);
                /*
                contact: 연락처
                period: 전시기간
                businesshour: 입장시간
                gallery: 전시장
                entryfee: 입장료
                address: 주소
                location: 지역
                website: 홈페이지
                likes: 좋아요
                */
                foreach (JObject artwork in json["artworks"]) {
                    Artwork aInfo = new Artwork();
                    aInfo.Parse(artwork);
                    mArtwork.Add(aInfo);
                    Console.WriteLine(artwork.ToString());
                }

            } catch(Exception e) {
            }
        }

        public  string  GetValue(JToken token)
        {
            if (token != null)      return  token.ToString();
            return  "";
        }
    }
}
