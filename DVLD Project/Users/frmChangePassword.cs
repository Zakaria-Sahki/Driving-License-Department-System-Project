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

namespace DVLD_Project
{
    public partial class frmChangePassword : Form
    {
        private int _UserID;
        private clsUsers _User;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(750, 520);
            _UserID = UserID;
        }

        private void _ResetDefaultValues() {

            txtCurrentPassword.Text = string.Empty;
            txtNewPassword.Text = string.Empty;
            txtConfirmPassword.Text = string.Empty;
            txtCurrentPassword.Focus();
        }
        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            _User = clsUsers.FindUser(_UserID);

            if (_User == null) {

                MessageBox.Show($"User with UserID = [{_UserID}] not found.", "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            uctrlUserInfo1.LoadUserInfo(_UserID);
        }
        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(clsHashingPassword.ComputeHash(txtCurrentPassword.Text.Trim())))
            {

                e.Cancel = true;
                txtCurrentPassword.Focus();
                errorProvider1.SetError(txtCurrentPassword, "this field is required!");
                return;
            }
            else {

                errorProvider1.SetError(txtCurrentPassword, null);
            }
            if (clsHashingPassword.ComputeHash(txtCurrentPassword.Text.Trim()) != _User._Password)
            {

                e.Cancel = true;
                txtCurrentPassword.Focus();
                errorProvider1.SetError(txtCurrentPassword, "incorrect Password!");
                return;
            }
            else {

                errorProvider1.SetError(txtCurrentPassword, null);
            }
        }
        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewPassword.Text.Trim()))
            {

                e.Cancel = true;
                txtNewPassword.Focus();
                errorProvider1.SetError(txtNewPassword, "this field is required!");
            }
            else {

                errorProvider1.SetError(txtNewPassword, null);
            }
        }
        


        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            //{

            //    e.Cancel = true;
            //    txtConfirmPassword.Focus();
            //    errorProvider1.SetError(txtConfirmPassword, "this field is required!");
            //}
            if (txtConfirmPassword.Text.Trim() != txtNewPassword.Text.Trim())
            {

                e.Cancel = true;
                txtConfirmPassword.Focus();
                errorProvider1.SetError(txtConfirmPassword, "Confirm Password not match the password!");
            }
            else {

                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            _User._Password = txtNewPassword.Text.Trim();
            if (_User.Save())
            {
                _ResetDefaultValues();
                MessageBox.Show("Password Updated successfully.", "Saved Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {

                MessageBox.Show("Error: Password Updated Fail.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
