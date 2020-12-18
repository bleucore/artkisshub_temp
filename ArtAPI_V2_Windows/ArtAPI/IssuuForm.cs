

using iTextSharp.text.pdf;
using Leadtools;
using Leadtools.Codecs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtAPI
{
    public partial class IssuuForm : Form
    {
        public IssuuForm()
        {
            InitializeComponent();

            string pdfFile = @"C:\\FireFly\\Report\\20200720\\F1593_00000001_단속고지서.pdf";
            string imageFile = @"C:\\FireFly\\Report\\20200720\\F1593_00000001_단속고지서.jpg";

            using (RasterCodecs _codecs = new RasterCodecs())
            {
                using (RasterImage _image = _codecs.Load(pdfFile))
                {
                    _codecs.Save(_image, imageFile, RasterImageFormat.Jpeg, 24);
                }
            }
        }
    }
}
