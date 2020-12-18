using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.utils
{
    public class ImageUtil
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Image GetImage(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            stream.Position = 0;
            return (Image.FromStream(stream));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public  Image   DownloadImage(string imageUrl)
        {
            Image image = null;

            try
            {
                System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(imageUrl);
                webRequest.AllowWriteStreamBuffering = true;
                webRequest.Timeout = 30000;
                webRequest.ServicePoint.ConnectionLeaseTimeout = 5000;
                webRequest.ServicePoint.MaxIdleTime = 5000;

                using (System.Net.WebResponse webResponse = webRequest.GetResponse())
                {

                    using (System.IO.Stream stream = webResponse.GetResponseStream())
                    {
                        image = System.Drawing.Image.FromStream(stream);
                    }
                }

                webRequest.ServicePoint.CloseConnectionGroup(webRequest.ConnectionGroupName);
                webRequest = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }

            return image;
        }

        public  Bitmap  LoadPicture(string url)
        {
            System.Net.HttpWebRequest wreq;
            System.Net.HttpWebResponse wresp;
            Stream mystream;
            Bitmap bmp;

            bmp = null;
            mystream = null;
            wresp = null;
            try {
                wreq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
                wreq.AllowWriteStreamBuffering = true;

                wresp = (System.Net.HttpWebResponse)wreq.GetResponse();

                if ((mystream = wresp.GetResponseStream()) != null)
                    bmp = new Bitmap(mystream);
            } catch {
                // Do nothing... 
            } finally {
                if (mystream != null)
                    mystream.Close();

                if (wresp != null)
                    wresp.Close();
            }

            return (bmp);
        }

        public  byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        public  Image ResizeImage(Image image, int new_height, int new_width)
        {
            Bitmap new_image = new Bitmap(new_width, new_height);
            Graphics g = Graphics.FromImage((Image)new_image);
            g.InterpolationMode = InterpolationMode.High;
            g.DrawImage(image, 0, 0, new_width, new_height);
            return new_image;
        }

        public  Image OverWriteShape(Image Shape, Image background, Size Pos)
        {
            Image img = new Bitmap(background);
            PointF ImagePos = new PointF(Pos.Width - (Shape.Width / Shape.HorizontalResolution * img.HorizontalResolution) / 2, Pos.Height - (Shape.Height / Shape.VerticalResolution * img.VerticalResolution) / 2);

            Graphics formGraphics = Graphics.FromImage(img);

            formGraphics.DrawImage(Shape, ImagePos);

            formGraphics.Save();
            formGraphics.Dispose();
            return img;
        }
    }
}
