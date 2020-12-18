using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.Adapter
{
    public  class   ConnInfo
    {
        // 
        public  string mDeviceID = "";
        public  string mCryptoKey = "";

        public  TcpClient    mEventHandle = null;
        public  StreamReader mEventReader = null;
        public  StreamWriter mEventWriter = null;

        public  DateTime    mLastTime = DateTime.Now;

        public bool ValidAlive(DateTime time, int alive_time)
        {
            int term = time.CompareTo(mLastTime.AddMilliseconds(alive_time));
            Console.WriteLine($"{term}, {alive_time}");
            if (term < 0) return false;
            return true;
        }

        public void Touch()
        {
            mLastTime = DateTime.Now;
        }

        public void Close()
        {
            // 전체를 다 close한다.
            if (mEventReader != null)
            {
                mEventReader.Close();
                mEventReader = null;
            }

            if (mEventWriter != null)
            {
                mEventWriter.Close();
                mEventWriter = null;
            }
            if (mEventHandle != null)
            {
                mEventHandle.Close();
                mEventHandle = null;
            }

        }
    }
}
