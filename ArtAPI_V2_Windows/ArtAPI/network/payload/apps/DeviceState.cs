
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArtAPI.info;

namespace ArtAPI.network.payload
{
	public	class	DeviceState : PayloadBase
	{
		Dictionary<string, string> keys_main = new Dictionary<string, string>() {
			{"lb_state_main_id"		, "ID"},
			{"lb_state_main_site"	, "Site"},
			{"lb_state_ver_main"	, "메인버젼"}
		};

		Dictionary<string, string> keys_cam = new Dictionary<string, string>() {
			{"lb_state_main_id"     , "렌즈"},
			{"lb_state_main_site"   , "Site"},
			{"lb_state_ver_main"    , "메인버젼"}
		};

		public void	SetRequest(CCUInfo info, Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "get");
			req.AddPayload("kind"	, "ccu_state");
			req.AddPayload("ccu_id"	, info.mID);
		}

		public	void	SetResponse(info.CCUState info, Protocol res) {
			res.AddPayload("cmd"	, "state");
			res.AddPayload("kind"	, "ccu_state");

			res.AddPayload("조회시신호"			, info.mCurSinho);
			res.AddPayload("최근신호"			, info.mLastSinho);
			res.AddPayload("최근속도"			, info.mLastSpeed.ToString());
			res.AddPayload("단속차선"			, info.mRoadNum.ToString());
			res.AddPayload("단속속도"			, info.mCutSpeed.ToString());
			res.AddPayload("제한속도"			, info.mMaxSpeed.ToString());
			res.AddPayload("최종단속일시"		, info.mLastCrackTime);
			res.AddPayload("최종단속차량번호"	, info.mLastCrackCarNo);
			res.AddPayload("연결상태"			, info.mConnState);
			res.AddPayload("카메라상태"			, info.mCamState);
			res.AddPayload("렌즈상태"			, info.mLensState);
		}

		public	void	Build(Control control) {
			//Build(control, keys_main);
			//Build(control, keys_cam);
		}

		public	CCUState	Parse(Protocol req) {
			try {
				info.CCUState	info	= new info.CCUState();

				info.mCurSinho		= req.GetValuePayload("조회시신호").ToString();
				info.mLastSinho		= req.GetValuePayload("최근신호").ToString();
				Int32.TryParse(req.GetValuePayload("최근속도").ToString(), out info.mLastSpeed);
				Int32.TryParse(req.GetValuePayload("단속차선").ToString(), out info.mRoadNum);
				Int32.TryParse(req.GetValuePayload("단속속도").ToString(), out info.mCutSpeed);
				Int32.TryParse(req.GetValuePayload("제한속도").ToString(), out info.mMaxSpeed);
				info.mLastCrackTime	= req.GetValuePayload("최종단속일시").ToString();
				info.mLastCrackCarNo= req.GetValuePayload("최종단속차량번호").ToString();
				info.mConnState		= req.GetValuePayload("연결상태").ToString();
				info.mCamState		= req.GetValuePayload("카메라상태").ToString();
				info.mLensState		= req.GetValuePayload("렌즈상태").ToString();
				return	info;
			} catch(Exception e) {}

			return	null;
		}
	}
}
