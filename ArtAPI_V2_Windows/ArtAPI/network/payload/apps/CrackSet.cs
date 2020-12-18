using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtAPI.network.payload.apps
{
	public	class	CrackSet : PayloadBase
	{
		public	Protocol	mCurReq;
		public	Protocol	mCurRes;

		Dictionary<string, string> fields	= new Dictionary<string, string>() {
			{"tb_crack_set_cut_speed_1"		, "승용단속속도"},
			{"tb_crack_set_cut_speed_2"		, "화물단속속도"},
			{"tb_crack_set_max_speed_1"		, "승용제한속도"},
			{"tb_crack_set_max_speed_2"		, "화물제한속도"},
			{"rb_crack_set_event_1"			, "트리거발생조건1(위반구분)"},
			{"rb_crack_set_event_2"			, "트리거발생조건2(시간대)"},
			{"cb_crack_set_event_3_1"		, "속도위반단속"},
			{"cb_crack_set_event_3_2"		, "버스전용단속"},
			{"cb_crack_set_event_3_3"		, "신호위반단속"},
		};

		public	void	SetResponse4Test(Protocol res) {
			res.AddPayload(fields["tb_crack_set_max_speed_1"]		, "60");
			res.AddPayload(fields["tb_crack_set_max_speed_2"]		, "70");
			res.AddPayload(fields["tb_crack_set_cut_speed_1"]		, "75");
			res.AddPayload(fields["tb_crack_set_cut_speed_2"]		, "85");
			res.AddPayload(fields["rb_crack_set_event_1"]			, "위반차량");
			res.AddPayload(fields["rb_crack_set_event_2"]			, "종일");
			res.AddPayload(fields["cb_crack_set_event_3_1"]			, "true");
			res.AddPayload(fields["cb_crack_set_event_3_2"]			, "false");
			res.AddPayload(fields["cb_crack_set_event_3_3"]			, "true");
		}

		public	void	ReqGetInfo(Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "get");
			req.AddPayload("kind"	, "crack_setting");

			mCurReq		= req;
		}

		public	void	ReqSetInfo(Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "set");
			req.AddPayload("kind"	, "crack_setting");

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
