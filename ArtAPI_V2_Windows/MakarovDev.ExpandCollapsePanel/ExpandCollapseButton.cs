using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MakarovDev.ExpandCollapsePanel
{
    /// <summary>
    /// Button with two states: expanded/collapsed
    /// </summary>
    public partial class ExpandCollapseButton : UserControl
    {
		private	bool	_isReloadVisible	= true;

        /// <summary>
        /// Set flag for expand or collapse button
        /// (true - expanded, false - collapsed)
        /// </summary>
        [Browsable(true)]
        [Category("ExpandCollapseButton")]
        [Description("Reload button.")]
        public bool IsReloadVisible
        {
            get { return _isReloadVisible; }
            set {
                _isReloadVisible = value;
				OnReloadVisible();
            }
        }

        /// <summary>
        /// Handle state changing
        /// </summary>
        protected virtual void OnReloadVisible()
        {
            pictureBox2.Visible = _isReloadVisible;
        }

		private	bool	_isSaveVisible	= true;

        /// <summary>
        /// Set flag for expand or collapse button
        /// (true - expanded, false - collapsed)
        /// </summary>
        [Browsable(true)]
        [Category("ExpandCollapseButton")]
        [Description("save button.")]
        public bool IsSaveVisible
        {
            get { return _isSaveVisible; }
            set {
                _isSaveVisible = value;
				OnSaveVisible();
            }
        }

        /// <summary>
        /// Handle state changing
        /// </summary>
        protected virtual void OnSaveVisible()
        {
            // set appropriate bitmap
            pictureBox3.Visible = _isSaveVisible;
            //lblHeader.ForeColor = _isExpanded ? Color.DarkGray : Color.SteelBlue;

            // and fire the event:
            //EventHandler<ExpandCollapseEventArgs> handler = ExpandCollapse;
            //if (handler != null)
            //    handler(this, new ExpandCollapseEventArgs(_isExpanded));
        }


        /// <summary>
        /// Image displays expanded state of button
        /// </summary>
        private Image _expanded;
        /// <summary>
        /// Image displays collapsed state of button
        /// </summary>
        private Image _collapsed;

        /// <summary>
        /// Set flag for expand or collapse button
        /// (true - expanded, false - collapsed)
        /// </summary>
        private bool _isExpanded;

        /// <summary>
        /// Set flag for expand or collapse button
        /// (true - expanded, false - collapsed)
        /// </summary>
        [Browsable(true)]
        [Category("ExpandCollapseButton")]
        [Description("Expand or collapse button.")]
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set {
                _isExpanded = value;
                OnExpandCollapse();
            }
        }

        /// <summary>
        /// Header
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Header")]
        [Browsable(true)]
        public override string Text
        {
            get
            {
                return lblHeader.Text;
            }
            set
            {
                lblHeader.Text = value;
            }
        }

        /// <summary>
        /// Font used for displays header text
        /// </summary>
        public override Font Font {
            get { return lblHeader.Font; }
            set { lblHeader.Font = value; }
        }

        /// <summary>
        /// Foreground color used for displays header text
        /// </summary>
        public override Color ForeColor
        {
            get
            {
                return lblHeader.ForeColor;
            }
            set
            {
                lblHeader.ForeColor = value;
            }
        }


        /// <summary>
        /// Occurs when the button has expanded or collapsed
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Occurs when the button has expanded or collapsed.")]
        [Browsable(true)]
        public event EventHandler<ExpandCollapseEventArgs> ExpandCollapse;

        /// <summary>
        /// Occurs when the button has expanded or collapsed
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Occurs when the button has expanded or collapsed.")]
        [Browsable(true)]
        public event EventHandler<RefreshEventArgs> PanelRefresh;

        /// <summary>
        /// Occurs when the button has expanded or collapsed
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Occurs when the button has expanded or collapsed.")]
        [Browsable(true)]
        public event EventHandler<RefreshEventArgs> PanelSave;

        public ExpandCollapseButton()
        {
            InitializeComponent();

            #region initialize expanded/collapsed state bitmaps:
            InitButtonStyle(ExpandButtonStyle.Circle);
            InitButtonSize(ExpandButtonSize.Normal);
            #endregion

            // initial state of panel - collapsed
            _isExpanded = false;
        }

        #region ExpandButtonStyles
        /// <summary>
        /// Visual styles of the expand-collapse button.
        /// </summary>
        public enum ExpandButtonStyle
        {
            Classic,
            Circle,
            MagicArrow,
            Triangle,
            FatArrow
        }
        private ExpandButtonStyle _expandButtonStyle = ExpandButtonStyle.Circle;

        /// <summary>
        /// Visual style of the expand-collapse button.
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Visual style of the expand-collapse button.")]
        [Browsable(true)]
        public ExpandButtonStyle ButtonStyle
        {
            get { return _expandButtonStyle; }
            set
            {
                if (_expandButtonStyle != value)
                {
                    InitButtonStyle(value);
                }
            }
        }

        private void InitButtonStyle(ExpandButtonStyle style)
        {
            _expandButtonStyle = style;

            switch (_expandButtonStyle)
            {
                case ExpandButtonStyle.MagicArrow:
                    var bmp = Properties.Resources.Upload;
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    pictureBox1.Image = bmp;
                    break;
                case ExpandButtonStyle.Circle:
                    bmp = Properties.Resources.icon_expand;
                    pictureBox1.Image = bmp;
                    break;
                case ExpandButtonStyle.Triangle:
                    pictureBox1.Image = Properties.Resources._1downarrow1;
                    break;
                case ExpandButtonStyle.FatArrow:
                    bmp = Properties.Resources.up_256;
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    pictureBox1.Image = bmp;
                    break;
                case ExpandButtonStyle.Classic:
                    bmp = Properties.Resources.icon_struct_hide_collapsed;
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    pictureBox1.Image = bmp;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("style");
            }

            // collapsed bitmap:
            _collapsed = pictureBox1.Image;

            // expanded bitmap is rotated collapsed bitmap:
            _expanded = MakeGrayscale3(pictureBox1.Image);
            _expanded.RotateFlip(RotateFlipType.Rotate180FlipNone);


            // finally set appropriate bitmap for current state
            pictureBox1.Image = _isExpanded ? _expanded : _collapsed;
        }
        #endregion ExpandButtonStyles

        #region ExpandButtonSizes
        /// <summary>
        /// Size presets of the expand-collapse button.
        /// </summary>
        public enum ExpandButtonSize
        {
            Small,
            Normal,
            Large
        }
        private ExpandButtonSize _expandButtonSize = ExpandButtonSize.Normal;

        /// <summary>
        /// Size preset of the expand-collapse button.
        /// </summary>
        [Category("ExpandCollapseButton")]
        [Description("Size preset of the expand-collapse button.")]
        [Browsable(true)]
        public ExpandButtonSize ButtonSize
        {
            get { return _expandButtonSize; }
            set
            {
                if (_expandButtonSize != value)
                {
                    InitButtonSize(value);
                }
            }
        }

		public	void	SetPanelWidth(int width) {
			pictureBox2.Location	= new Point(width - 8 - pictureBox2.Width, pictureBox2.Top);
			pictureBox3.Location	= new Point(width - 8 - pictureBox2.Width - 4 - pictureBox3.Width, pictureBox3.Top);
		}

        /// <summary>
        /// Resize and arrange child controls according to ButtonSize preset
        /// </summary>
        /// <param name="size">ButtonSize preset</param>
        private void InitButtonSize(ExpandButtonSize size)
        {
            _expandButtonSize = size;

            //pictureBox2.Location	= new Point(2, 4);
            //pictureBox2.Size		= new Size(16, 16);

            switch (_expandButtonSize)
            {
                case ExpandButtonSize.Small:
                    pictureBox1.Location	= new Point(0, 1);
                    pictureBox1.Size		= new Size(18, 18);
					pictureBox2.Location	= new Point(20, 0);
					pictureBox2.Size		= new Size(18, 18);
					pictureBox3.Location	= new Point(40, 4);
					pictureBox3.Size		= new Size(18, 18);
                    lblLine.Location		= new Point(20, 20);
                    lblHeader.Location		= new Point(30, 4);
                    break;
                case ExpandButtonSize.Normal:
                    pictureBox1.Location	= new Point(0, 0);
                    pictureBox1.Size		= new Size(24, 24);
					pictureBox2.Location	= new Point(26, 0);
					pictureBox2.Size		= new Size(24, 24);
					pictureBox3.Location	= new Point(52, 0);
					pictureBox3.Size		= new Size(24, 24);
                    lblLine.Location		= new Point(30, 26);
                    lblHeader.Location		= new Point(34, 5);
                    break;
                case ExpandButtonSize.Large:
                    pictureBox1.Location	= new Point(0, 1);
                    pictureBox1.Size		= new Size(35, 35);
					pictureBox2.Location	= new Point(37, 0);
					pictureBox2.Size		= new Size(35, 35);
					pictureBox3.Location	= new Point(74, 0);
					pictureBox3.Size		= new Size(35, 35);
                    lblLine.Location		= new Point(51, 32);
                    lblHeader.Location		= new Point(43, 6);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // after resize all child controls - do resize for entire ExpandCollapseButton control:
            Height = pictureBox1.Location.Y + pictureBox1.Height + 2;
        }
        #endregion ExpandButtonSizes




        /// <summary>
        /// Handle clicks from PictureBox and Header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMouseDown(object sender, EventArgs e)
        {
            // just invert current state
            IsExpanded = !IsExpanded;
        }

        /// <summary>
        /// Handle state changing
        /// </summary>
		protected virtual void OnPanelRefresh(object sender, MouseEventArgs e)
		{
            EventHandler<RefreshEventArgs> handler = PanelRefresh;
            if (handler != null)
                handler(this, new RefreshEventArgs(_isExpanded));
 
		}

        /// <summary>
        /// Handle state changing
        /// </summary>
		protected virtual void OnPanelSave(object sender, MouseEventArgs e)
		{
            EventHandler<RefreshEventArgs> handler = PanelSave;
            if (handler != null)
                handler(this, new RefreshEventArgs(_isExpanded));
 
		}


        /// <summary>
        /// Handle state changing
        /// </summary>
        protected virtual void OnExpandCollapse()
        {
            // set appropriate bitmap
            pictureBox1.Image = _isExpanded ? _expanded : _collapsed;
            //lblHeader.ForeColor = _isExpanded ? Color.DarkGray : Color.SteelBlue;

            // and fire the event:
            EventHandler<ExpandCollapseEventArgs> handler = ExpandCollapse;
            if (handler != null)
                handler(this, new ExpandCollapseEventArgs(_isExpanded));
        }

        /// <summary>
        /// Utillity method for createing a grayscale copy of image
        /// </summary>
        /// <param name="original">original image</param>
        /// <returns>grayscale copy of image</returns>
        public static Bitmap MakeGrayscale3(Image original)
        {
            // create a blank bitmap the same size as original
            var newBitmap = new Bitmap(original.Width, original.Height);

            // get a graphics object from the new image
            using (var g = Graphics.FromImage(newBitmap))
            {

                // create the grayscale ColorMatrix
                var colorMatrix = new ColorMatrix(
                    new float[][]
                        {
                            new float[] {.3f, .3f, .3f, 0, 0},
                            new float[] {.59f, .59f, .59f, 0, 0},
                            new float[] {.11f, .11f, .11f, 0, 0},
                            new float[] {0, 0, 0, 1, 0},
                            new float[] {0, 0, 0, 0, 1}
                        });

                // create some image attributes
                var attributes = new ImageAttributes();

                // set the color matrix attribute
                attributes.SetColorMatrix(colorMatrix);

                // draw the original image on the new image
                // using the grayscale color matrix
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                            0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

                // dispose the Graphics object
                g.Dispose();
            }

            return newBitmap;
        }

		private void pictureBox2_MouseHover(object sender, EventArgs e)
		{
			ToolTip tt = new ToolTip();
			tt.SetToolTip(this.pictureBox2, "(Get)장비로 부터 정보를 가지고 옵니다.");
		}

		private void pictureBox3_MouseHover(object sender, EventArgs e)
		{
			ToolTip tt = new ToolTip();
			tt.SetToolTip(this.pictureBox3, "(Set)장비에 현재 수정된 정보를 전송합니다.");
		}
	}
}
