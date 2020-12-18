using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Listboxtest
{
	/// <summary>
	/// Summary description for hListView.
	/// </summary>
	public class hListView : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.HScrollBar hsbScroll;
		private System.Windows.Forms.ListView lvImages;
		private ArrayList lvil = new ArrayList();
		private System.Windows.Forms.ImageList ilistImages;
		private System.Windows.Forms.ColumnHeader chText;
		private System.ComponentModel.IContainer components;

		public hListView()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Adds a string to the end of the list
		/// </summary>
		/// <param name="text"></param>
		public void Add(string text)
		{
			ListViewItem lvi = new ListViewItem(text);
			lvil.Add(lvi);
			hsbScroll.Maximum = lvil.Count;
			hsbScroll_Scroll(null, null);
		}
		
		/// <summary>
		/// Adds an image with subscription to the end of the list
		/// </summary>
		/// <param name="img"></param>
		/// <param name="text"></param>
		public void Add(Image img, string text)
		{
			ilistImages.Images.Add(img);
			ListViewItem lvi = new ListViewItem(text,ilistImages.Images.Count-1);
			lvil.Add(lvi);
			hsbScroll.Maximum = lvil.Count;
			hsbScroll_Scroll(null, null);
		}

		/// <summary>
		/// Adds an image to the end of the list
		/// </summary>
		/// <param name="img"></param>
		public void Add(Image img)
		{
			ilistImages.Images.Add(img);
			ListViewItem lvi = new ListViewItem("",ilistImages.Images.Count-1);
			lvil.Add(lvi);
			hsbScroll.Maximum = lvil.Count;
			hsbScroll_Scroll(null, null);
		}

		/// <summary>
		/// Adds an array of Images to the list
		/// </summary>
		/// <param name="images"></param>
		public void AddRange(Image[] images)
		{
			foreach(Image i in images)
			{
				ilistImages.Images.Add(i);
				ListViewItem lvi = new ListViewItem("",ilistImages.Images.Count-1);
				lvil.Add(lvi);
			}
			hsbScroll.Maximum = lvil.Count;
			hsbScroll_Scroll(null, null);
		}

		/// <summary>
		/// Removes the element at the specified index of the list
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			lvil.RemoveAt(index);
			hsbScroll.Maximum = lvil.Count;
			hsbScroll_Scroll(null, null);
		}

		/// <summary>
		/// Removes all the elements of the list
		/// </summary>
		public void Clear()
		{
			lvil.Clear();
			ilistImages.Images.Clear();
			hsbScroll.Maximum = lvil.Count;
			hsbScroll_Scroll(null, null);
		}

		// Gets the number of elements actually contained in the hListView
		public int Count()
		{
			return lvil.Count;
		}

		/// <summary>
		/// Updates the subscription at the specified index
		/// </summary>
		/// <param name="index"></param>
		/// <param name="text"></param>
		public void Update(int index, string text)
		{
			((ListViewItem) lvil[index]).Text = text;
			hsbScroll_Scroll(null, null);
		}

		public int SelectedIndex()
		{
			return lvImages.SelectedIndices[0] + hsbScroll.Value;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.hsbScroll = new System.Windows.Forms.HScrollBar();
			this.lvImages = new System.Windows.Forms.ListView();
			this.chText = new System.Windows.Forms.ColumnHeader();
			this.ilistImages = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// hsbScroll
			// 
			this.hsbScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.hsbScroll.LargeChange = 3;
			this.hsbScroll.Location = new System.Drawing.Point(0, 120);
			this.hsbScroll.Maximum = 10;
			this.hsbScroll.Name = "hsbScroll";
			this.hsbScroll.Size = new System.Drawing.Size(400, 17);
			this.hsbScroll.TabIndex = 3;
			this.hsbScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbScroll_Scroll);
			// 
			// lvImages
			// 
			this.lvImages.Alignment = System.Windows.Forms.ListViewAlignment.Default;
			this.lvImages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lvImages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					   this.chText});
			this.lvImages.LargeImageList = this.ilistImages;
			this.lvImages.Location = new System.Drawing.Point(0, 0);
			this.lvImages.Name = "lvImages";
			this.lvImages.Scrollable = false;
			this.lvImages.Size = new System.Drawing.Size(400, 120);
			this.lvImages.TabIndex = 2;
			// 
			// chText
			// 
			this.chText.Text = "Text";
			this.chText.Width = 100;
			// 
			// ilistImages
			// 
			this.ilistImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
			this.ilistImages.ImageSize = new System.Drawing.Size(100, 100);
			this.ilistImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// hListView
			// 
			this.Controls.Add(this.hsbScroll);
			this.Controls.Add(this.lvImages);
			this.Name = "hListView";
			this.Size = new System.Drawing.Size(400, 136);
			this.ResumeLayout(false);

		}
		#endregion

		private void hsbScroll_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			lvImages.Items.Clear();
			for (int i=hsbScroll.Value; i<hsbScroll.Value+(lvImages.Width/(lvImages.Columns[0].Width)-1); i++)
			{
				if (i<lvil.Count)
				{
					lvImages.Items.Add((ListViewItem)lvil[i]);
				}
			}
		}
	}
}
