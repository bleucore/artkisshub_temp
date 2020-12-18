using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAPI.info;
using ArtAPI.utils;

namespace ArtAPI.network.payload
{
	public	class	CrackDown
	{
		ImageUtil	imageUtil		= new ImageUtil();

		public	void	EventCrackInfo(Protocol req, CrackInfo info) {
			req.AddPayload("cmd"		, "crack");
			req.AddPayload("kind"		, "위반정보_이벤트");

			SetHead(req, info);
		}

		public	void	ReqCrackImage(Protocol req, string cont) {
			req.AddPayload("cmd"		, "crack");
			req.AddPayload("kind"		, cont);
		}

		public	void	ReqCrackEvent(Protocol req, string filename) {
			req.AddPayload("cmd"		, "event");
			req.AddPayload("kind"		, "위반정보_상세");
			req.AddPayload("filename"	, filename);
		}

		public	void	ReqCrackInfo(Protocol req, string filename) {
			req.AddPayload("cmd"		, "crack");
			req.AddPayload("kind"		, "위반정보_상세");
			req.AddPayload("filename"	, filename);
		}

		public	void	ReqCrackList(Protocol req, string date) {
			req.AddPayload("cmd"		, "crack");
			req.AddPayload("kind"		, "위반정보_리스트");
			req.AddPayload("date"		, date);
		}

		public	void	SetHead(Protocol request, CrackInfo info) {
			request.AddPayload("kubun"		, info.mKubun);
			request.AddPayload("ccu_no"		, info.CCUNo);
			request.AddPayload("위반속도"	, info.mOverSpeed.ToString());
			request.AddPayload("차종"		, info.mCarKind);
			request.AddPayload("위반차로"	, info.mRoadNum);
			request.AddPayload("위반날자"	, info.mDate);
			request.AddPayload("위반시간"	, info.mTime);
			if (string.IsNullOrEmpty(info.mMilliSec))		info.mMilliSec	= "0";
			request.AddPayload("MilliSec"	, info.mMilliSec);
			request.AddPayload("차량번호"	, info.mCarNO);
			request.AddPayload("번호판X"	, info.mCarNoX.ToString());
			request.AddPayload("번호판Y"	, info.mCarNoY.ToString());
			request.AddPayload("번호판W"	, info.mCarNoW.ToString());
			request.AddPayload("번호판H"	, info.mCarNoH.ToString());
			request.AddPayload("번호판유무"	, info.mCarNoKind);
			request.AddPayload("위반코드"	, info.mCrackCode);
			request.AddPayload("위반종류"	, info.mCrackKind);
			request.AddPayload("정지선통과"	, info.mStopLine);
			request.AddPayload("교차로통과"	, info.mCrossCut);
			request.AddPayload("프레임수"	, info.mFrameNum);
			request.AddPayload("신호등색"	, info.mSinhoColor);
			request.AddPayload("차량진행"	, info.mMoveKind);
			request.AddPayload("신호등종류"	, info.mSinhoKind);
			request.AddPayload("filename"	, info.mFileName);
		}

		public	void	SetInfoAll(Protocol req, CrackInfo info) {

			SetHead(req, info);

			var	total_stream	= new MemoryStream();

			// 이미지를 합쳐서 보낸다... 
			using (var ms = new MemoryStream())
			{
				byte[]	main_image		= imageUtil.ImageToByteArray(info.mMainImage);
				byte[]	main_length		= Encoding.Default.GetBytes(main_image.Length.ToString("D7"));
				total_stream.Write(main_length	, 0, main_length.Length);
				total_stream.Write(main_image	, 0, main_image.Length);

				byte[]	car_no_image	= imageUtil.ImageToByteArray(info.mCarNoImage);
				byte[]	car_no_length	= Encoding.Default.GetBytes(car_no_image.Length.ToString("D7"));
				total_stream.Write(car_no_length, 0, car_no_length.Length);
				total_stream.Write(car_no_image	, 0, car_no_image.Length);

				for(int i = 0; i < 8; i++) {
					byte[]	sub_image	= imageUtil.ImageToByteArray(info.mSubImages[i]);
					byte[]	sub_length	= Encoding.Default.GetBytes(sub_image.Length.ToString("D7"));
					total_stream.Write(sub_length	, 0, sub_length.Length);
					total_stream.Write(sub_image	, 0, sub_image.Length);
				}
			}
			req.SetImage(total_stream.ToArray());
		}

		public	CrackInfo	GetInfo(Protocol req) 
		{
			try {
				CrackInfo	info	= new CrackInfo();

				info.mKubun			= req.GetValuePayload("kubun").ToString();
				info.CCUNo			= req.GetValuePayload("ccu_no").ToString();
				Int32.TryParse(req.GetValuePayload("위반속도").ToString(), out info.mOverSpeed);
				info.mCarKind		= req.GetValuePayload("차종").ToString();
				info.mRoadNum		= req.GetValuePayload("위반차로").ToString();
				info.mDate			= req.GetValuePayload("위반날자").ToString();
				info.mTime			= req.GetValuePayload("위반시간").ToString();
				info.mMilliSec		= req.GetValuePayload("MilliSec").ToString();
				info.mCarNO			= req.GetValuePayload("차량번호").ToString();
				Int32.TryParse(req.GetValuePayload("번호판X").ToString(), out info.mCarNoX);
				Int32.TryParse(req.GetValuePayload("번호판Y").ToString(), out info.mCarNoY);
				Int32.TryParse(req.GetValuePayload("번호판W").ToString(), out info.mCarNoW);
				Int32.TryParse(req.GetValuePayload("번호판H").ToString(), out info.mCarNoH);
				info.mCarNoKind		= req.GetValuePayload("번호판유무").ToString();
				info.mCrackCode		= req.GetValuePayload("위반코드").ToString();
				info.mCrackKind		= req.GetValuePayload("위반종류").ToString();
				info.mStopLine		= req.GetValuePayload("정지선통과").ToString();
				info.mCrossCut		= req.GetValuePayload("교차로통과").ToString();
				info.mFrameNum		= req.GetValuePayload("프레임수").ToString();
				info.mSinhoColor	= req.GetValuePayload("신호등색").ToString();
				info.mMoveKind		= req.GetValuePayload("차량진행").ToString();
				info.mSinhoKind		= req.GetValuePayload("신호등종류").ToString();
				info.mFileName		= req.GetValuePayload("filename").ToString();

				// 이미지 parse
				byte[]	image	= req.GetImage();
				if (image != null) {
//					byte[]	length = new byte[7];
//					br.Read(length, 0, length.Length);

//					int	len;

//					Int32.TryParse(Encoding.Default.GetString(length), out len);
				}

				return	info;
			} catch(Exception e) {}

			return	null;
		}
	}
}
