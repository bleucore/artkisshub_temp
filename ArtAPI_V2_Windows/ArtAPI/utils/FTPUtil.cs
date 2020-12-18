using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.utils
{
    public  class   FTPUtil
    {
        //public  string  ftpServerIP = "112.152.56.163";     //insert your FTP Server IP ;
        //public  string  ftpUserID   = "administrator";      // insert your ID;
        //public  string  ftpPassword = "sena12#$";           // insert your password;

        public  string  ftpPath     = "/ArtKiss/DocuSign";
        public  string  ftpIP       = "112.152.56.163";
        public  string  ftpID       = "administrator";
        public  string  ftpPW       = "sena12#$";
        public  bool    usePassive  = false;

        public  FTPUtil(string ip, string id, string pw)
        {
            ftpIP   = ip;
            ftpID   = id;
            ftpPW   = pw;
        }

        public  FTPUtil() { }

        public  void    Set(string ip, string id, string pw)
        {
            ftpIP = ip;
            ftpID = id;
            ftpPW = pw;
        }

        public int     GetFile(string file)
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://www.contoso.com/test.htm");
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            Console.WriteLine(reader.ReadToEnd());

            Console.WriteLine($"Download Complete, status {response.StatusDescription}");

            reader.Close();
            response.Close();

            return  0;
        }

        public  string[] GetFileList(string ftpFolder, string filter)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;

            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpIP + "/" + ftpFolder));
                reqFTP.UseBinary    = true;
                reqFTP.UsePassive   = usePassive;
                reqFTP.Credentials  = new NetworkCredential(ftpID, ftpPW);
                reqFTP.Method       = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (!string.IsNullOrEmpty(line) && line.Contains(filter)) { 
                        result.Append(line);
                        result.Append("\n");
                    }
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();

                return result.ToString().Split('\n');
            }
            catch
            {
                downloadFiles = null;
                return downloadFiles;
            }
        }

    }
}
