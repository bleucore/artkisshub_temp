using ArtAPI.info;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArtAPI.Adapter
{
    public static class ClientHelper
    {
        // Basic auth
        public static HttpClient GetClient(string username, string password)
        {
            var authValue = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

            var client = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue }
                //Set some other client defaults like timeout / BaseAddress
            };
            return client;
        }

        // Auth with bearer token
        public static HttpClient GetClient(string token)
        {
            //var authValue = new AuthenticationHeaderValue("Bearer", token);

            var client = new HttpClient();
            
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            //DefaultRequestHeaders = { Authorization = authValue }
            //Set some other client defaults like timeout / BaseAddress
            
            return client;
        }
    }
    
    // single instance
    public  sealed  class   ArtKissApt
    {
        public  const   int     LOOP_DELAY          = 1000;

        public  const   string  ARTKISS_ID          = "restapi";
        public  const   string  ARTKISS_PW          = "6HuNPwrvGWbqKvA";

        public  const   string  ARTKISS_LOGIN_URL   = "https://stage.artkiss.info/auth/local";
        public  const   string  ARTKISS_ARTISTS_URL = "https://stage.artkiss.info/artists";

        public  static  string  mJWT;

        public  static  JObject mEstyList;

        // MutexName1 이라는 뮤텍스 생성
        private static  Mutex   mtx     = new Mutex(false, "ArtKissApt");


        //콜백을 전달할 delegate 선언 (인수 전달/)
        public delegate void InitDele(Object observer);
        InitDele mInitDele = null;

        private static readonly ArtKissApt instance = new ArtKissApt();
        public  static ArtKissApt Instance
        {
            get
            {
                return instance;
            }
        }

        public  void    Init(InitDele _deledate)
        {
            mInitDele   = _deledate;
            StartService();
        }

        private ArtKissApt()
        {
        }

        public  string  Login()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(ARTKISS_LOGIN_URL);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"identifier\":\""+ ARTKISS_ID +"\"," +
                              "\"password\":\""+ ARTKISS_PW + "\"}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                JObject payload = JObject.Parse(result);

                mJWT    = payload["jwt"].ToString();
                return  mJWT;

                // Artists
                Console.WriteLine("----------------------------");
            }
        }

        public  JObject GetArtwork(string artist_id)
        {
            //if (artist_id == GetValue(mEstyList["_id"]))
                return  (JObject)mEstyList["art_work"];

            //return  null;
        }

        public async Task<string> GetEstyListAsync()
        {
            //var authValue = new AuthenticationHeaderValue("Bearer", mJWT);

            //var client = new HttpClient()
            //{
            //    DefaultRequestHeaders = { Authorization = authValue }
            //Set some other client defaults like timeout / BaseAddress
            //};
            using (var client = ClientHelper.GetClient("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjVlZmJkYWVkMGZjOWI3MmM0NzhjNTFhNCIsImlzQWRtaW4iOnRydWUsImlhdCI6MTYwNjUyOTk1NiwiZXhwIjoxNjA5MTIxOTU2fQ.VVuV_9HomPQGGP1utJf8wwI4CatmueEXe0ydC-I5abE"))
            {
                HttpResponseMessage result = await client.GetAsync("https://stage.artkiss.info/shopitemregs/5fdacf04f1adb63620413da1");
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
                try
                {
                    mEstyList   = JObject.Parse(stream);
                    //JObject artist      = (JObject)payload["artist"];
                    //JObject art_work    = (JObject)payload["art_work"];

                    /*
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
                    */
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
   

                if (mInitDele != null) mInitDele("load_esty_finish");

                //JObject artist  = (JObject)artists[0];
                Console.WriteLine("----------------------------");

                Console.WriteLine("----------------------------");
                //}

                //JArray payload = JArray.Parse(contents.Read(.ToString());
                //Perform some http call
            }            /*
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + mJWT);
                var response = client.GetStringAsync(ARTKISS_ARTISTS_URL);
                Console.WriteLine("----------------------------");
            }
            */
            return "";
        }

        public string GetValue(JToken token)
        {
            if (token != null) return token.ToString();
            return "";
        }

        public async   Task<string> GetArtistsAsync()
        {
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

                JArray payload  = JArray.Parse(stream);
                foreach(JObject lists in payload)
                {
                    var artist = lists["_id"];
                    Console.WriteLine("----------------------------");
                    if (artist != null) {
                        Artist  info    = new Artist(artist.ToString());
                        info.Parse(lists);
                        Global.mArtists.Add(info);
                    }
                }

                if (mInitDele != null)  mInitDele("load_artist_finish");

                //JObject artist  = (JObject)artists[0];
                Console.WriteLine("----------------------------");

                Console.WriteLine("----------------------------");
                //}

                //JArray payload = JArray.Parse(contents.Read(.ToString());
                //Perform some http call
            }            /*
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + mJWT);
                var response = client.GetStringAsync(ARTKISS_ARTISTS_URL);
                Console.WriteLine("----------------------------");
            }
            */
            return  "";
        }

        public void MonitorProc(object obj)
        {
            // retrieve client from parameter passed to thread
            ArtKissApt manager = (ArtKissApt)obj;

            while (true) {
                // 먼저 뮤텍스를 취득할 때까지 대기
                mtx.WaitOne();
                Thread.Sleep(100);
                /*
                int idx = 0;
                while (idx < mCracks.Count)
                {
                    if (IsRemove(mCracks[idx])) mCracks.RemoveAt(idx);
                    else
                    {
                        Decision(mCracks[idx]);
                        ++idx;
                    }
                }
                */

                // 뮤텍스 해제
                mtx.ReleaseMutex();
                Thread.Sleep(LOOP_DELAY);
            }
        }

        public  int     StartService()
        {
            Login();
            GetArtistsAsync();
            GetEstyListAsync();

            // for Event callback
            Thread t = new Thread(new ParameterizedThreadStart(MonitorProc));

            t.Start(this);
            return 0;
        }
    }
}
