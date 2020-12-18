using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtAPI.network.payload.apps
{
	public	class	CamCtrl : PayloadBase
	{
		public	Protocol	mCurReq;
		public	Protocol	mCurRes;

		public	Dictionary<string, string> fields	= new Dictionary<string, string>() {
			{"tb_cam_ctrl_In_out"		, "In-Out 값"},
			{"tb_cam_ctrl_near_far"		, "Near-Far 값"},
			{"tb_cam_ctrl_open_close"	, "Open-Close 값"},
			{"tb_cam_ctrl_ptz"			, "P/T term"},
			{"tb_cam_ctrl_lens"			, "Lens term"},
			{"tb_cam_ctrl_time"			, "조명 On 시간"},
			{"cb_cam_ctrl_light"		, "Light On/Off"},
			{"tb_cam_ctrl_night"		, "주/야 전환값"},
			{"cb_cam_ctrl_install"		, "설치(가감)차로"},
			{"cb_cam_ctrl_auto"			, "AUTO_Responds"},

			{"rb_cam_ctrl_lane_1"		, "1개 설치차로"},
			{"tb_cam_ctrl_pan_1_1"		, "Pan 1차로 1차선"},
			{"tb_cam_ctrl_pan_1_2"		, "Pan 1차로 2차선"},
			{"tb_cam_ctrl_pan_1_3"		, "Pan 1차로 3차선"},
			{"tb_cam_ctrl_pan_1_4"		, "Pan 1차로 4차선"},
			{"tb_cam_ctrl_tilt_1_1"		, "Tilt 1차로 1차선"},
			{"tb_cam_ctrl_tilt_1_2"		, "Tilt 1차로 2차선"},
			{"tb_cam_ctrl_tilt_1_3"		, "Tilt 1차로 3차선"},
			{"tb_cam_ctrl_tilt_1_4"		, "Tilt 1차로 4차선"},
			{"tb_cam_ctrl_zoom_1_1"		, "Zoom 1차로 1차선"},
			{"tb_cam_ctrl_zoom_1_2"		, "Zoom 1차로 2차선"},
			{"tb_cam_ctrl_zoom_1_3"		, "Zoom 1차로 3차선"},
			{"tb_cam_ctrl_zoom_1_4"		, "Zoom 1차로 4차선"},
			{"tb_cam_ctrl_day_1_1"		, "Focus(day) 1차로 1차선"},
			{"tb_cam_ctrl_day_1_2"		, "Focus(day) 1차로 2차선"},
			{"tb_cam_ctrl_day_1_3"		, "Focus(day) 1차로 3차선"},
			{"tb_cam_ctrl_day_1_4"		, "Focus(day) 1차로 4차선"},
			{"tb_cam_ctrl_night_1_1"	, "Focus(night) 1차로 1차선"},
			{"tb_cam_ctrl_night_1_2"	, "Focus(night) 1차로 2차선"},
			{"tb_cam_ctrl_night_1_3"	, "Focus(night) 1차로 3차선"},
			{"tb_cam_ctrl_night_1_4"	, "Focus(night) 1차로 4차선"},

			{"rb_cam_ctrl_lane_2"		, "2개 설치차로"},
			{"tb_cam_ctrl_pan_2_1"		, "Pan 2차로 1,2차선"},
			{"tb_cam_ctrl_pan_2_2"		, "Pan 2차로 2,3차선"},
			{"tb_cam_ctrl_pan_2_3"		, "Pan 2차로 3차선"},
			{"tb_cam_ctrl_tilt_2_1"		, "Tilt 2차로 1,2차선"},
			{"tb_cam_ctrl_tilt_2_2"		, "Tilt 2차로 2,3차선"},
			{"tb_cam_ctrl_tilt_2_3"		, "Tilt 2차로 3,4차선"},
			{"tb_cam_ctrl_zoom_2_1"		, "Zoom 2차로 1,2차선"},
			{"tb_cam_ctrl_zoom_2_2"		, "Zoom 2차로 2,3차선"},
			{"tb_cam_ctrl_zoom_2_3"		, "Zoom 2차로 3,4차선"},
			{"tb_cam_ctrl_day_2_1"		, "Focus(day) 2차로 1,2차선"},
			{"tb_cam_ctrl_day_2_2"		, "Focus(day) 2차로 2,3차선"},
			{"tb_cam_ctrl_day_2_3"		, "Focus(day) 2차로 3,4차선"},
			{"tb_cam_ctrl_night_2_1"	, "Focus(night) 2차로 1,2차선"},
			{"tb_cam_ctrl_night_2_2"	, "Focus(night) 2차로 2,3차선"},
			{"tb_cam_ctrl_night_2_3"	, "Focus(night) 2차로 3,4차선"},

			{"rb_cam_ctrl_lane_3"		, "3개 설치차로"},
			{"tb_cam_ctrl_pan_3_1"		, "Pan 3차로 1,2,3차선"},
			{"tb_cam_ctrl_pan_3_2"		, "Pan 3차로 2,3,4차선"},
			{"tb_cam_ctrl_tilt_3_1"		, "Tilt 3차로 1,2,3차선"},
			{"tb_cam_ctrl_tilt_3_2"		, "Tilt 3차로 2,3,4차선"},
			{"tb_cam_ctrl_zoom_3_1"		, "Zoom 3차로 1,2,3차선"},
			{"tb_cam_ctrl_zoom_3_2"		, "Zoom 3차로 2,3,4차선"},
			{"tb_cam_ctrl_day_3_1"		, "Focus(day) 3차로 1,2,3차선"},
			{"tb_cam_ctrl_day_3_2"		, "Focus(day) 3차로 2,3,4차선"},
			{"tb_cam_ctrl_night_3_1"	, "Focus(night) 3차로 1,2,3차선"},
			{"tb_cam_ctrl_night_3_2"	, "Focus(night) 3차로 2,3,4차선"},
		};

		public	void	SetResponse4Test(Protocol res) {
			res.AddPayload(fields["tb_cam_ctrl_In_out"]		, "111");
			res.AddPayload(fields["tb_cam_ctrl_near_far"]	, "222");
			res.AddPayload(fields["tb_cam_ctrl_open_close"]	, "333");
			res.AddPayload(fields["tb_cam_ctrl_ptz"]		, "444");
			res.AddPayload(fields["tb_cam_ctrl_lens"]		, "555");
			res.AddPayload(fields["tb_cam_ctrl_time"]		, "666");
			res.AddPayload(fields["cb_cam_ctrl_light"]		, "true");
			res.AddPayload(fields["tb_cam_ctrl_night"]		, "888");
			res.AddPayload(fields["cb_cam_ctrl_install"]	, "2");
			res.AddPayload(fields["cb_cam_ctrl_auto"]		, "true");

			res.AddPayload(fields["rb_cam_ctrl_lane_1"]		, "2 차선");
			res.AddPayload(fields["tb_cam_ctrl_pan_1_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_pan_1_2"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_pan_1_3"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_pan_1_4"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_tilt_1_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_tilt_1_2"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_tilt_1_3"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_tilt_1_4"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_zoom_1_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_zoom_1_2"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_zoom_1_3"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_zoom_1_4"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_day_1_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_day_1_2"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_day_1_3"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_day_1_4"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_night_1_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_night_1_2"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_night_1_3"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_night_1_4"]	, "100");

			res.AddPayload(fields["rb_cam_ctrl_lane_2"]		, "1,2 차선");
			res.AddPayload(fields["tb_cam_ctrl_pan_2_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_pan_2_2"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_pan_2_3"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_tilt_2_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_tilt_2_2"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_tilt_2_3"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_zoom_2_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_zoom_2_2"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_zoom_2_3"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_day_2_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_day_2_2"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_day_2_3"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_night_2_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_night_2_2"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_night_2_3"]	, "100");

			res.AddPayload(fields["rb_cam_ctrl_lane_3"]		, "2,3,4 차선");
			res.AddPayload(fields["tb_cam_ctrl_pan_3_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_pan_3_2"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_tilt_3_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_tilt_3_2"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_zoom_3_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_zoom_3_2"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_day_3_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_day_3_2"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_night_3_1"]	, "100");
			res.AddPayload(fields["tb_cam_ctrl_night_3_2"]	, "100");
		}

		public	void	ReqGetInfo(Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "get");
			req.AddPayload("kind"	, "cam_control");

			mCurReq		= req;
		}

		public	void	ReqSetInfo(Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "set");
			req.AddPayload("kind"	, "cam_control");

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
