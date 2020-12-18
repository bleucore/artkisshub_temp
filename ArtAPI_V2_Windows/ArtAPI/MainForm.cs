
using ArtAPI.info;
using ArtAPI.library;
using ArtAPI.network;
using ArtAPI.network.payload;
using ArtAPI.utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Threading;
using Tulpep.NotificationWindow;
using Widget;
using ArtAPI.Adapter;
using Newtonsoft.Json.Linq;
using TikaOnDotNet.TextExtraction;
using System.Drawing;
using iTextSharp.text.pdf;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using System.Drawing.Imaging;

namespace ArtAPI
{
	public	partial		class	MainForm : Form
	{
		public	CrackInfo	mCurCrackInfo	= new CrackInfo();

        public  string  mCurArtistID    = "";


        ListUtil    listUtil	= new ListUtil();
		DateUtil	dateUtil	= new DateUtil();
		ImageUtil	imageUtil	= new ImageUtil();

		public	MainForm(){
			InitializeComponent();
		}

        private void    DispCatalog(IssuuInfo iInfo)
        {
            if (iInfo == null)             return;
            //if (iInfo.mArtist == null)     return;
            //if (string.IsNullOrEmpty(iInfo.mArtist._id)) return;
            //if (iInfo.mArtist._id != mCurArtistID)
            //{
                hlv_catalog.Clear();

                foreach(IssuuDoc doc in iInfo.docs) {
                    hlv_catalog.Add(imageUtil.LoadPicture(doc.mThumb), doc.mTitle);
                }

                hlv_catalog.Add(new Bitmap("River_Sumida.bmp"), "pic 2");
                hlv_catalog.Add(new Bitmap("Santa_Fe_Stucco.bmp"), "pic 3");
                hlv_catalog.Add(new Bitmap("Soap_Bubbles.bmp"), "pic 4");

            //}
        }

        private void    CCUMainForm_Load(object sender, EventArgs e)
		{
            Global.mArtKissApt.Init(Update);

            foreach (string title in Artist.titles)
            {
                ListViewItem item = new ListViewItem();
                item.SubItems.Add(title);
                item.SubItems.Add("");

                lv_artist_info.Items.Add(item);
            }


            Global.mEtsyApt.Init(Update);
            /*
			string[] files = Directory.GetFiles(Global.mCrackFilePath);
			foreach (string file in files)
			{
				string fileName = Path.GetFileName(file);
				ListViewItem item = new ListViewItem();
				item.SubItems.Add(fileName);
//				item.SubItems.Add(file);
				//item.Tag = file;

				lv_crack_file_list.Items.Add(item);
			}

			foreach( string title in CrackInfo.titles) {
				ListViewItem item = new ListViewItem();
				item.SubItems.Add(title);
				item.SubItems.Add("");

				lv_crack_head.Items.Add(item);
			}

			//DispSenarioList();
            */
        }

        /*
        public void AddCCUList(CCUInfo ccu)
        {
            this.Invoke(new Action(delegate () {
                ListViewItem left = new ListViewItem("S");
                left.SubItems.Add(ccu.mCCUID);
                left.SubItems.Add(ccu.mSite);
                left.SubItems.Add(ccu.mStatus);
                left.SubItems.Add(ccu.mAddr);

                lv_ccu_list.Items.Add(left);

                ListViewItem right = new ListViewItem();
                right.SubItems.Add(ccu.mCCUID);
                right.SubItems.Add(ccu.mSite);

                lv_state_ccu_list.Items.Add(right);
            }));
        }
        */

        public void AddArtist(Artist artist)
        {
            this.Invoke(new Action(delegate () {

                TreeNode aNode  = new TreeNode(string.Format("[{0}] {1} ({2})", artist.usercountryname, artist.englishname, artist.email), 0, 0);
                aNode.Tag       = string.Format("{0}", artist._id);

                tv_art_kiss.Nodes.Add(aNode);
                foreach (Artwork artwork in artist.mArtwork) {
                    TreeNode wNode = new TreeNode(string.Format("[{0}] {1}", artwork.year, artwork.title), 0, 0);
                    aNode.Nodes.Add(wNode);
                }
                //tv_art_kiss.Nodes.Add(sectionNode);
                /*
                TreeNode sectionNode = new TreeNode(string.Format("[{0}] {1}", section.mSecCode, section.mSecSite), 0, 0);

                TreeNode stxNode = new TreeNode("시작 지점", 0, 0);
                foreach (var ccu in section.mStxPoint)
                {
                    TreeNode ccuNode = new TreeNode(string.Format("[{0}] {1}", ccu.mCCUID, ccu.mSite), 0, 0);
                    stxNode.Nodes.Add(ccuNode);
                }
                stxNode.ExpandAll();

                TreeNode etxNode = new TreeNode("종료 지점", 0, 0);
                foreach (var ccu in section.mEtxPoint)
                {
                    TreeNode ccuNode = new TreeNode(string.Format("[{0}] {1}", ccu.mCCUID, ccu.mSite), 0, 0);
                    etxNode.Nodes.Add(ccuNode);
                }
                etxNode.ExpandAll();

                sectionNode.Nodes.Add(stxNode);
                sectionNode.Nodes.Add(etxNode);
                sectionNode.ExpandAll();

                tv_art_kiss.Nodes.Add(sectionNode);

                TreeNode spaceNode = new TreeNode("", 0, 0);
                tv_art_kiss.Nodes.Add(spaceNode);
                */
            }));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Update(Object observer)
        {
            if (observer is string)
            {
                string mode = (string)observer;
                switch (mode)
                {
                    case "load_artist_finish":
                        foreach (var artist in Global.mArtists)
                        {
                            AddArtist(artist);
                            //artist.Start();
                            //artist.SetDispDeligate(UpdateCCUState);
                        }
                        break;
                    case "load_esty_finish":
                        Console.WriteLine("load_esty_finish");
                        break;
                }
            }

            //Thread t = new Thread(new ParameterizedThreadStart(LoopMonitor));
            //t.Start(this);
        }

        /*
		private	void	DispSenarioList() {
			lv_crack_list_stx.Items.Clear();
			lv_crack_list_etx.Items.Clear();
			foreach(var race in Global.mRaceList) {
				ListViewItem item	= new ListViewItem();
				item.SubItems.Add(race.mDelay.ToString());
				item.SubItems.Add(dateUtil.ToStringOnlyTime(race.mTime));
				item.SubItems.Add(race.mSpeed.ToString());
				item.SubItems.Add(race.mRoad);
				item.SubItems.Add(race.mCarNo);

				if (race.mType == "S")	lv_crack_list_stx.Items.Add(item);
				if (race.mType == "E")	lv_crack_list_etx.Items.Add(item);
			}
		}
        */

        public void	DispArtistInfo(Artist aInfo) {
            //pb_crack_main_image.Image = aInfo.mMainImage;

            listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 0], aInfo.localname);
			listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 1], aInfo.englishname);
			listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 2], aInfo.edulevel);
			listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 3], aInfo.ex_career);
			listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 4], aInfo.ex_group_career);
			listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 5], aInfo.awards);
			listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 6], aInfo.email);
			listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 7], aInfo.mobile);
			listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 8], aInfo.usercountryname);
			listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 9], aInfo.facebook);
			listUtil.SetValueCrack(lv_artist_info, Artist.titles[10], aInfo.instagram);
			listUtil.SetValueCrack(lv_artist_info, Artist.titles[11], aInfo.blog);
			listUtil.SetValueCrack(lv_artist_info, Artist.titles[12], aInfo.homepage);
		}


		private void bt_send_end_crack_Click(object sender, EventArgs e)
		{
		}

		private void bt_send_start_crack_Click(object sender, EventArgs e)
		{
		}

		private void bt_crack_senario_start_Click(object sender, EventArgs e)
		{
		}

		public	int		GetIndexFromCarNo(int pos, string car_no) {
			for(int idx = 0; idx < Global.mRaceList.Count; idx++) {
				if (Global.mRaceList[idx].mCarNo == car_no && pos != idx)		return	idx;
			}
			return	-1;
		}


		private void bt_crack_senario_end_Click(object sender, EventArgs e)
		{
			foreach(var race in Global.mRaceList) {
				race.StopAction();
			}
		}

		private void bt_e_direct_event_Click(object sender, EventArgs e)
		{
		}

		private void button2_Click(object sender, EventArgs e)
		{
		}

		private void ecp_crack_info_PanelSave(object sender, MakarovDev.ExpandCollapsePanel.RefreshEventArgs e)
		{
			if (mCurCrackInfo == null)		return;

			// 이미지를 파일로 저장을 한다.
			string	savePath	= ".\\CrackInfo";
			if (!System.IO.Directory.Exists(savePath))	System.IO.Directory.CreateDirectory(savePath);

			if (mCurCrackInfo.mMainImage != null)	
				mCurCrackInfo.mMainImage.Save(savePath + "\\Test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
				//mCurCrackInfo.mMainImage.Save(savePath+ "\\Test.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
		}

		private void ecp_crack_senario_PanelRefresh(object sender, MakarovDev.ExpandCollapsePanel.RefreshEventArgs e)
		{
		}

        private void bt_all_clear_catalog_Click(object sender, EventArgs e)
        {
            hlv_catalog.Clear();
        }


        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
            Environment.Exit(0);
        }


        private void tv_art_kiss_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Artist  artist  = null;

            TreeView treeView = (TreeView)sender;
            treeView.SelectedNode = e.Node;

            string  artist_id   = "";
            if (treeView.SelectedNode != null) {
                Console.WriteLine(treeView.SelectedNode.Text);
                if (treeView.SelectedNode.Tag != null) {
                    artist_id   = treeView.SelectedNode.Tag.ToString();
                    Console.WriteLine(artist_id);
                    artist  = Global.FindArtist(artist_id);
                }
            }

            if (artist != null)
            {
                pb_artist_picture.ImageLocation = artist.profileurl;

                listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 0], artist.localname);
                listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 1], artist.englishname);
                listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 2], artist.edulevel);
                listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 3], artist.ex_career);
                listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 4], artist.ex_group_career);
                listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 5], artist.awards);
                listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 6], artist.email);
                listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 7], artist.mobile);
                listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 8], artist.usercountryname);
                listUtil.SetValueCrack(lv_artist_info, Artist.titles[ 9], artist.facebook);
                listUtil.SetValueCrack(lv_artist_info, Artist.titles[10], artist.instagram);
                listUtil.SetValueCrack(lv_artist_info, Artist.titles[11], artist.blog);
                listUtil.SetValueCrack(lv_artist_info, Artist.titles[12], artist.homepage);
            }

            // DocuList
            this.Invoke(new Action(delegate () {
                List<JObject>   list    = Global.mDocuSignApt.GetList(artist_id);
                lv_crack_real.Items.Clear();
                foreach (JObject docu in list)
                {
                    // List 추가
                    ListViewItem left = new ListViewItem("S");
                    left.Tag    = docu["sign_id"].ToString();

                    //left.SubItems.Add(docu["pay_type"].ToString());
                    left.SubItems.Add(docu["s_date"].ToString());
                    left.SubItems.Add(docu["type"].ToString());
                    left.SubItems.Add(docu["title"].ToString());
                    left.SubItems.Add(docu["comps"][0].ToString());
                    left.SubItems.Add(docu["amount"].ToString());
                    left.SubItems.Add(docu["e_date"].ToString());

                    lv_crack_real.Items.Add(left);
                }
            }));

            // Esty
            this.Invoke(new Action(delegate () {
                JObject list = Global.mArtKissApt.GetArtwork(artist_id);

                if (list != null)
                {
                    string  imageurl = list["imageurl"].ToString();
                    pb_crack_main_image.ImageLocation   = imageurl;
                    tb_artwork_title.Text   = list["title"].ToString();
                }
                /*
                lv_crack_real.Items.Clear();
                foreach (JObject docu in list)
                {
                    // List 추가
                    ListViewItem left = new ListViewItem("S");
                    left.Tag = docu["sign_id"].ToString();

                    //left.SubItems.Add(docu["pay_type"].ToString());
                    left.SubItems.Add(docu["s_date"].ToString());
                    left.SubItems.Add(docu["type"].ToString());
                    left.SubItems.Add(docu["title"].ToString());
                    left.SubItems.Add(docu["comps"][0].ToString());
                    left.SubItems.Add(docu["amount"].ToString());
                    left.SubItems.Add(docu["e_date"].ToString());

                    lv_crack_real.Items.Add(left);
                }
                */
            }));

        }

        private void button3_Click(object sender, EventArgs e)
        {
            PopupNotifier popup = new PopupNotifier();
            popup.TitleText = "Title Text";
            popup.ContentText = "Content";
            popup.Popup();
        }

        private void ecp_catalog_PanelRefresh(object sender, MakarovDev.ExpandCollapsePanel.RefreshEventArgs e)
        {
            IssuuInfo iInfo = Global.mIssuuApt.AppendIssuuInfo("1234");
            DispCatalog(iInfo);
        }

        private void hlv_catalog_MouseDoubleClick(object sender, EventArgs e)
        {
            Console.WriteLine("OnMouseDoubleClick : {0}", hlv_catalog.SelectedIndex());
            try {
                IssuuDoc iDoc   = Global.mIssuuApt.mCurIssuu.docs[hlv_catalog.SelectedIndex()];
                System.Diagnostics.Process.Start(iDoc.mUrl);
            } catch(Exception ee) { }

        }

        private void bt_catalog_add_Click(object sender, EventArgs e)
        {
            CataInfo    cataInfo    = new CataInfo("test_artist_id");
            cataInfo.MakeTestData();

            IssuuPDF    iPDF    = new IssuuPDF();
            iPDF.Init();
            iPDF.MakePDF("hyperpro", cataInfo);
        }

        private void lv_crack_real_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lv_crack_real_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var senderList = (ListView)sender;
            if (senderList.SelectedItems.Count > 0)
            {
                ListView.SelectedListViewItemCollection items = senderList.SelectedItems;
                ListViewItem item = items[0];
                string sign_id = item.Tag.ToString();

                //SetCurCCU(ccu_no);
                SignDetailForm  form    = new SignDetailForm();
                form.mSignID    = sign_id;
                form.ShowDialog();
            }
        }

        private void PdfToPng(string inputFile, string outputFileName, int pageNumber)
        {
            var xDpi = 100; //set the x DPI
            var yDpi = 100; //set the y DPI
            //var pageNumber = 1; // the pages in a PDF document

            using (var rasterizer = new GhostscriptRasterizer()) //create an instance for GhostscriptRasterizer
            {
                rasterizer.Open(inputFile); //opens the PDF file for rasterizing

                //set the output image(png's) complete path
                var outputPNGPath = Path.Combine(@"C:\Project\AnyGate\", string.Format("{0}.png", outputFileName));

                //converts the PDF pages to png's 
                var pdf2PNG = rasterizer.GetPage(xDpi, yDpi, pageNumber);

                //save the png's
                pdf2PNG.Save(outputPNGPath, ImageFormat.Png);

                Console.WriteLine("Saved " + outputPNGPath);
            }
        }

        // C:\Project\AnyGate\자료\성민글로벌_표준재무제표(PDF,한글)\부속명세서(제조원가명세서).pdf
        // C:\Project\AnyGate\자료\성민글로벌_표준재무제표(PDF,한글)\표준대차대조표.pdf
        // C:\Project\AnyGate\자료\성민글로벌_표준재무제표(PDF,한글)\표준손익계산서.pdf

        private void bt_text_from_pdf_Click(object sender, EventArgs e)
        {
            /*
            PdfToPng(@"C:\Project\AnyGate\sin_1.pdf", "sin_1_1", 1);
            PdfToPng(@"C:\Project\AnyGate\sin_1.pdf", "sin_1_2", 2);

            PdfToPng(@"C:\Project\AnyGate\sin_2.pdf", "sin_2_1", 1);
            PdfToPng(@"C:\Project\AnyGate\sin_2.pdf", "sin_2_2", 2);
            PdfToPng(@"C:\Project\AnyGate\sin_2.pdf", "sin_2_3", 3);
            PdfToPng(@"C:\Project\AnyGate\sin_2.pdf", "sin_2_4", 4);
            PdfToPng(@"C:\Project\AnyGate\sin_2.pdf", "sin_2_5", 5);
            PdfToPng(@"C:\Project\AnyGate\sin_2.pdf", "sin_2_6", 6);
            PdfToPng(@"C:\Project\AnyGate\sin_2.pdf", "sin_2_7", 7);
            PdfToPng(@"C:\Project\AnyGate\sin_2.pdf", "sin_2_8", 8);
            PdfToPng(@"C:\Project\AnyGate\sin_2.pdf", "sin_2_9", 9);

            PdfToPng(@"C:\Project\AnyGate\sin_3.pdf", "sin_3_1", 1);
            PdfToPng(@"C:\Project\AnyGate\sin_3.pdf", "sin_3_2", 2);

            var textExtractor = new TextExtractor();

            var result = textExtractor.Extract(@"C:\Project\AnyGate\sin_1.pdf");
            Console.WriteLine("\n\n" + result.Text.Trim());
            result = textExtractor.Extract(@"C:\Project\AnyGate\sin_2.pdf");
            Console.WriteLine("\n\n" + result.Text.Trim());
            result = textExtractor.Extract(@"C:\Project\AnyGate\sin_3.pdf");
            Console.WriteLine("\n\n" + result.Text.Trim());
            */
        }
    }
    /*
    public class CustomResult
    {
        public string Text { get; set; }
        public IDictionary;
        string, string[]; Metadata { get; set; }
    }
    */
}
