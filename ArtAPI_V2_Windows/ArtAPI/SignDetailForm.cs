using ArtAPI.info;
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
    public partial class SignDetailForm : Form
    {
        public  string      mSignID;
        public  SignInfo    mSignInfo;

        public SignDetailForm()
        {
            InitializeComponent();
        }

        private void    bt_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void    bt_ok_Click(object sender, EventArgs e)
        {
            mSignInfo.type      = cb_sign_type.Text;
            mSignInfo.title     = tb_sign_title.Text;
            mSignInfo.s_date    = dtp_s_date.Text;
            mSignInfo.e_date    = dtp_e_date.Text;
            mSignInfo.amount    = tb_sign_amount.Text;
            mSignInfo.desc      = tb_sign_desc.Text;

            Global.mDocuSignApt.SetSignInfo(mSignInfo);

            MessageBox.Show("저장 성공 :" + mSignID);

            Close();
        }

        private void    bt_see_docu_Click(object sender, EventArgs e)
        {

        }

        private void    SignDetailForm_Load(object sender, EventArgs e)
        {
            mSignInfo   = Global.mDocuSignApt.GetSignInfo(mSignID);

            tb_sign_id.Text     = mSignInfo.sign_id;
            cb_sign_type.Text   = mSignInfo.type;
            tb_sign_title.Text  = mSignInfo.title;
            try {
                dtp_s_date.Text = mSignInfo.s_date;
            } catch(Exception ee1) {
                dtp_s_date.Value    = DateTime.Now;
            }
            try {
                dtp_e_date.Text = mSignInfo.e_date;
            } catch (Exception ee2) {
                dtp_e_date.Value = DateTime.Now;
            }

            tb_sign_amount.Text = mSignInfo.amount;
            if (string.IsNullOrEmpty(tb_sign_amount.Text)) tb_sign_amount.Text = "0";

            tb_sign_desc.Text   = mSignInfo.desc;
        }

        private void tb_sign_amount_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string lgsText;

            lgsText = textBox.Text.Replace(",", ""); //** 숫자변환시 콤마로 발생하는 에러방지...

            try {
                textBox.Text = String.Format("{0:#,##0}", Convert.ToDouble(lgsText));
            } catch(Exception ee) {
                textBox.Text = "0";
            }

            textBox.SelectionStart = textBox.TextLength; //** 캐럿을 맨 뒤로 보낸다...
            textBox.SelectionLength = 0;

        }

        private void tb_sign_amount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && e.KeyChar != 8) {
                e.Handled = true;
            }
        }
    }
}
