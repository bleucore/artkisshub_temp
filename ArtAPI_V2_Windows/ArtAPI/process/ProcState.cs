using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtAPI.info;
using ArtAPI.network;
using ArtAPI.network.payload;
using ArtAPI.network.payload.apps;

namespace ArtAPI.process {
	public	class	ProcState {

		public	Protocol	Exec(Protocol req)
		{
			string	mode	= req.GetValuePayload("mode").ToString();
			switch(mode) {
			case "set":
				return	Set(req);
			case "get":
				return	Get(req);
			}

			return	null;
		}

		public Protocol	Set(Protocol req) {
			Protocol	res;
			string	kind	= req.GetValuePayload("kind").ToString();
			switch(kind) {
				case "cam_carno": {
					res	= new Protocol(Protocol.OP_RES);
					res.AddPayload("Name", "cam_carno");
					// print for sample data
					Console.WriteLine(req.GetValuePayload("Packet 최대크기"));
					res.SetOK();
					return	res;
				}
				case "cam_state": {
					string	target		= req.GetValuePayload("target").ToString();
					string	value		= req.GetValuePayload("value").ToString();
					string	direction	= req.GetValuePayload("direction").ToString();
					switch(target) {
						case "pan_tilt":
							Console.WriteLine("direction : {0}", direction);
							break;
						case "lens":
							Console.WriteLine("direction : {0}", direction);
							break;
					}
					res	= new Protocol(Protocol.OP_RES);
					res.AddPayload("Name", "cam_state");
					res.SetOK();
					return	res;
				}
				case "cam_control": {
					res	= new Protocol(Protocol.OP_RES);
					res.AddPayload("Name", "cam_control");
					// print for sample data
					Console.WriteLine(req.GetValuePayload("설치(가감)차로"));
					res.SetOK();
					return	res;
				}
				case "cam_sinho": {
					res	= new Protocol(Protocol.OP_RES);
					res.AddPayload("Name", "cam_sinho");
					// print for sample data
					Console.WriteLine(req.GetValuePayload("검지값(적)"));
					res.SetOK();
					return	res;
				}
				case "network": {
					res	= new Protocol(Protocol.OP_RES);
					res.AddPayload("Name", "network");
					res.SetOK();
					return	res;
				}
				case "info": {
					res	= new Protocol(Protocol.OP_RES);
					res.AddPayload("Name", "info");
					res.SetOK();
					return	res;
				}
				case "crack_setting": {
					res	= new Protocol(Protocol.OP_RES);
					res.AddPayload("Name", "crack_setting");
					// print for sample data
					Console.WriteLine(req.GetValuePayload("화물단속속도"));
					res.SetOK();
					return	res;
				}
				case "crack_road": {
					res	= new Protocol(Protocol.OP_RES);
					res.AddPayload("Name", "crack_road");
					Console.WriteLine(req.GetValuePayload("3차선 단속"));
					res.SetOK();
					return	res;
				}
				default:
					res	= new Protocol(Protocol.OP_NON);
					break;
					
			}
			res.SetError("0999", "kind is empty");
			return	res;
		}

		public	Protocol	Get(Protocol req) {
			string	kind	= req.GetValuePayload("kind").ToString();
			switch(kind) {
				case "cam_carno": {
					Protocol	res	= new Protocol(Protocol.OP_RES);

					// 보내는 이름
					res.AddPayload("Name", "cam_carno");

					CamCarNo	payload	= new CamCarNo();
					payload.SetResponse4Test(res);
					res.SetOK();

					return	res;
				}
				case "cam_control": {
					Protocol	res	= new Protocol(Protocol.OP_RES);

					// 보내는 이름
					res.AddPayload("Name", "cam_control");

					CamCtrl	payload	= new CamCtrl();
					payload.SetResponse4Test(res);
					res.SetOK();

					return	res;
				}
				case "cam_sinho": {
					Protocol	res	= new Protocol(Protocol.OP_RES);

					// 보내는 이름
					res.AddPayload("Name", "cam_sinho");

					CamSinho	payload	= new CamSinho();
					payload.SetResponse4Test(res);
					res.SetOK();

					return	res;
				}
				case "network": {
					Protocol	res	= new Protocol(Protocol.OP_RES);

					// 보내는 이름
					res.AddPayload("Name", "network");

					StateNet	payload	= new StateNet();
					payload.SetResponse4Test(res);
					res.SetOK();

					return	res;
				}
				case "info": {
					Protocol	res	= new Protocol(Protocol.OP_RES);

					// 보내는 이름
					res.AddPayload("Name", "info");

					StateInfo	payload	= new StateInfo();
					payload.SetResponse4Test(res);
					res.SetOK();

					return	res;
				}
				case "crack_setting": {
					Protocol	res	= new Protocol(Protocol.OP_RES);

					// 보내는 이름
					res.AddPayload("Name", "crack_setting");

					CrackSet	payload	= new CrackSet();
					payload.SetResponse4Test(res);
					res.SetOK();

					return	res;
				}
				case "crack_road": {
					Protocol	res	= new Protocol(Protocol.OP_RES);

					// 보내는 이름
					res.AddPayload("Name", "crack_setting");

					CrackRoad	payload	= new CrackRoad();
					payload.SetResponse4Test(res);
					res.SetOK();

					return	res;
				}
				case "ccu_state": {
					Protocol	res	= new Protocol(Protocol.OP_RES);

					// 보내는 이름
					res.AddPayload("Name", "ccu_state");

					CCUState info		= new CCUState();
					info.mCurSinho		= "파랑";
					info.mLastSinho		= "파랑";
					info.mLastSpeed		= 92;
					info.mRoadNum		= 2;
					info.mCutSpeed		= 90;
					info.mMaxSpeed		= 80;
					info.mLastCrackTime	= "12:34:56";
					info.mLastCrackCarNo= "112가1234";
					info.mConnState		= "최상";
					info.mCamState		= "흐림";
					info.mLensState		= "정상";

					DeviceState	payload	= new DeviceState();
					payload.SetResponse(info, res);
					res.SetOK();

					return	res;
				}
				/*
				case "ecp_state_part_main": {
					Protocol	res	= new Protocol(Protocol.OP_RES);

					// 보내는 이름
					res.AddPayload("Name", "ProcState");

					// 1. 정보가 더 있음을 표시
					//res.AddPayload("continue", "violation-image");

					res.AddPayload("ID"		, "11111");
					res.AddPayload("Site"	, DateUtil.ToStringOnlyDate());

					return	res;
				}
				case "위반정보_V2": { 
					Protocol res = new Protocol(Protocol.OP_IMG);

					// 이미지를 보낸다...
					byte[]	data	= File.ReadAllBytes(".\\차량이미지.jpg");
					res.SetImage(data);

					return res;
				}
				*/
			}

			Protocol	retv	= new Protocol(Protocol.OP_RES);
			retv.SetError("8832", "kind is not found at payload");
			return	retv;
		}
	}
}
