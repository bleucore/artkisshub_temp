using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtAPI.network.payload.apps
{
	public	class	StateInfo : PayloadBase
	{
		public	Protocol	mCurReq;
		public	Protocol	mCurRes;

		Dictionary<string, string> fields	= new Dictionary<string, string>() {
			{"tb_state_info_road"		, "단속차로"},
			{"tb_state_info_max"		, "제한속도"},
			{"tb_state_info_cut"		, "단속속도"},
			{"tb_state_info_condition"	, "연결상태"},
			{"tb_state_info_date"		, "최종단속일시"},
			{"tb_state_info_car_no"		, "최종단속 차량번호"},
			{"tb_state_info_sinho"		, "현재신호등상태"},
			{"tb_state_info_cam"		, "카메라상태"},
			{"tb_state_info_lens"		, "렌즈상태"}
		};

		public	void	SetResponse4Test(Protocol res) {
			res.AddPayload(fields["tb_state_info_road"]		, "2");
			res.AddPayload(fields["tb_state_info_max"]		, "80");
			res.AddPayload(fields["tb_state_info_cut"]		, "90");
			res.AddPayload(fields["tb_state_info_condition"], "최상");
			res.AddPayload(fields["tb_state_info_date"]		, "2020-06-14");
			res.AddPayload(fields["tb_state_info_car_no"]	, "254로0101");
			res.AddPayload(fields["tb_state_info_sinho"]	, "파랑");
			res.AddPayload(fields["tb_state_info_cam"]		, "중");
			res.AddPayload(fields["tb_state_info_lens"]		, "중");
		}

		public	void	ReqGetInfo(Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "get");
			req.AddPayload("kind"	, "info");

			mCurReq		= req;
		}

		public	void	ReqSetInfo(Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "set");
			req.AddPayload("kind"	, "info");

			mCurReq		= req;
		}

		public	bool	SetControl(Protocol res, Control control) {
			foreach (var field in fields) {
				try {
					SetValue(control, field.Key, res.GetValuePayload(field.Value).ToString());
				} catch(Exception e) {
					Console.WriteLine("SetControl error => key :{0}, {1} is null", field.Key, field.Value);
				}
			}
			return	true;
		}

		public	bool	GetValue(Protocol protocol, Control control) {
			GetValue(control, fields);
			foreach (var field in fields) {
				protocol.AddPayload(field.Value, util.Get(tuples, field.Value).ToString());
			}
			return	true;
		}
	}
}
