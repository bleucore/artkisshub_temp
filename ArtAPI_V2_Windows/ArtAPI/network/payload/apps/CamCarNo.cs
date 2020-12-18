using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ArtAPI.network.payload.apps
{
	public	class	CamCarNo : PayloadBase
	{
		public	Protocol	mCurReq;
		public	Protocol	mCurRes;

		public	Dictionary<string, string> fields	= new Dictionary<string, string>() {
			{"tb_cam_carno_left"		, "인식영역(좌)"},
			{"tb_cam_carno_right"		, "인식영역(우)"},
			{"tb_cam_set_top"			, "인식영역(상)"},
			{"tb_cam_set_bottom"		, "인식영역(하)"},
			{"tb_cam_carno_value"		, "기울기"},
			{"rb_cam_carno_angle"		, "번호판 기울기"},
			{"tb_cam_carno_speed"		, "shutter Speed"},
			{"tb_cam_carno_time"		, "Strobe Time"},
			{"tb_cam_carno_gain"		, "Video-Gain"},
			{"tb_cam_carno_match"		, "매칭대기 시간"},
			{"tb_cam_carno_size"		, "Packet 최대크기"},
			{"cb_cam_carno_log"			, "Log Display"},
			{"cb_cam_carno_bmp"			, "BMP 저장"},
			{"cb_cam_carno_mode"		, "운영모드"}
		};

		public	void	SetResponse4Test(Protocol res) {
			res.AddPayload(fields["tb_cam_carno_left"]		, "100");
			res.AddPayload(fields["tb_cam_carno_right"]		, "200");
			res.AddPayload(fields["tb_cam_set_top"]			, "300");
			res.AddPayload(fields["tb_cam_set_bottom"]		, "400");
			res.AddPayload(fields["tb_cam_carno_value"]		, "50");
			res.AddPayload(fields["rb_cam_carno_angle"]		, "2차트");
			res.AddPayload(fields["tb_cam_carno_speed"]		, "70");
			res.AddPayload(fields["tb_cam_carno_time"]		, "80");
			res.AddPayload(fields["tb_cam_carno_gain"]		, "110");
			res.AddPayload(fields["tb_cam_carno_match"]		, "220");
			res.AddPayload(fields["tb_cam_carno_size"]		, "330");
			res.AddPayload(fields["cb_cam_carno_log"]		, "true");
			res.AddPayload(fields["cb_cam_carno_bmp"]		, "false");
			res.AddPayload(fields["cb_cam_carno_mode"]		, "true");
		}

		public	void	ReqGetInfo(Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "get");
			req.AddPayload("kind"	, "cam_carno");

			mCurReq		= req;
		}

		public	void	ReqSetInfo(Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "set");
			req.AddPayload("kind"	, "cam_carno");

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
