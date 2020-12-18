using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.info
{
	public	class	CCUState
	{
		public	string		mCurSinho;			// 조회시신호

		public	string		mLastSinho;			// 최근신호
		public	int			mLastSpeed;			// 최근속도
		
		//public	string		mLineWidth;			// 단속차로폭
		public	int			mRoadNum;			// 단속차선
		public	int			mCutSpeed;			// 단속속도

		public	int			mMaxSpeed;			// 제한속도
		public	string		mLastCrackTime;		// 최종단속일시
		public	string		mLastCrackCarNo;	// 최종단속차량번호
		public	string		mConnState;			// 연결상태
		public	string		mCamState;			// 카메라상태
		public	string		mLensState;			// 렌즈상태
	}
}
