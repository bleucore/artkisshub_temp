using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtAPI.network.payload.apps
{
	public	class	CrackRoad : PayloadBase
	{
		public	Protocol	mCurReq;
		public	Protocol	mCurRes;

		public	Dictionary<string, string> fields	= new Dictionary<string, string>() {
			{"cb_crack_road_cut_1"		, "1차선 단속"},
			{"cb_crack_road_cut_2"		, "2차선 단속"},
			{"cb_crack_road_cut_3"		, "3차선 단속"},
			{"cb_crack_road_cut_4"		, "4차선 단속"},
			{"cb_crack_road_kind_1"		, "1차선 종류"},
			{"cb_crack_road_kind_2"		, "2차선 종류"},
			{"cb_crack_road_kind_3"		, "3차선 종류"},
			{"cb_crack_road_kind_4"		, "4차선 종류"},
			{"cb_crack_road_speed_1"	, "1차선 속도"},
			{"cb_crack_road_speed_2"	, "2차선 속도"},
			{"cb_crack_road_speed_3"	, "3차선 속도"},
			{"cb_crack_road_speed_4"	, "4차선 속도"},
			{"cb_crack_road_bus_1"		, "1차선 버스"},
			{"cb_crack_road_bus_2"		, "2차선 버스"},
			{"cb_crack_road_bus_3"		, "3차선 버스"},
			{"cb_crack_road_bus_4"		, "4차선 버스"},
			{"cb_crack_road_sinho_1"	, "1차선 신호"},
			{"cb_crack_road_sinho_2"	, "2차선 신호"},
			{"cb_crack_road_sinho_3"	, "3차선 신호"},
			{"cb_crack_road_sinho_4"	, "4차선 신호"},
			{"cb_crack_road_sway_1"		, "1차선 갓길"},
			{"cb_crack_road_sway_2"		, "2차선 갓길"},
			{"cb_crack_road_sway_3"		, "3차선 갓길"},
			{"cb_crack_road_sway_4"		, "4차선 갓길"},
			{"cb_crack_list_line_1"		, "1차선 라인"},
			{"cb_crack_list_line_2"		, "2차선 라인"},
			{"cb_crack_list_line_3"		, "3차선 라인"},
			{"cb_crack_list_line_4"		, "4차선 라인"}
		};

		public	void	SetResponse4Test(Protocol res) {
			res.AddPayload(fields["cb_crack_road_cut_1"]	, "true");
			res.AddPayload(fields["cb_crack_road_cut_2"]	, "true");
			res.AddPayload(fields["cb_crack_road_cut_3"]	, "true");
			res.AddPayload(fields["cb_crack_road_cut_4"]	, "true");
			res.AddPayload(fields["cb_crack_road_kind_1"]	, "직진");
			res.AddPayload(fields["cb_crack_road_kind_2"]	, "좌회전");
			res.AddPayload(fields["cb_crack_road_kind_3"]	, "직/좌");
			res.AddPayload(fields["cb_crack_road_kind_4"]	, "사용안함");
			res.AddPayload(fields["cb_crack_road_speed_1"]	, "true");
			res.AddPayload(fields["cb_crack_road_speed_2"]	, "true");
			res.AddPayload(fields["cb_crack_road_speed_3"]	, "true");
			res.AddPayload(fields["cb_crack_road_speed_4"]	, "true");
			res.AddPayload(fields["cb_crack_road_bus_1"]	, "true");
			res.AddPayload(fields["cb_crack_road_bus_2"]	, "false");
			res.AddPayload(fields["cb_crack_road_bus_3"]	, "true");
			res.AddPayload(fields["cb_crack_road_bus_4"]	, "true");
			res.AddPayload(fields["cb_crack_road_sinho_1"]	, "true");
			res.AddPayload(fields["cb_crack_road_sinho_2"]	, "true");
			res.AddPayload(fields["cb_crack_road_sinho_3"]	, "true");
			res.AddPayload(fields["cb_crack_road_sinho_4"]	, "true");
			res.AddPayload(fields["cb_crack_road_sway_1"]	, "true");
			res.AddPayload(fields["cb_crack_road_sway_2"]	, "true");
			res.AddPayload(fields["cb_crack_road_sway_3"]	, "true");
			res.AddPayload(fields["cb_crack_road_sway_4"]	, "true");
			res.AddPayload(fields["cb_crack_list_line_1"]	, "#1");
			res.AddPayload(fields["cb_crack_list_line_2"]	, "#2");
			res.AddPayload(fields["cb_crack_list_line_3"]	, "#3");
			res.AddPayload(fields["cb_crack_list_line_4"]	, "#4");
		}

		public	void	ReqGetInfo(Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "get");
			req.AddPayload("kind"	, "crack_road");

			mCurReq		= req;
		}

		public	void	ReqSetInfo(Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "set");
			req.AddPayload("kind"	, "crack_road");

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
