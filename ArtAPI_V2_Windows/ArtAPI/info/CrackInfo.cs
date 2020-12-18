using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAPI.utils;

namespace ArtAPI.info
{
	public	class	CrackInfo
	{
		public	const		string			CRACK_KUBUN_FIX		= "F";
		public	const		string			CRACK_KUBUN_STX		= "S";
		public	const		string			CRACK_KUBUN_ETX		= "E";

		DateUtil	dateUtil	= new DateUtil();

		// PropertyGrid
		// http://whiteat.com/WhiteAT_Csharp/34124

		public	static	string[]	titles = {"CCU ID", "위반속도", "차종", "위반차로", "위반날자", "위반시간", "차량번호", 
								"번호판X", "번호판Y", "번호판W", "번호판H", "번호판유무", "위반코드", "위반종류",
								"정지선통과", "교차로통과", "프레임수", "신호등색", "차량진행", "신호등종류"};

		public	string		mKubun			= "N";		// "F" : 지정 지역, "S" : 시작지점, "E" : 끝지점.

		private	string		mCCUNo			= "N";
		[CategoryAttribute("ID Settings"), DescriptionAttribute("Color of the customer")]
		public	string		CCUNo {
			get { return mCCUNo; }
			set { mCCUNo = value; }	
		}

		private	CCUInfo		mCCUInfo		= null;

		public	int			mOverSpeed		= 0;		// 위반속도
		public	string		mCarKind		= "N";		// 차종
		public	string		mRoadNum		= "1";		// 위반차로

		// 일자와 시간을 같이 취급.
		public	DateTime	mDateTime		= new DateTime();
		public	string		mDate			= "";
		public	string		mTime			= "";
		public	string		mMilliSec		= "";

		public	string		mCarNO			= "N";		// 자량번호
		public	int			mCarNoX			= 0;
		public	int			mCarNoY			= 0;
		public	int			mCarNoW			= 0;
		public	int			mCarNoH			= 0;

		public	string		mCarNoKind		= "";		// 번호판종류
		public	string		mCrackCode		= "";
		public	string		mCrackKind		= "";		// 위반종류

		public	string		mStopLine		= "";
		public	string		mCrossCut		= "";

		public	string		mFrameNum		= "";
		public	string		mSinhoColor		= "";
		public	string		mMoveKind		= "";
		public	string		mSinhoKind		= "";		// 신호등 종류


		// 중간에 Data가 붕 떴음....
		//public	string		mCrackKind		= "";

		public	bool		mIsOver			= false;	// 속도위반

		public	Image		mMainImage		= null;
		public	Image		mCarNoImage		= null;
		public	Image[]		mSubImages		= new Image[8];

		public	string		mFileName		= "";

		// 각종 정보
		// ....
		/*
		MemoryStream stream = new MemoryStream(rImage.image);
		stream.Position = 0;
		pb_road_main_cam.Image = Image.FromStream(stream);
		*/
		public	CrackInfo() {
			mCCUNo		= "";
			mKubun		= "N";
		}

		public	CrackInfo(string ccu_no, string kubun) {
			mCCUNo		= ccu_no;
			mKubun		= kubun;
		}

		public	bool		ParseImage(byte[] image) {
			if (image == null)		return	false;
			using (var ms = new MemoryStream(image))
			{
				byte[]	length = new byte[7];
				ms.Read(length, 0, length.Length);

				int	len;

				Int32.TryParse(Encoding.Default.GetString(length), out len);
				byte[]	mainImage	= new byte[len];
				ms.Read(mainImage, 0, mainImage.Length);
				mMainImage		= ImageUtil.GetImage(mainImage);

				ms.Read(length, 0, length.Length);
				
				Int32.TryParse(Encoding.Default.GetString(length), out len);
				Console.WriteLine(HexDump.Dump(length));

				byte[]	carNoImage	= new byte[len];
				ms.Read(carNoImage, 0, carNoImage.Length);
				mCarNoImage			= ImageUtil.GetImage(carNoImage);

				for(int i = 0; i < 8; i++) {
					ms.Read(length, 0, length.Length);
				
					Int32.TryParse(Encoding.Default.GetString(length), out len);
					Console.WriteLine(HexDump.Dump(length));
				
					byte[]	subImage	= new byte[len];
					ms.Read(subImage, 0, subImage.Length);

					mSubImages[i]		= ImageUtil.GetImage(subImage);
				}
			}
			return	true;
		}

		public	bool	ParseHead(byte[] head) {

			mCCUNo		= Encoding.Default.GetString(head,  7,  5);
			Int32.TryParse(Encoding.Default.GetString(head,12,  3), out mOverSpeed);
			mCarKind	= Encoding.Default.GetString(head, 15,  1);
			mRoadNum	= Encoding.Default.GetString(head, 16,  1);

			mDate		= Encoding.Default.GetString(head, 17,  8);
			mTime		= Encoding.Default.GetString(head, 25,  6);

			mCarNO		= Encoding.Default.GetString(head, 31, 12);
			Int32.TryParse(Encoding.Default.GetString(head,43,  4), out mCarNoX);
			Int32.TryParse(Encoding.Default.GetString(head,47,  4), out mCarNoY);
			Int32.TryParse(Encoding.Default.GetString(head,51,  4), out mCarNoW);
			Int32.TryParse(Encoding.Default.GetString(head,55,  4), out mCarNoH);

			mCarNoKind	= Encoding.Default.GetString(head, 59,  1);
			mCrackCode	= Encoding.Default.GetString(head, 60,  1);
			mCrackKind	= Encoding.Default.GetString(head, 61,  1);

			mStopLine	= Encoding.Default.GetString(head, 62,  8);
			mCrossCut	= Encoding.Default.GetString(head, 85,  8);

			mFrameNum	= Encoding.Default.GetString(head,108,  2);
			mSinhoColor	= Encoding.Default.GetString(head,110,  1);
			mMoveKind	= Encoding.Default.GetString(head,111,  1);
			mSinhoKind	= Encoding.Default.GetString(head,112,  1);

			return	true;
		}

		public	bool	MakeHead(byte[] head) {

			// Text 부 크기
			byte[]	temp		= Encoding.Default.GetBytes("0000106");
			//System.Array.Copy(bytes, data, 4);
			System.Array.Copy(temp		, 0, head,  0,  7);

			byte[]	ccu_no		= Encoding.Default.GetBytes(mCCUNo);
			System.Array.Copy(ccu_no	, 0, head,  7,  5);

			byte[]	over_speed	= Encoding.Default.GetBytes(mOverSpeed.ToString("D3"));
			System.Array.Copy(over_speed, 0, head, 12,  3);

			byte[]	car_kind	= Encoding.Default.GetBytes(mCarKind);
			System.Array.Copy(car_kind	, 0, head, 15,  1);

			byte[]	road_num	= Encoding.Default.GetBytes(mRoadNum);
			System.Array.Copy(road_num	, 0, head, 16,  1);

			byte[]	date		= Encoding.Default.GetBytes(mDate);
			System.Array.Copy(date		, 0, head, 17,  8);

			byte[]	time		= Encoding.Default.GetBytes(mTime);
			System.Array.Copy(time		, 0, head, 25,  6);

			byte[]	car_no		= Encoding.Default.GetBytes(mCarNO.PadLeft(12));
			System.Array.Copy(car_no	, 0, head, 31, 12);

			byte[]	car_no_x	= Encoding.Default.GetBytes(mCarNoX.ToString("D4"));
			System.Array.Copy(car_no_x	, 0, head, 43, 4);

			byte[]	car_no_y	= Encoding.Default.GetBytes(mCarNoY.ToString("D4"));
			System.Array.Copy(car_no_y	, 0, head, 47, 4);

			byte[]	car_no_w	= Encoding.Default.GetBytes(mCarNoW.ToString("D4"));
			System.Array.Copy(car_no_w	, 0, head, 51, 4);

			byte[]	car_no_h	= Encoding.Default.GetBytes(mCarNoH.ToString("D4"));
			System.Array.Copy(car_no_h	, 0, head, 55, 4);

			byte[]	car_no_kind	= Encoding.Default.GetBytes(mCarNoKind);
			System.Array.Copy(car_kind	, 0, head, 59,  1);

			byte[]	crack_code	= Encoding.Default.GetBytes(mCrackCode);
			System.Array.Copy(crack_code, 0, head, 60,  1);

			byte[]	crack_kind	= Encoding.Default.GetBytes(mCrackKind);
			System.Array.Copy(crack_kind, 0, head, 61,  1);

			byte[]	stop_line	= Encoding.Default.GetBytes(mStopLine);
			System.Array.Copy(stop_line	, 0, head, 62,  8);

			byte[]	cross_cut	= Encoding.Default.GetBytes(mCrossCut);
			System.Array.Copy(cross_cut	, 0, head, 85,  8);

			byte[]	frame_num	= Encoding.Default.GetBytes(mFrameNum);
			System.Array.Copy(frame_num	, 0, head,108,  2);

			byte[]	sinho_color	= Encoding.Default.GetBytes(mSinhoColor);
			System.Array.Copy(sinho_color,0, head,110,  1);

			byte[]	move_kind	= Encoding.Default.GetBytes(mMoveKind);
			System.Array.Copy(move_kind	, 0, head,111,  1);

			byte[]	sinho_kind	= Encoding.Default.GetBytes(mSinhoKind);
			System.Array.Copy(sinho_kind, 0, head,112,  1);

			return	true;
		}

		public	JObject	GetHeader() {
			JObject	header			= new JObject();

			header["kubun"]			= mKubun;
			header["ccu_no"]		= CCUNo;
			header["위반속도"]		= mOverSpeed.ToString();
			header["차종"]			= mCarKind;
			header["위반차로"]		= mRoadNum;
			header["위반날자"]		= mDate;
			header["위반시간"]		= mTime;
			header["MilliSec"]		= mMilliSec;
			header["차량번호"]		= mCarNO;
			header["번호판X"]		= mCarNoX.ToString();
			header["번호판Y"]		= mCarNoY.ToString();
			header["번호판W"]		= mCarNoW.ToString();
			header["번호판H"]		= mCarNoH.ToString();
			header["번호판유무"]	= mCarNoKind;
			header["위반코드"]		= mCrackCode;
			header["위반종류"]		= mCrackKind;
			header["정지선통과"]	= mStopLine;
			header["교차로통과"]	= mCrossCut;
			header["프레임수"]		= mFrameNum;
			header["신호등색"]		= mSinhoColor;
			header["차량진행"]		= mMoveKind;
			header["신호등종류"]	= mSinhoKind;

			Console.WriteLine(header.ToString());

			return	header;
		}

		public	bool	WriteToFile(string path) {
			FileStream		file	= File.Open(path, FileMode.CreateNew);
			BinaryWriter	bw		= new BinaryWriter(file);

			byte[]	head = new byte[113];

			MakeHead(head);
			bw.Write(head);

			byte[]	main_image		= ImageToByteArray(mMainImage);
			byte[]	main_length		= Encoding.Default.GetBytes(main_image.Length.ToString("D7"));
			bw.Write(main_length);
			bw.Write(main_image);

			byte[]	car_no_image	= ImageToByteArray(mCarNoImage);
			byte[]	car_no_length	= Encoding.Default.GetBytes(car_no_image.Length.ToString("D7"));
			bw.Write(car_no_length);
			bw.Write(car_no_image);

			for(int i = 0; i < 8; i++) {
				byte[]	sub_image	= ImageToByteArray(mSubImages[i]);
				byte[]	sub_length	= Encoding.Default.GetBytes(sub_image.Length.ToString("D7"));
				bw.Write(sub_length);
				bw.Write(sub_image);
			}

			file.Close();

			return	true;
		}

		public byte[] ImageToByteArray(Image imageIn)
		{
		   using (var ms = new MemoryStream())
		   {
			  imageIn.Save(ms,imageIn.RawFormat);
			  return  ms.ToArray();
		   }
		}

		public	bool	ReadFromFile(string path) {
			try {
				FileStream	file	= File.Open(path, FileMode.Open);
				using(BinaryReader br = new BinaryReader(file)) {
					byte[]	head = new byte[113];
					br.Read(head, 0, 113);
					//Console.WriteLine(HexDump.Dump(head));

					ParseHead(head);

					byte[]	length = new byte[7];
					br.Read(length, 0, length.Length);

					int	len;

					Int32.TryParse(Encoding.Default.GetString(length), out len);
					if (len < 10000)	return	false;
					byte[]	mainImage	= new byte[len];
					br.Read(mainImage, 0, mainImage.Length);
					mMainImage		= ImageUtil.GetImage(mainImage);

					br.Read(length, 0, length.Length);
				
					Int32.TryParse(Encoding.Default.GetString(length), out len);
					//Console.WriteLine(HexDump.Dump(length));

					byte[]	carNoImage	= new byte[len];
					br.Read(carNoImage, 0, carNoImage.Length);
					mCarNoImage			= ImageUtil.GetImage(carNoImage);

					for(int i = 0; i < 8; i++) {
						br.Read(length, 0, length.Length);
				
						Int32.TryParse(Encoding.Default.GetString(length), out len);
						//Console.WriteLine(HexDump.Dump(length));

						if (len < 10000)	return	false;
				
						byte[]	subImage	= new byte[len];
						br.Read(subImage, 0, subImage.Length);

						mSubImages[i]		= ImageUtil.GetImage(subImage);
					}
					return	true;
				}
			} catch(Exception e) {
				Console.WriteLine(e.Message);
			}
			return	false;
			/*
			FileStream	file	= File.Open(path, FileMode.Open);
			using(BinaryReader br = new BinaryReader(file)) {
				byte[]	head = new byte[113];
				br.Read(head, 0, 113);
				Console.WriteLine(HexDump.Dump(head));

				ParseHead(head);

				byte[]	length = new byte[7];
				br.Read(length, 0, length.Length);

				int	len;

				Int32.TryParse(Encoding.Default.GetString(length), out len);
				if (len < 10000)	return	false;
				byte[]	mainImage	= new byte[len];
				br.Read(mainImage, 0, mainImage.Length);
				mMainImage		= ImageUtil.GetImage(mainImage);

				br.Read(length, 0, length.Length);
				
				Int32.TryParse(Encoding.Default.GetString(length), out len);
				//Console.WriteLine(HexDump.Dump(length));

				byte[]	carNoImage	= new byte[len];
				br.Read(carNoImage, 0, carNoImage.Length);
				mCarNoImage			= ImageUtil.GetImage(carNoImage);

				for(int i = 0; i < 8; i++) {
					br.Read(length, 0, length.Length);
				
					Int32.TryParse(Encoding.Default.GetString(length), out len);
					//Console.WriteLine(HexDump.Dump(length));

					if (len < 10000)	return	false;
				
					byte[]	subImage	= new byte[len];
					br.Read(subImage, 0, subImage.Length);

					mSubImages[i]		= ImageUtil.GetImage(subImage);
				}
				return	true;
			}
			*/
		}

		public	bool	CrackValid() {
			if (mCCUInfo == null)		return	false;
			if (mOverSpeed > 0)			mIsOver	= true;

			return	mIsOver;
		}

		public	string	ToStringSimple() {
			return	string.Format("{0}:{1} {2} {3}차선 over:{4}km", mCCUNo, mKubun, dateUtil.ToStringOnlyTime(mDateTime), mRoadNum, mOverSpeed);
		}
	}
}
