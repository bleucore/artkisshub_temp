using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArtAPI.network.payload.apps
{
	public	class	CamSinho : PayloadBase
	{
		public	Protocol	mCurReq;
		public	Protocol	mCurRes;

		public	Dictionary<string, string> fields	= new Dictionary<string, string>() {
			{"tb_cam_sinho_light_1"		, "점등시간(적)"},
			{"tb_cam_sinho_light_2"		, "점등시간(황)"},
			{"tb_cam_sinho_light_3"		, "점등시간(좌)"},
			{"tb_cam_sinho_light_4"		, "점등시간(직)"},
			{"tb_cam_sinho_detect_1"	, "검지값(적)"},
			{"tb_cam_sinho_detect_2"	, "검지값(황)"},
			{"tb_cam_sinho_detect_3"	, "검지값(좌)"},
			{"tb_cam_sinho_detect_4"	, "검지값(직)"},
			{"tb_cam_sinho_level_1"		, "레벨(적)"},
			{"tb_cam_sinho_level_2"		, "레벨(황)"},
			{"tb_cam_sinho_level_3"		, "레벨(좌)"},
			{"tb_cam_sinho_level_4"		, "레벨(직)"},
			{"cb_cam_sinho_value"		, "값요청"},
			{"cb_cam_sinho_type_1"		, "신호 형태(적색)"},
			{"cb_cam_sinho_type_2"		, "신호 형태(황색)"},
			{"cb_cam_sinho_type_3"		, "신호 형태(좌회전)"},
			{"cb_cam_sinho_type_4"		, "신호 형태(녹색)"}
		};

		public	void	SetResponse4Test(Protocol res) {
			res.AddPayload(fields["tb_cam_sinho_light_1"]	, "100");
			res.AddPayload(fields["tb_cam_sinho_light_2"]	, "200");
			res.AddPayload(fields["tb_cam_sinho_light_3"]	, "300");
			res.AddPayload(fields["tb_cam_sinho_light_4"]	, "400");
			res.AddPayload(fields["tb_cam_sinho_detect_1"]	, "50");
			res.AddPayload(fields["tb_cam_sinho_detect_2"]	, "60");
			res.AddPayload(fields["tb_cam_sinho_detect_3"]	, "70");
			res.AddPayload(fields["tb_cam_sinho_detect_4"]	, "80");
			res.AddPayload(fields["tb_cam_sinho_level_1"]	, "110");
			res.AddPayload(fields["tb_cam_sinho_level_2"]	, "220");
			res.AddPayload(fields["tb_cam_sinho_level_3"]	, "330");
			res.AddPayload(fields["tb_cam_sinho_level_4"]	, "440");
			res.AddPayload(fields["cb_cam_sinho_value"]		, "true");
			res.AddPayload(fields["cb_cam_sinho_type_1"]	, "true");
			res.AddPayload(fields["cb_cam_sinho_type_2"]	, "true");
			res.AddPayload(fields["cb_cam_sinho_type_3"]	, "false");
			res.AddPayload(fields["cb_cam_sinho_type_4"]	, "true");
		}

		public	void	ReqGetInfo(Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "get");
			req.AddPayload("kind"	, "cam_sinho");

			mCurReq		= req;
		}

		public	void	ReqSetInfo(Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "set");
			req.AddPayload("kind"	, "cam_sinho");

			mCurReq		= req;
		}

		public	bool	SetControl(Protocol res, Control control) {
			// RadioButton은 따로 관리한다...
			//{"rb_crack_set_event_1"			, "트리거발생조건1(위반구분)"},
			//{"rb_crack_set_event_2"			, "트리거발생조건2(시간대)"},

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
				try {
					protocol.AddPayload(field.Value, util.Get(tuples, field.Value).ToString());
				} catch(Exception e) {
					Console.WriteLine("SetControl error => key :{0}, {1} is null", field.Key, field.Value);
				}
			}
			return	true;
		}
	}
}
