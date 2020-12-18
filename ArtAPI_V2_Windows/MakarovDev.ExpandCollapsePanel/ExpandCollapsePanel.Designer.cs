namespace MakarovDev.ExpandCollapsePanel
{
    partial class ExpandCollapsePanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this._btnExpandCollapse = new MakarovDev.ExpandCollapsePanel.ExpandCollapseButton();
			this.animationTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// _btnExpandCollapse
			// 
			this._btnExpandCollapse.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._btnExpandCollapse.ButtonSize = MakarovDev.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonSize.Normal;
			this._btnExpandCollapse.ButtonStyle = MakarovDev.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonStyle.Circle;
			this._btnExpandCollapse.IsExpanded = false;
			this._btnExpandCollapse.Location = new System.Drawing.Point(3, 3);
			this._btnExpandCollapse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			//this._btnExpandCollapse.MaximumSize = new System.Drawing.Size(0, 46);
			this._btnExpandCollapse.MaximumSize = new System.Drawing.Size(0, 34);
			this._btnExpandCollapse.Name = "_btnExpandCollapse";
			this._btnExpandCollapse.Size = new System.Drawing.Size(0, 29);
			//this._btnExpandCollapse.Font = new System.Drawing.Font("굴림", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

			this._btnExpandCollapse.TabIndex = 0;
			// 
			// animationTimer
			// 
			this.animationTimer.Interval = 20;
			this.animationTimer.Tick += new System.EventHandler(this.animationTimer_Tick);
			// 
			// ExpandCollapsePanel
			// 
			this.Controls.Add(this._btnExpandCollapse);
			this.Size = new System.Drawing.Size(365, 319);
			this.ResumeLayout(false);
        }

        #endregion

        private MakarovDev.ExpandCollapsePanel.ExpandCollapseButton _btnExpandCollapse;
        private System.Windows.Forms.Timer animationTimer;
    }
}
