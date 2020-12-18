namespace ArtAPI
{
    partial class SignDetailForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tb_sign_id = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_sign_type = new System.Windows.Forms.ComboBox();
            this.tb_sign_title = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dtp_s_date = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtp_e_date = new System.Windows.Forms.DateTimePicker();
            this.tb_sign_amount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.bt_see_docu = new System.Windows.Forms.Button();
            this.bt_ok = new System.Windows.Forms.Button();
            this.bt_cancel = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_sign_desc = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "계약서 No";
            // 
            // tb_sign_id
            // 
            this.tb_sign_id.Enabled = false;
            this.tb_sign_id.Location = new System.Drawing.Point(86, 14);
            this.tb_sign_id.Name = "tb_sign_id";
            this.tb_sign_id.Size = new System.Drawing.Size(268, 21);
            this.tb_sign_id.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "계약 종류";
            // 
            // cb_sign_type
            // 
            this.cb_sign_type.FormattingEnabled = true;
            this.cb_sign_type.Items.AddRange(new object[] {
            "콜라보",
            "용역",
            "작품구입"});
            this.cb_sign_type.Location = new System.Drawing.Point(86, 41);
            this.cb_sign_type.Name = "cb_sign_type";
            this.cb_sign_type.Size = new System.Drawing.Size(268, 20);
            this.cb_sign_type.TabIndex = 3;
            this.cb_sign_type.Text = "콜라보";
            // 
            // tb_sign_title
            // 
            this.tb_sign_title.Location = new System.Drawing.Point(86, 78);
            this.tb_sign_title.Name = "tb_sign_title";
            this.tb_sign_title.Size = new System.Drawing.Size(268, 21);
            this.tb_sign_title.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "계약명";
            // 
            // dtp_s_date
            // 
            this.dtp_s_date.Location = new System.Drawing.Point(86, 109);
            this.dtp_s_date.Name = "dtp_s_date";
            this.dtp_s_date.Size = new System.Drawing.Size(200, 21);
            this.dtp_s_date.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(39, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "계약일";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "만료일";
            // 
            // dtp_e_date
            // 
            this.dtp_e_date.Location = new System.Drawing.Point(86, 136);
            this.dtp_e_date.Name = "dtp_e_date";
            this.dtp_e_date.Size = new System.Drawing.Size(200, 21);
            this.dtp_e_date.TabIndex = 8;
            // 
            // tb_sign_amount
            // 
            this.tb_sign_amount.Location = new System.Drawing.Point(86, 173);
            this.tb_sign_amount.Name = "tb_sign_amount";
            this.tb_sign_amount.Size = new System.Drawing.Size(248, 21);
            this.tb_sign_amount.TabIndex = 11;
            this.tb_sign_amount.Text = "0";
            this.tb_sign_amount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tb_sign_amount.TextChanged += new System.EventHandler(this.tb_sign_amount_TextChanged);
            this.tb_sign_amount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_sign_amount_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(29, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "계약금액";
            // 
            // bt_see_docu
            // 
            this.bt_see_docu.Location = new System.Drawing.Point(31, 314);
            this.bt_see_docu.Name = "bt_see_docu";
            this.bt_see_docu.Size = new System.Drawing.Size(99, 31);
            this.bt_see_docu.TabIndex = 12;
            this.bt_see_docu.Text = "계약서 보기";
            this.bt_see_docu.UseVisualStyleBackColor = true;
            this.bt_see_docu.Click += new System.EventHandler(this.bt_see_docu_Click);
            // 
            // bt_ok
            // 
            this.bt_ok.Location = new System.Drawing.Point(198, 314);
            this.bt_ok.Name = "bt_ok";
            this.bt_ok.Size = new System.Drawing.Size(74, 31);
            this.bt_ok.TabIndex = 13;
            this.bt_ok.Text = "확  인";
            this.bt_ok.UseVisualStyleBackColor = true;
            this.bt_ok.Click += new System.EventHandler(this.bt_ok_Click);
            // 
            // bt_cancel
            // 
            this.bt_cancel.Location = new System.Drawing.Point(280, 314);
            this.bt_cancel.Name = "bt_cancel";
            this.bt_cancel.Size = new System.Drawing.Size(74, 31);
            this.bt_cancel.TabIndex = 14;
            this.bt_cancel.Text = "취  소";
            this.bt_cancel.UseVisualStyleBackColor = true;
            this.bt_cancel.Click += new System.EventHandler(this.bt_cancel_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(40, 206);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "코멘트";
            // 
            // tb_sign_desc
            // 
            this.tb_sign_desc.Location = new System.Drawing.Point(86, 202);
            this.tb_sign_desc.Multiline = true;
            this.tb_sign_desc.Name = "tb_sign_desc";
            this.tb_sign_desc.Size = new System.Drawing.Size(268, 90);
            this.tb_sign_desc.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(337, 179);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "원";
            // 
            // SignDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 362);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tb_sign_desc);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.bt_cancel);
            this.Controls.Add(this.bt_ok);
            this.Controls.Add(this.bt_see_docu);
            this.Controls.Add(this.tb_sign_amount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtp_e_date);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtp_s_date);
            this.Controls.Add(this.tb_sign_title);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cb_sign_type);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_sign_id);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SignDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "계약 상세 내용";
            this.Load += new System.EventHandler(this.SignDetailForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_sign_id;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cb_sign_type;
        private System.Windows.Forms.TextBox tb_sign_title;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtp_s_date;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtp_e_date;
        private System.Windows.Forms.TextBox tb_sign_amount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button bt_see_docu;
        private System.Windows.Forms.Button bt_ok;
        private System.Windows.Forms.Button bt_cancel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_sign_desc;
        private System.Windows.Forms.Label label8;
    }
}