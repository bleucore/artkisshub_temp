using ArtAPI.info;
using ArtAPI.utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtAPI.Adapter
{
    public  class   IssuuPDF
    {
        private BaseFont    bf;

        private Font    fTitle;
        private Font    sTitle;

        private ImageUtil   imageUtil   = new ImageUtil();
        private DateUtil    dateUtil    = new DateUtil();

        public bool    Init()
        {
            //var font_factory = new XMLWorkerFontProvider(XMLWorkerFontProvider.DONTLOOKFORFONTS);
            //font.Register(Environment.CurrentDirectory + "/malgun.ttf", "MalgunGothic");
            //font_factory.Register(@"c:/windows/fonts/malgun.ttf", "MalgunGothic");
            //font	= font_factory.GetFont("MalgunGothic", BaseFont.IDENTITY_H, 10);

            //한글 폰트를 읽어온다.
            BaseFont bf = BaseFont.CreateFont(@"C:\Windows\Fonts\malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            fTitle = new Font(bf, 16, Font.BOLD, CMYKColor.BLACK);
            sTitle = new Font(bf, 14, Font.NORMAL, CMYKColor.BLACK);

            //Font font = new Font(bf, 12, Font.BOLD | Font.UNDERLINE, CMYKColor.BLACK);


            //fContents = new Font(bf, 10, Font.NORMAL, CMYKColor.BLACK);



            return true;
        }

        public  string  MakePDF(string artist_name, CataInfo catalog)
        {
            string  filename    = Path.Combine(Global.ReportPath, string.Format("{0}_{1}.pdf", artist_name, dateUtil.ToStringMilliseconds()));
            // 파일 IO 스트림을 취득한다.
            using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                // Pdf형식의 document를 생성한다.
                Document doc = new Document(PageSize.A4, 10, 10, 10, 10);

                PdfWriter writer = PdfWriter.GetInstance(doc, stream);
                // document Open
                doc.Open();
                try {
                    // Create a Simple table
                    PdfPTable table = new PdfPTable(1);
                    //table.SetTotalWidth(new float[] { 10f, 3000f, 10f});
                    //table.WidthPercentage = 100;

                    iTextSharp.text.Image mainThumb = iTextSharp.text.Image.GetInstance(imageUtil.DownloadImage(catalog.thumb), System.Drawing.Imaging.ImageFormat.Jpeg);
                    PdfPCell cell = new PdfPCell(new Phrase(""));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 76;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(mainThumb, false);
                    cell.Border = Rectangle.NO_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(""));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 76;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(catalog.title, new Font(bf, 22, Font.BOLD, CMYKColor.BLACK)));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    doc.Add(table);

                    // ------------------------ 표지 끝 ----------------------------

                    // ------------------------ 작가 관련 ----------------------------
                    doc.NewPage();

                    table = new PdfPTable(1);

                    cell = new PdfPCell(new Phrase(""));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 46;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    //cell = new PdfPCell(new Phrase(catalog.artist.localname, new Font(bf, 16, Font.NORMAL, CMYKColor.BLACK)));
                    cell = new PdfPCell(new Phrase(catalog.artist.localname, fTitle)); // <--- 이렇게 폰트를 미리 해야 한글이 나온다. 버그인듯
                    cell.Border = Rectangle.NO_BORDER;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                    
                    cell = new PdfPCell(new Phrase("("+ catalog.artist.englishname+")", new Font(bf, 14, Font.NORMAL, CMYKColor.BLACK)));
                    cell.Border         = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                    
                    cell = new PdfPCell(new Phrase("Artist Note &Introduction", new Font(bf, 14, Font.NORMAL | Font.ITALIC, CMYKColor.BLACK)));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Artist Note", new Font(bf, 14, Font.BOLD, CMYKColor.BLACK)));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    Phrase p = new Phrase(new Chunk(catalog.artist_note, new Font(bf, 10, Font.NORMAL, CMYKColor.BLACK)));
                    cell = new PdfPCell(p);
                    cell.Border = Rectangle.NO_BORDER;
//                    cell.FixedHeight = 126;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(""));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Introduction", new Font(bf, 14, Font.BOLD, CMYKColor.BLACK)));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    p = new Phrase(new Chunk(catalog.artist_intro, new Font(bf, 10, Font.NORMAL, CMYKColor.BLACK)));
                    cell = new PdfPCell(p);
                    cell.Border = Rectangle.NO_BORDER;
                    //                    cell.FixedHeight = 126;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(""));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Private Exhibition", new Font(bf, 14, Font.BOLD, CMYKColor.BLACK)));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    p = new Phrase(new Chunk(catalog.private_exhib, new Font(bf, 10, Font.NORMAL, CMYKColor.BLACK)));
                    cell = new PdfPCell(p);
                    cell.Border = Rectangle.NO_BORDER;
                    //                    cell.FixedHeight = 126;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(""));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Group Exibition", new Font(bf, 14, Font.BOLD, CMYKColor.BLACK)));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    p = new Phrase(new Chunk(catalog.group_exhib, new Font(bf, 10, Font.NORMAL, CMYKColor.BLACK)));
                    cell = new PdfPCell(p);
                    cell.Border = Rectangle.NO_BORDER;
                    //                    cell.FixedHeight = 126;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(""));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Email : " + catalog.artist.email, new Font(bf, 10, Font.NORMAL, CMYKColor.BLACK))); 
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Instagram : " + catalog.artist.instagram, new Font(bf, 10, Font.NORMAL, CMYKColor.BLACK)));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("facebook : " + catalog.artist.facebook, new Font(bf, 10, Font.NORMAL, CMYKColor.BLACK)));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("blog : " + catalog.artist.blog, new Font(bf, 10, Font.NORMAL, CMYKColor.BLACK)));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Homepage : " + catalog.artist.homepage, new Font(bf, 10, Font.NORMAL, CMYKColor.BLACK)));
                    cell.Border = Rectangle.NO_BORDER;
                    cell.FixedHeight = 26;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);

                    doc.Add(table);

                    // ------------------------ 작품 관련 ----------------------------
                    foreach(Artwork artwork in catalog.mArtworks)
                    {
                        doc.NewPage();

                        table = new PdfPTable(1);

                        cell = new PdfPCell(new Phrase(""));
                        cell.Border = Rectangle.NO_BORDER;
                        cell.FixedHeight = 76;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        Image aThumb = iTextSharp.text.Image.GetInstance(imageUtil.DownloadImage(artwork.image), System.Drawing.Imaging.ImageFormat.Jpeg);
                        cell = new PdfPCell(aThumb, false);
                        cell.Border = Rectangle.NO_BORDER;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(""));
                        cell.Border = Rectangle.NO_BORDER;
                        cell.FixedHeight = 46;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(artwork.title, sTitle));
                        cell.Border = Rectangle.NO_BORDER;
                        cell.FixedHeight = 26;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(artwork.year + ", " + artwork.size, sTitle));
                        cell.Border = Rectangle.NO_BORDER;
                        cell.FixedHeight = 26;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase(artwork.material, sTitle));
                        cell.Border = Rectangle.NO_BORDER;
                        cell.FixedHeight = 26;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.AddCell(cell);

                        doc.Add(table);
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                } finally {
                    // document Close
                    doc.Close();
                }
            }
            return "";
        }
    }
}
