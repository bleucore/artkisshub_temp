using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArtAPI.info;
using ArtAPI.utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp.Serialization.Json;

namespace ArtAPI.Adapter
{
    // https://developers.issuu.com/search/examples/
    public sealed   class   IssuuApt
    {
        public  const   string  APIKEY          = "6t482e2hsh1j4rzy22dovsk3hzcvjx5v";
        public  const   string  APISECRET       = "heo7b1p7l1mundq1se4u23ux82egstn5";

        //public static string URL_BASE_ISSUU = "http://api.issuu.com/1_0";
        //public static string URL_BASE_ISSUU = "api.issuu.com";
        public  static  string  URL_BASE_ISSUU    = "http://api.issuu.com";
        public  const   string  ACTION_LIST     = "issuu.documents.list";

        public  const   string  ACTION_UPLOAD   = "issuu.document.upload";
        // accountId
        // userId
        private static  Formatting _formatting  = Formatting.None;

        public  static  bool    mAliveMode      = false;
        public  static  int     mAliveTime;
        public  static  int     mReadTimeout    = 0;

        public  static  int     mEventPort      = 9002;

        private         Mutex   mtx = new Mutex(false, "IssuuApt");

        // connection들 모음.... 
        //private List<ConnInfo> mHandles = new List<ConnInfo>();


        public  List<IssuuInfo> mIssuues    = new List<IssuuInfo>();
        public  IssuuInfo       mCurIssuu;


        private static readonly IssuuApt instance = new IssuuApt();
        public static IssuuApt Instance {
            get {
                return instance;
            }
        }

        public  IssuuInfo   AppendIssuuInfo(string artist_id)
        {
            IssuuInfo   iInfo   = GetList(artist_id);
            iInfo.mArtist   = new Artist(artist_id);

            foreach(IssuuInfo issuu in mIssuues)
            {
                if (issuu.mArtist != null)
                {
                    if (issuu.mArtist._id == artist_id)
                    {
                        mCurIssuu   = issuu;
                        return  mCurIssuu;
                    }
                }
            }

            mIssuues.Add(iInfo);
            mCurIssuu = iInfo;

            return  mCurIssuu;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public  string  GetFullContents(string name)
        {
            //https://issuu.com/artnartech.com/docs/bieaf2_2

            return string.Format("https://issuu.com/artnartech.com/docs/{0}", name);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public  string  GetThumbUrl(string doc_id)
        {
            //https://image.issuu.com/200914191952-9047244b8650d80eaaf5e868c3cfdc31/jpg/page_1_thumb_large.jpg
            return string.Format("https://image.issuu.com/{0}/jpg/page_1_thumb_large.jpg", doc_id);
        }

        // https://developer.issuu.com/ 
        // http://api.issuu.com/1_0?apiKey=<apiKey>&signature=<signature>&action=<method>
        // API key	    qyy6ls1qv15uh9xwwlvk853u2uvpfka7
        // API secret	13e3an36eaxjy8nenuepab05yc7j7w5g
        public  IssuuInfo   GetList(string artist_id)
        {
            IssuuInfo   iInfo   = new IssuuInfo();
            //string field = "name,documentId,title,tag";

            string  field   = "name,documentId,title";
            string  action  = "issuu.documents.list";
            string  str     = APISECRET + "accesspublic" + "action" + action + "apiKey" + APIKEY + "formatjson" + "responseParams" + field;
            //string str = "13e3an36eaxjy8nenuepab05yc7j7w5g" + "accesspublic" + "action" + "issuu.documents.list" + "apiKey" + "qyy6ls1qv15uh9xwwlvk853u2uvpfka7" + "formatjson" + "responseParamstitle,description";
            string  signature   = MD5(str);
            // 		"7431D31140CF412AB5CAA73586D6324A"	string


            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["action"]            = action;
                values["apiKey"]            = APIKEY;
                values["access"]            = "public";
                values["responseParams"]    = field;
                values["format"]            = "json";
                values["signature"]         = signature.ToLower();

                var response = client.UploadValues("http://api.issuu.com/1_0", values);

                // 		responseString	"{\"rsp\":{\"_content\":{\"result\":{\"totalCount\":1,\"startIndex\":0,\"pageSize\":10,\"more\":false,\"_content\":[{\"document\":{\"name\":\"bieaf2_2\",\"documentId\":\"200914191952-9047244b8650d80eaaf5e868c3cfdc31\",\"title\":\"17th Busan International Environment Art Festival\"}}]}},\"stat\":\"ok\"}}"	string
                
                var responseString = Encoding.Default.GetString(response);

                string rest = responseString.Replace("\"", "'");

                //string json = JsonConvert.SerializeObject(responseString, _formatting);
                //T targetObject = JsonConvert.DeserializeObject<T>(json);
                /*
                '"{\"rsp\":{
                    \"_content\":{
                            \"result\":{
                                \"totalCount\":1,
                                \"startIndex\":0,
                                \"pageSize\":10,
                                \"more\":false,
                                \"_content\":[
                                        {
                                            \"document\":{
                                                \"name\":\"bieaf2_2\",
                                                \"documentId\":\"200914191952-9047244b8650d80eaaf5e868c3cfdc31\",
                                                \"title\":\"17th Busan International Environment Art Festival\"}
                                        }
                                        ]
                                    }
                            },
                    \"stat\":\"ok\"}
                }"'

                https://image.issuu.com/200914191952-9047244b8650d80eaaf5e868c3cfdc31/jpg/page_1_thumb_large.jpg
                https://issuu.com/artnartech.com/docs/bieaf2_2
                */

                JObject data = JObject.Parse(rest);
                try {
                    JObject rsp = (JObject)data["rsp"];
                    if (rsp["stat"].ToString() != "ok")     return  null;

                    JToken _content = rsp["_content"];
                    JToken  result  = (JObject)_content["result"];

                    int.TryParse(result["totalCount"].ToString(), out int totalCount);

                    // 여기서 Loop를 돈다...
                    JToken content = result["_content"];
                    foreach (JToken token in content) {

                        JToken  doc     = token["document"];
                        IssuuDoc    iDoc    = new IssuuDoc();

                        iDoc.mDocID     = doc["documentId"].ToString();
                        iDoc.mName      = doc["name"].ToString();
                        iDoc.mTitle     = doc["title"].ToString();

                        iDoc.mThumb     = GetThumbUrl(iDoc.mDocID);
                        iDoc.mUrl       = GetFullContents(iDoc.mName);

                        iInfo.docs.Add(iDoc);

                        Console.WriteLine(token.ToString());
                    }

                    return  iInfo;
                } catch(Exception e) { }

                //Console.WriteLine("response : {0}", rest);

                return  null;
            }
        }

        /*
        curl -F "file=@\"/myfolder/myfilename.pdf\";filename=\"myfilename.pdf\""\
         -F "action=issuu.document.upload"\
         -F "apiKey=qyy6ls1qv15uh9xwwlvk853u2uvpfka7"\
         -F "name=racing"\
         -F "title=Race Cars"\
         -F "signature=810b910ed5c8a53d704fd062a6001b22"\
         http://upload.issuu.com/1_0
        */

        // API key	    qyy6ls1qv15uh9xwwlvk853u2uvpfka7
        // API secret	13e3an36eaxjy8nenuepab05yc7j7w5g
        public string  Upload()
        {
            string  access  = "public";
            string  name    = "racing1";
            string  title   = "Race Cars";
            string  action  = "issuu.document.upload";
            string  file    = "C:\\Project\\ArtAPI\\자료\\issuu_test.pdf";
            string  filename    = "myfilename.pdf";

            
            string  str     = APISECRET +   "action" + action +
                                            "apiKey" + APIKEY +
                                            "name" + name +
                                            "title" + title;

            /*
            curl -F "file=@\"/myfolder/myfilename.pdf\";filename=\"myfilename.pdf\""\
                 -F "action=issuu.document.upload"\
                 -F "apiKey=qyy6ls1qv15uh9xwwlvk853u2uvpfka7"\
                 -F "name=racing"\
                 -F "title=Race Cars"\
                 -F "signature=810b910ed5c8a53d704fd062a6001b22"\
                 http://upload.issuu.com/1_0
            */
            // ArtKiss
            //str = "heo7b1p7l1mundq1se4u23ux82egstn5" + "action" + "issuu.document.upload" + "apiKey" + "6t482e2hsh1j4rzy22dovsk3hzcvjx5v" + "nameracingtitleRace Cars";
            // signature   "CC6F8B92E61559A1F0044CF93E576D8B"  

            // Issuu Example
            //     13e3an36eaxjy8nenuepab05yc7j7w5g     action     issuu.document.upload     apiKey     qyy6ls1qv15uh9xwwlvk853u2uvpfka7     nameracingtitleRace Cars
            //str = "13e3an36eaxjy8nenuepab05yc7j7w5g" + "action" + "issuu.document.upload" + "apiKey" + "qyy6ls1qv15uh9xwwlvk853u2uvpfka7" + "nameracingtitleRace Cars";
            string signature = MD5(str);
            // signature   "810b910ed5c8a53d704fd062a6001b22"
            Console.WriteLine("str -> {0}\nsignature -> {1}", str, signature.ToLower());
            var responseString =  HttpMultiPart.PostMultipart(
                "http://upload.issuu.com/1_0",
                new Dictionary<string, object>() {
                    //{ "access"  , access },
                    //{ "action"  , action },
                    //{ "apiKey"  , APIKEY },
                    //{ "name"    , name },                    
                    { "action"  , action },
                    { "apiKey"  , APIKEY },
                    { "name"    , name },
//                    { "file"    , new FormFile() { Name = "myfilename.pdf", ContentType = "image/jpeg", FilePath = "C:\\Project\\ArtAPI\\자료\\issuu_test.pdf" } },
                    { "file"    , new FormFile() { Name = "myfilename.pdf", ContentType = "pdf", FilePath = "/myfolder/myfilename.pdf" } },
                    { "title"   , title },
                    { "signature"   , signature.ToLower() }
                });

                //var responseString = Encoding.Default.GetString(response);
                string  rest = responseString.Replace("\"", "'");
                // 에러 처리를 한다...
            
            return  "";
        }

            /*
HttpMultiPart.PostMultipart(
    "http://www.myserver.com/upload.php", 
    new Dictionary<string, object>() {
        { "testparam", "my value" },
        { "file", new FormFile() { Name = "image.jpg", ContentType = "image/jpeg", FilePath = "c:\\temp\\myniceimage.jpg" } },
        { "other_file", new FormFile() { Name = "image2.jpg", ContentType = "image/jpeg", Stream = imageDataStream } },
    });
             */

            //public  string  GetIssuuInfo

        public string  MD5(string str)
        {
            StringBuilder MD5Str = new StringBuilder();
            byte[] byteArr = Encoding.ASCII.GetBytes(str);
            byte[] resultArr = (new MD5CryptoServiceProvider()).ComputeHash(byteArr);

            //for (int cnti = 1; cnti < resultArr.Length; cnti++) (2010.06.27)
            for (int cnti = 0; cnti < resultArr.Length; cnti++) {
                MD5Str.Append(resultArr[cnti].ToString("X2"));
            }
            return MD5Str.ToString();
        }

        public  void sendPost()
        {
            // Définition des variables qui seront envoyés
            HttpContent stringContent1 = new StringContent("test"); // Le contenu du paramètre P1
            HttpContent stringContent2 = new StringContent("date"); // Le contenu du paramètre P2
            var paramFileStream = new FileStream("c:\\TemporyFiles\\test.jpg", FileMode.Open);

            HttpContent fileStreamContent = new StreamContent(paramFileStream);
            //HttpContent bytesContent = new ByteArrayContent(paramFileBytes);

            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(stringContent1, "P1"); // Le paramètre P1 aura la valeur contenue dans param1String
                formData.Add(stringContent2, "P2"); // Le parmaètre P2 aura la valeur contenue dans param2String
                formData.Add(fileStreamContent, "FICHIER", "RETURN.xml");
                //  formData.Add(bytesContent, "file2", "file2");
                try
                {
                    var response = client.PostAsync(ACTION_UPLOAD, formData).Result;
                    MessageBox.Show(response.ToString());
                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Erreur de réponse");
                    }
                }
                catch (Exception Error)
                {
                    MessageBox.Show(Error.Message);
                }
                finally
                {
                    client.CancelPendingRequests();
                }
            }
        }

        /*
        public async void MultiPartsAsync()
        {
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();

            form.Add(new StringContent(username), "username");
            form.Add(new StringContent(useremail), "email");
            form.Add(new StringContent(password), "password");
            form.Add(new ByteArrayContent(file_bytes, 0, file_bytes.Length), "profile_pic", "hello1.jpg");
            HttpResponseMessage response = await httpClient.PostAsync("PostUrl", form);

            response.EnsureSuccessStatusCode();
            httpClient.Dispose();
            string sd = response.Content.ReadAsStringAsync().Result;
        }
        */

        private TcpListener _server;
        private Boolean     _isRunning;

        public void HandleClient(object obj)
        {
            // retrieve client from parameter passed to thread
            TcpClient client = (TcpClient)obj;

            // sets two streams
            StreamWriter sWriter = new StreamWriter(client.GetStream(), new UTF8Encoding(false));
            StreamReader sReader = new StreamReader(client.GetStream(), new UTF8Encoding(false));

            if (mReadTimeout != 0)
                sReader.BaseStream.ReadTimeout = mReadTimeout;

            // you could use the NetworkStream to read and write, 
            // but there is no forcing flush, even when requested
            Boolean bIsConnected = true;

            byte[] nLen     = new byte[4];

            ConnInfo mConnInfo = null;

            // HandShake 과정을 진행하고 Key를 받는다....

            while (bIsConnected)
            {
                // reads from stream
                int retv = nRead(client, sReader, ref nLen);
                Console.WriteLine("rData : read bytes is {0}", retv);
                if (mConnInfo != null)  mConnInfo.Touch();

                if (retv > 0)
                {
                    Console.WriteLine(HexDump.Dump(nLen, nLen.Length));

                    /*
                    int len = BitConverter.ToInt32(nLen, 0);
                    //Console.WriteLine("rData : network Len is {0}", len);

                    len = IPAddress.NetworkToHostOrder(len);
                    Console.WriteLine("rData : host order  is {0}", len);
                    */
                    string str = Encoding.Default.GetString(nLen);

                    int.TryParse(str, out int len);

                    if (len < 0 || len > 200000048) continue;

                    byte[] rData = new byte[len];
                    retv = nRead(client, sReader, ref rData);

                    Console.WriteLine(HexDump.Dump(rData, rData.Length));

                    //string rStr	= Encoding.UTF8.GetString(rData);
                    //string rStr	= Encoding.Unicode.GetString(rData);
                    //Console.WriteLine("rStr ->" + rStr);
                    /*
                    if (rMsg.Parse(rData))
                    {
                        Console.WriteLine(rMsg.ToString());
                        if (!handShake.mIsHandShake)
                        {
                            if (handShake.IsMainHandle(rMsg))
                            {
                                string device_id = rMsg.GetValuePayload("device_id").ToString();

                                // 1. 지금 연결을 Control Handle로 하고
                                // 2. Crypto 키를 발급하고 전송한다....
                                mConnInfo = new ConnInfo();
                                mConnInfo.mCryptoKey = Guid.NewGuid().ToString();
                                mConnInfo.mDeviceID = device_id;

                                sMsg.AddPayload("crypto_key", mConnInfo.mCryptoKey);
                                handShake.SetResCtrl(sMsg);

                                // Send data
                                byte[] hData = sMsg.Build();
                                Console.WriteLine(HexDump.Dump(hData, hData.Length));

                                try
                                {
                                    sWriter.BaseStream.Write(hData, 0, hData.Length);
                                    //sWriter.BaseStream.Write(sData, 0, 22);
                                    sWriter.Flush();

                                    mConnInfo.mCtrlHandle = client;
                                    mConnInfo.mCtrlReader = sReader;
                                    mConnInfo.mCtrlWriter = sWriter;

                                    mtx.WaitOne();
                                    mHandles.Add(mConnInfo);
                                    mtx.ReleaseMutex();

                                    handShake.mIsHandShake = true;
                                }
                                catch (Exception e)
                                {
                                    mConnInfo.mCtrlHandle = null;
                                    Console.WriteLine(e.Message);
                                }
                                continue;
                            }

                            if (handShake.IsEventHandle(rMsg))
                            {
                                string device_id = rMsg.GetValuePayload("device_id").ToString();
                                // event이면
                                mConnInfo = FindConnInfo(device_id);
                                if (mConnInfo == null)
                                {
                                    // Ctrl handle이 없어요. 다시 Handshake하세요....
                                    sMsg.ClearPayload();
                                    sMsg.SetError("1002", "Not Ctrl handle");
                                }
                                else
                                {
                                    mConnInfo.mEventHandle = client;
                                    mConnInfo.mEventReader = sReader;
                                    mConnInfo.mEventWriter = sWriter;

                                    handShake.mIsHandShake = true;

                                    sMsg.ClearPayload();
                                    sMsg.SetOK();
                                }

                                try
                                {
                                    // Send data
                                    byte[] hData = sMsg.Build();
                                    Console.WriteLine(HexDump.Dump(hData, hData.Length));
                                    sWriter.BaseStream.Write(hData, 0, hData.Length);
                                    //sWriter.BaseStream.Write(sData, 0, 22);
                                    sWriter.Flush();

                                    // 여기서 끝이어야 한다...
                                    // Event recv가 따로 ....
                                    Console.WriteLine("NetServer : Event recv가 따로 ....");
                                    bIsConnected = false;
                                }
                                catch (Exception e)
                                {
                                    mConnInfo.mCtrlHandle = null;
                                    Console.WriteLine(e.Message);
                                }
                                break;
                                //continue;
                            }
                        }
                        else
                        {
                            //if (!rMsg.IsOK()) {
                            //	LogUtil.LogD("LOG", rMsg.ToString());
                            //	continue;
                            //}
                            if (Global.mGateWay != null)
                            {
                                sMsg = Global.mGateWay.Proccess(rMsg);
                            }
                        }
                    }
                    else
                    {
                        sMsg.SetError("E01", "message parse error");
                    }
                    */

                    /*
                    try
                    {
                        // Send data
                        byte[] sData = sMsg.Build();
                        if (sData.Length > 300) Console.WriteLine(HexDump.Dump(sData, 300));
                        else Console.WriteLine(HexDump.Dump(sData, sData.Length));
                        sWriter.BaseStream.Write(sData, 0, sData.Length);
                        //sWriter.BaseStream.Write(sData, 0, 22);
                        sWriter.Flush();
                    }
                    catch (Exception e)
                    {
                        mConnInfo.mCtrlHandle = null;
                        Console.WriteLine(e.Message);
                    }

                    System.Array.Clear(rData, 0, rData.Length);
                    */
                }
                else
                {
                    // Socket이 끊어 졌는지... 확인을 한다.....
                    if (!IsSocketConnected(client))
                    {
                        Console.WriteLine("NetServer : TcpClient Socket이 끊어짐");
                        bIsConnected = false;

                        // 해당하는 채널을 전체 Close;
                        mtx.WaitOne();
                        if (mConnInfo != null) mConnInfo.Close();
                        mtx.ReleaseMutex();
                    }
                }
            }
        }

        public void    LoopClients(object obj)
        {
            while (_isRunning) {
                // wait for client connection
                TcpClient newClient = _server.AcceptTcpClient();

                // client found.
                // create a thread to handle communication
                Thread t = new Thread(new ParameterizedThreadStart(HandleClient));
                t.Start(newClient);
            }
        }

        public  IssuuApt()
        {
            _server = new TcpListener(IPAddress.Any, mEventPort);
            _server.Start();

            _isRunning = true;


            Console.WriteLine("Server ---------------> start[" + mEventPort + "]");
            Thread t = new Thread(new ParameterizedThreadStart(LoopClients));
            t.Start();

            //Thread m = new Thread(new ParameterizedThreadStart(ConnMonitor));
            //m.Start(this);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        static bool IsSocketConnected(TcpClient client)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            try
            {
                TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections().Where(x => x.LocalEndPoint.Equals(client.Client.LocalEndPoint) && x.RemoteEndPoint.Equals(client.Client.RemoteEndPoint)).ToArray();

                if (tcpConnections != null && tcpConnections.Length > 0)
                {
                    TcpState stateOfConnection = tcpConnections.First().State;
                    if (stateOfConnection == TcpState.Established)
                    {
                        return true;            // Connection is OK
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public int nRead(TcpClient client, StreamReader reader, ref byte[] bytes)
        {
            int retv = 0;
            int nRead = 0;
            while (nRead < bytes.Length)
            {
                try
                {
                    retv = reader.BaseStream.Read(bytes, nRead, bytes.Length - nRead);
                    if (retv > 0)
                    {
                        nRead += retv;
                    }
                    else
                    {
                        if (!IsSocketConnected(client))
                        {
                            return -1;
                        }
                    }
                }
                catch (Exception e)
                {
                    if (!IsSocketConnected(client))
                    {
                        return -2;
                    }
                }
            }
            return nRead;
        }

        /*
        public void ConnMonitor(object obj)
        {
            IssuuApt netServer = (IssuuApt)obj;

            bool isRemove = false;

            while (true) {
                for (int idx = 0; idx < netServer.mHandles.Count; idx++)
                {
                    ConnInfo info = netServer.mHandles[idx];
                    try {
                        isRemove = false;

                        if (mAliveMode && info.ValidAlive(DateTime.Now, mAliveTime)) {
                            isRemove = true;
                        }

                        if (info.mEventHandle == null) {
                            isRemove = true;
                        }

                        if (isRemove) {
                            // Close시키고 삭제한다.
                            netServer.mtx.WaitOne();
                            info.Close();
                            netServer.mHandles.RemoveAt(idx);
                            netServer.mtx.ReleaseMutex();
                        }
                    } catch (Exception e) {}
                }
                Thread.Sleep(1000);
            }
        }
        */
    }
}
