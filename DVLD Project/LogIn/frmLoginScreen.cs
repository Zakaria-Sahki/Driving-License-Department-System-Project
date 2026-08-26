using DVLDBussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DVLD_Project
{
    public partial class frmLoginScreen : Form
    {
        public frmLoginScreen()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {

                e.Cancel = true;
                txtUsername.Focus();
                errorProvider1.SetError(txtUsername, "Field is Required!");
            }
            else {

                errorProvider1.SetError(txtUsername, null);
            }
        }
        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {

                e.Cancel = true;
                txtPassword.Focus();
                errorProvider1.SetError(txtPassword, "Field is Required!");
            }
            else
            {

                errorProvider1.SetError(txtPassword, null);
            }
        }
        private void frmLoginScreen_Load(object sender, EventArgs e)
        {
            string Username = "", Password = "";
            if (clsGlobal.GetStoredCredential(ref Username, ref Password)) {

                txtUsername.Text = Username;
                txtPassword.Text = Password;
                cBRememberMe.Checked = true;
            }
            else {

                cBRememberMe.Checked = false;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {


            clsUsers User = clsUsers.FindUser(txtUsername.Text.Trim(), txtPassword.Text.Trim());
            if (User != null)
            {
                SaveRememberInfo();

                if (!User._IsActive) {

                    txtUsername.Focus();
                    MessageBox.Show("Your Account Is Not Active! Contact Admin.", "Account is Not Active", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                clsGlobal.CurrentUserInfo = User;
                this.Hide();
                Form frm = new MainForm(this);
                frm.ShowDialog();
                
            }
            else {

                txtUsername.Focus();
                MessageBox.Show("Invalid Username/Password!", "Wrong Credential", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }
        private void SaveRememberInfo() {

            if (cBRememberMe.Checked)
            {
                clsGlobal.RememberUsernameAndPassword(txtUsername.Text.Trim(), txtPassword.Text.Trim());
            }
            else
            {

                clsGlobal.RememberUsernameAndPassword("", "");
            }
        }

        private void cBRememberMe_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
