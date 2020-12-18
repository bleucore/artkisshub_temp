using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArtAPI.Adapter
{
    public  sealed  class   EtsyApt
    {
        // GET https://openapi.etsy.com/v2/listings/active?api_key={YOUR_API_KEY}
        public  const   string  URL_LISTINGS = "https://openapi.etsy.com/v2/listings/active?api_key=";
        public  const   string  URL_ETSY    = @"https://developer.issuu.com/";

        public  string  mAppName        = "ARTKiss Shop";
        public  string  mKeyString      = "ld30domg8zmyfc6b3p24ag5r";
        public  string  mSharedSecret   = "1zj6z5g2ii";

        // MutexName1 이라는 뮤텍스 생성
        private static  Mutex   mtx     = new Mutex(false, "EtsyApt");


        //콜백을 전달할 delegate 선언 (인수 전달/)
        public delegate void InitDele(Object observer);
        InitDele mInitDele = null;

        private static readonly EtsyApt instance = new EtsyApt();
        public  static EtsyApt  Instance
        {
            get
            {
                return instance;
            }
        }

        private EtsyApt()
        {
        }

        public  void    Init(InitDele _deledate)
        {
            mInitDele   = _deledate;
            GetListAsync();
        }

        public  string  GetList()
        {
            string  json;
            using (WebClient wc = new WebClient()) {
                json = new WebClient().DownloadString( URL_LISTINGS + mKeyString);
                Console.WriteLine(json.ToString()); 
            }
            return  json;
        }


        public  async   Task<string>    GetListAsync()
        {
            string json;
            using (WebClient wc = new WebClient())
            {
                json = new WebClient().DownloadString(URL_LISTINGS + mKeyString);
                Console.WriteLine(json.ToString());
                if (mInitDele != null)  mInitDele("load_esty_finish");
            }
            /*
            //var authValue = new AuthenticationHeaderValue("Bearer", mJWT);

            //var client = new HttpClient()
            //{
            //    DefaultRequestHeaders = { Authorization = authValue }
            //Set some other client defaults like timeout / BaseAddress
            //};
            using (var client = ClientHelper.GetClient(mJWT))
            {
                HttpResponseMessage result = await client.GetAsync(ARTKISS_ARTISTS_URL);
                Console.WriteLine("----------------------------");

                //var contents = await result.Content.ReadAsByteArrayAsync();
                //var contents = await result.Content.ReadAsByteArrayAsync();
                Stream contents = result.Content.ReadAsStreamAsync().Result;

                //var responseString = Encoding.UTF8.GetString(contents, 0, contents.Length - 1);
                //JObject payload = JObject.Parse(responseString);

                //var stream = await result.Content.ReadAsStringAsync();
                var datas = await result.Content.ReadAsByteArrayAsync();
                //{
                //var bytes = new byte[100000];
                //var bytesread = stream.Read(bytes, 0, 100000);
                //stream.Close();

                string stream = Encoding.UTF8.GetString(datas);
                //JObject payload = JObject.Parse(stream);

                Console.WriteLine(stream);

                JArray payload = JArray.Parse(stream);
                foreach (JObject lists in payload)
                {
                    var artist = lists["_id"];
                    Console.WriteLine("----------------------------");
                    if (artist != null)
                    {
                        Artist info = new Artist(artist.ToString());
                        info.Parse(lists);
                        Global.mArtists.Add(info);
                    }
                }

                if (mInitDele != null) mInitDele("load_artist_finish");

                //JObject artist  = (JObject)artists[0];
                Console.WriteLine("----------------------------");

                Console.WriteLine("----------------------------");
                //}

                //JArray payload = JArray.Parse(contents.Read(.ToString());
                //Perform some http call
            }           

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + mJWT);
                var response = client.GetStringAsync(ARTKISS_ARTISTS_URL);
                Console.WriteLine("----------------------------");
            }
            */
            return "";
        }

    }
}
