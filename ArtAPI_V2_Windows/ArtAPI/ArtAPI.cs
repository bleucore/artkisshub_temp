using ArtAPI.info;
using ArtAPI.utils;
using ArtAPI;

using ArtAPI.network.payload;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArtAPI.library;
using ArtAPI.network;
using System.Net;
using Newtonsoft.Json.Linq;
using ArtAPI.Adapter;
using Leadtools.Codecs;
using Leadtools;
using RestSharp;

namespace ArtAPI
{
	// 전역변수 설정
    public	static	class	Global
    {
//        public	static	CtrlMainForm_old		mMainForm;
		public	static	string			mAppPath;

		public	static	string			mDeviceID;
		// -------------------------------------------------------
		//public	static	string			mCrackFilePath;


		public	static	MainForm		mMainForm;
		public	static	GateWay			mGateWay;       // single instance

        public  static  ArtKissApt      mArtKissApt;    // single instance
        public  static  EtsyApt         mEtsyApt;       // single instance
        public  static  IssuuApt        mIssuuApt;      // single instance
        public  static  DocuSignApt     mDocuSignApt;   // single instance


        public  static  List<Artist>    mArtists        = new List<Artist>();

        // ----------------------------------------------------------------------------
        //            구간 단속을 위한 
        //public	static	float	SectionDistance		= 10.0f;
        //public	static	int		CutSpeed_1			= 100;
        //public	static	int		CutSpeed_2			=  90;

        public  static	List<RaceInfo>	mRaceList	    = new List<RaceInfo>();
        // ----------------------------------------------------------------------------
        public  static  string          ReportPath      = Environment.CurrentDirectory;

        //public static   CrackReport     mCrackReport    = new CrackReport();

        public static	LibWrapper.Blowfish	mBlowfish = null;

		public	static	ImageUtil	imageUtil	= new ImageUtil();
		public	static	IniUtil		iniUtil		= new IniUtil();
		public	static	DateUtil	dateUtil	= new DateUtil();

        public	static	void	Init() {

			StringBuilder str_temp = new StringBuilder();
			GetPrivateProfileString("System", "DeviceID", "", str_temp, 1000, Const.gIniFile);
			mDeviceID	= str_temp.ToString();
			
			// ----------------------------------------------------------
			// 로그 관련
			// ----------------------------------------------------------
			GetPrivateProfileString("Log", "Path", "", str_temp, 1000, Const.gIniFile);
			LogUtil.mLogPath	= str_temp.ToString();

			LogUtil.mLevelW		= (int)GetPrivateProfileInt("Log", "Level_W", 0, Const.gIniFile);
			LogUtil.mLevelD		= (int)GetPrivateProfileInt("Log", "Level_D", 0, Const.gIniFile);
			LogUtil.mLevelN		= (int)GetPrivateProfileInt("Log", "Level_N", 0, Const.gIniFile);
			// ----------------------------------------------------------

            // ----------------------------------------------------------
            // Data FilePath 관련
            // ----------------------------------------------------------
            GetPrivateProfileString("File", "ReportPath", ReportPath, str_temp, 1000, Const.gIniFile);
            ReportPath      = str_temp.ToString();

            //Global.mCurSite		= TBSiteInfo.Login("site001", "1234");
            //Global.mCurSite.Init();

            //mVideoManager.MakeVideoTable(Global.mCurSite);

            //RecordInfo	rInfo	= new RecordInfo();
            //rInfo.Set("C:\\Project\\AnyAI4D\\Application\\Windows\\WinAI4D_200503_V1\\WinAI4D\\bin\\x64\\Debug\\Video\\20200513");

            int mOverSpeed	= 32;
			string te = mOverSpeed.ToString("D3");

			// 1. 날자 Format
			DateTime dti03 = Convert.ToDateTime("2015-11-13 12:23:34");


			mBlowfish = new LibWrapper.Blowfish("FEDCBA9876543210");
			String	plain	= "BlowwFIshhgk한글DFhhhhhh!";
			Console.WriteLine("plain   : {0}", plain);
			String	encrypt = mBlowfish.Encrypt(plain);
			//String encrypt = mBlowfish.Encrypt("BlowwFIshhhhhh!");
			Console.WriteLine("Encrypt : {0}", encrypt);
			String decrypt = mBlowfish.Decrypt(encrypt);
			//byte[] temp = mBlowfish.Decrypt(encrypt);
			//						           "A1EAF42D7D1C47D74FCC2BC213B0599B9F4E70B6B5B218E9BB215BB7133DBF42FB529D5D3177AB45"
			//									E4FCD1A43EA8CE7E8D9E982A568F6F494765EC67506208CD5FED4E637AD961EF
			//String decrypt = mBlowfish.Decrypt("434D98558CE2B34700009989000085600000C60A0000895A0000BAFC00008919");
			//String decrypt = mBlowfish.Decrypt("E4FCD1A43EA8CE7E8D9E982A568F6F494765EC67506208CD5FED4E637AD961EF");
			//byte[] temp = Encoding.UTF8.GetBytes(decrypt);

			//Console.WriteLine("Decrypt : {0}", Encoding.UTF8.GetString(temp));
			Console.WriteLine("Decrypt : {0}", decrypt);

			LibWrapper.Blowfish8B mBlowfish8B = null;
			mBlowfish8B = new LibWrapper.Blowfish8B("FEDCBA9876543210");
			plain	= "BlowwFIshhgkddDFhhhhhh!";
			Console.WriteLine("plain   : {0}", plain);
			encrypt	= mBlowfish.Encrypt(plain);
			Console.WriteLine("Encrypt : {0}", encrypt);
			decrypt = mBlowfish.Decrypt(encrypt);
			Console.WriteLine("Decrypt : {0}", decrypt);
			mAppPath	= Path.GetDirectoryName(Application.ExecutablePath);

            //run the program again and close this one
			Console.WriteLine(System.Environment.GetCommandLineArgs()[0]);

			//StringBuilder str = new StringBuilder();

			//mGateWay = new GateWay();
			mGateWay = GateWay.Instance;
			mGateWay.Init();

            // ----------------------------------------------------------
            // ArtKiss 관련
            // ----------------------------------------------------------
            mArtKissApt = ArtKissApt.Instance;
            //mArtKiss.StartService();

            // ----------------------------------------------------------
            // Docu Sign 관련
            // ----------------------------------------------------------
            mDocuSignApt = DocuSignApt.Instance;

            GetPrivateProfileString("DocuSign", "IntegratorKey", "", str_temp, 1000, Const.gIniFile);
            mDocuSignApt.IntegratorKey  = str_temp.ToString();
            GetPrivateProfileString("DocuSign", "UserID", "", str_temp, 1000, Const.gIniFile);
            mDocuSignApt.UserID         = str_temp.ToString();
            GetPrivateProfileString("DocuSign", "OAuthBasePath", "", str_temp, 1000, Const.gIniFile);
            mDocuSignApt.OAuthBasePath  = str_temp.ToString();

            GetPrivateProfileString("DocuSign", "ftpIP", "", str_temp, 1000, Const.gIniFile);
            string  ftpIP   = str_temp.ToString();
            GetPrivateProfileString("DocuSign", "ftpID", "", str_temp, 1000, Const.gIniFile);
            string  ftpID   = str_temp.ToString();
            GetPrivateProfileString("DocuSign", "ftpPW", "", str_temp, 1000, Const.gIniFile);
            string  ftpPW   = str_temp.ToString();

            mDocuSignApt.mFTPUtil.Set(ftpIP, ftpID, ftpPW);

            GetPrivateProfileString("DocuSign", "ftpPath", "", str_temp, 1000, Const.gIniFile);
            mDocuSignApt.mFTPUtil.ftpPath   = str_temp.ToString();


            // ----------------------------------------------------------
            // Etsy 관련
            // ----------------------------------------------------------
            mEtsyApt = EtsyApt.Instance;

            GetPrivateProfileString("Esty", "AppName", "", str_temp, 1000, Const.gIniFile);
            mEtsyApt.mAppName       = str_temp.ToString();
            GetPrivateProfileString("Esty", "KeyString", "", str_temp, 1000, Const.gIniFile);
            mEtsyApt.mKeyString     = str_temp.ToString();
            GetPrivateProfileString("Esty", "SharedSecret", "", str_temp, 1000, Const.gIniFile);
            mEtsyApt.mSharedSecret  = str_temp.ToString();

            // ----------------------------------------------------------
            // Issuu 관련
            // ----------------------------------------------------------
            mIssuuApt = IssuuApt.Instance;

            GetPrivateProfileString("ISSUE", "BaseUrl", "", str_temp, 1000, Const.gIniFile);
            IssuuApt.URL_BASE_ISSUU = str_temp.ToString();

            string resUpload = mIssuuApt.Upload();
            //JObject ResUpload = new JObject(list);




            // 5B8BBFCAF6D27F22AD2702A8BC5D1623
            // 5b8bbfcaf6d27f22ad2702a8bc5d1623
            //Console.WriteLine("issuu list -> {0}", issuuList);

            string pdfFile = @"C:\FireFly\Report\F1593_00000001.pdf";
            string imageFile = @"C:\FireFly\Report\20200720\F1593_00000001.jpg";

            using (RasterCodecs _codecs = new RasterCodecs())
            {
                try { 
                    using (RasterImage _image = _codecs.Load(pdfFile))
                    {
                        _codecs.Save(_image, imageFile, RasterImageFormat.Jpeg, 24);
                    }
                } catch(Exception e) {
                    // 라이센스 없음으로 에러가 난다.. ㅠㅠㅠㅠ
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("");

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public  static  Artist  FindArtist(string id)
        {
            if (string.IsNullOrEmpty(id))       return  null;

            foreach(Artist artist in mArtists) {
                if (id.CompareTo(artist._id) == 0)  return  artist;
            }
            return  null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
		public	static	void	RaceAction(Object observer) {

			Console.WriteLine("여기서 Socket으로 쏜다....ActionDelay : {0}", observer);
		}

		public	static	void	SoundPlay(string file) {
			Thread t2 = new Thread(new ThreadStart(delegate () {
				string myFile = Path.Combine(mAppPath, @"Sound\"+file);
				System.Media.SoundPlayer player = new System.Media.SoundPlayer(myFile);
				player.Play();
            }));
            t2.Start(); // 스레드 시작               
		}

        public	static	void	RestartApp()
        {
            try {
                //run the program again and close this one
				Console.WriteLine(System.Environment.GetCommandLineArgs()[0]);

                Process.Start(System.Environment.GetCommandLineArgs()[0]);
                //or you can use Application.ExecutablePath

                //close this one
                Process.GetCurrentProcess().Kill();
            } catch { }
            /*
            Application.Restart();
            Environment.Exit(0);
            */
            /*
            // Wait for the process to terminate
            Process process = null;
            try
            {
                process = Process.GetProcessById(pid);
                process.WaitForExit(2000);
                //doma.Close();
                process.Kill();
            }
            catch (ArgumentException ex)
            {
                // ArgumentException to indicate that the 
                // process doesn't exist?   LAME!!
            }
            Process.Start(Application.ProductName, "");

            // 
            Thread.Sleep(200);
            Application.Exit();
            */
        }
		[DllImport("kernel32")]
		private static	extern	int		GetPrivateProfileSectionNames(byte[] lpszReturnBuffer, int nSize, string lpFileName);

		[DllImport("kernel32")]
		private static	extern	long	WritePrivateProfileString(String section, String key, String val, String filePath);

        [DllImport("kernel32")]
        private static	extern	int		GetPrivateProfileString(string section, string key, string def, StringBuilder retVal,
                                                        int size, string filePath);
		[DllImport("kernel32")]
		public	static	extern	uint	GetPrivateProfileInt( string lpAppName, string lpKeyName, int nDefault, string lpFileName );
    }

	static  class   ArtAPI
	{
		/// <summary>
		/// 해당 응용 프로그램의 주 진입점입니다.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Global.mMainForm	= new MainForm();
			Global.Init();

            Global.mDocuSignApt.Init(Global.mMainForm);

            Application.Run(Global.mMainForm);
		}
	}

	public	static  class   Const {
		public	const	string	mVersion		= @"1.0.0.1";
		public	const	string	mAppName		= @"ArtAPI";					// 로그파일 이름에 쓰이므로 한글 안된다....
		public	const	string	mCompany		= @"ArtAndArtTech";
		public	const	string	mAppDesc		= @"미술 작품 콜라보 소비재 C2B 융합 플랫폼";
		
		public	const	string	mDomain			= @"artapi.awooltech.com";

		public	const	string	gIniFile		= @".\\Config\\ArtAPI.ini";

        public  const   string gURL_ARTKISS = @"https://api.artkiss.info/admin";
        //public const string gURL_ARTKISS = @"https://api.artkiss.info/artists";

        public  const   string  gURL_DOCU       = @"https://developers.docusign.com/";
        // GET https://openapi.etsy.com/v2/listings/active?api_key={YOUR_API_KEY}

        public  const   string  gURL_ISSUU      = @"https://developer.issuu.com/ ";
        // http://api.issuu.com/1_0?apiKey=<apiKey>&signature=<signature>&action=<method>
    }

}
