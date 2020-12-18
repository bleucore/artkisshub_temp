using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtAPI.network.payload.apps
{
	public	class	StateNet : PayloadBase
	{
		public	Protocol	mCurReq;
		public	Protocol	mCurRes;

		Dictionary<string, string> fields	= new Dictionary<string, string>() {
			{"tb_state_net_ccu_no"	, "제어기번호"},
			{"tb_state_net_site"	, "설치장소"},
			{"tb_state_net_addr"	, "통신 IP"},
			{"tb_state_net_port"	, "통신 port"},
			{"tb_state_net_sinho"	, "신호"},
			{"tb_state_net_speed"	, "최근속도"}
		};

		public	void	SetResponse4Test(Protocol res) {
			res.AddPayload("제어기번호"	, "F1234");
			res.AddPayload("설치장소"	, "Test 장소");
			res.AddPayload("통신 IP"	, "192.168.123.123");
			res.AddPayload("통신 port"	, "9001");
			res.AddPayload("신호"		, "파랑");
			res.AddPayload("최근속도"	, "92");
		}

		public	void	ReqGetInfo(Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "get");
			req.AddPayload("kind"	, "network");

			mCurReq		= req;
		}

		public	void	ReqSetInfo(Protocol req) {
			req.AddPayload("cmd"	, "state");
			req.AddPayload("mode"	, "set");
			req.AddPayload("kind"	, "network");

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
