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
    public partial class frmAddEditUser : Form
    {
        enum enMode { AddUser, UpdateUser};
        enMode Mode = enMode.AddUser;
        private int _PersonSelectedID = -1;
        private int _UserID;
        clsUsers _User;


        public frmAddEditUser()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(820, 630);
            Mode = enMode.AddUser;
        }
        public frmAddEditUser(int UserID) { 
            
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(820, 630);
            _UserID = UserID;
            Mode = enMode.UpdateUser;
        }

        public void _RefreshDefaultValue() {

            if (Mode == enMode.AddUser)
            {

                lblTitle.Text = "Add New User";
                this.Text = "Add New User";
                tpLoginInfo.Enabled = false;


                ctrlPersonCardWithFilter1.FilterFocus();
                _User = new clsUsers();
            }
            else {

                lblTitle.Text = "Update User";
                this.Text = "Update User";
                tpLoginInfo.Enabled = true;
                
                // btnSave.Enabled=true;
            }

            txtConfirmPassword.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtUserName.Text = string.Empty;
            cbIsActive.Checked = true;
        }
        public void _LoadData() {

            _User = clsUsers.FindUser(_UserID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if (_User == null) {

                MessageBox.Show($"No User with ID [{_UserID}]", "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlPersonCardWithFilter1.LoadData(_User._PersonID);
            lblUserID.Text = _User._UserID.ToString();
            txtUserName.Text = _User._UserName;
            txtPassword.Text = _User._Password;
            txtConfirmPassword.Text = _User._Password;
            cbIsActive.Checked = _User._IsActive;
            
        }
        private void btnNext_Click(object sender, EventArgs e)
        {

            if (Mode == enMode.UpdateUser) {

                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;
                tabControl1.SelectedIndex = 1; // Login info tab
                return;
            }

            if (_PersonSelectedID != -1)
            {

                if (clsUsers.IsExistUserByPersonID(_PersonSelectedID))
                {

                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select Another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();
                    return;
                }
                else
                {

                    btnSave.Enabled = true;
                    tpLoginInfo.Enabled = true;
                    tabControl1.SelectedIndex = 1; // Login info tab
                }
            }
            else {

                MessageBox.Show("Please Select a person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ctrlPersonCardWithFilter1.FilterFocus();
                return;
            }
        }
        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            
            _PersonSelectedID = obj;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmAddEditUser_Load(object sender, EventArgs e)
        {

            _RefreshDefaultValue();
            if(Mode == enMode.UpdateUser)
                _LoadData();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            _User._PersonID = _PersonSelectedID;
            _User._UserName = txtUserName.Text.Trim();
            _User._Password = txtPassword.Text.Trim();
            _User._IsActive = cbIsActive.Checked;



            if (_User.Save())
            {
                lblUserID.Text = _User._UserID.ToString(); 
                Mode = enMode.UpdateUser;
                lblTitle.Text = "Update User";
                MessageBox.Show("Data Saved Successfully.", "Saved Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {

                MessageBox.Show("Error: Data is not saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {

            
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {

                e.Cancel = true;
                txtUserName.Focus();
                errorProvider1.SetError(txtUserName, "UserName Required!");
            }
            else {

                errorProvider1.SetError(txtUserName, null);
            }


            if (Mode == enMode.AddUser)
            {

                if (clsUsers.IsExistUser(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "username is used by another user");
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                }

            }
            else
            {

                //incase update make sure not to use anothers user name
                if (_User._UserName != txtUserName.Text.Trim())
                {
                    if (clsUsers.IsExistUser(txtUserName.Text.Trim()))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "username is used by another user");
                        return;
                    }
                    else
                    {
                        errorProvider1.SetError(txtUserName, null);
                    }

                }
            }
        }
        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text.Trim()))
            {

                e.Cancel = true;
                txtPassword.Focus();
                errorProvider1.SetError(txtPassword, "Password Required!");
            }
            else {

                errorProvider1.SetError(txtPassword, null);
            }
        }
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {

                e.Cancel = true;
                txtConfirmPassword.Focus();
                errorProvider1.SetError(txtConfirmPassword, "Confirm Password Required!");
            }
            else {

                errorProvider1.SetError(txtConfirmPassword, null);
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {

                e.Cancel = true;
                txtConfirmPassword.Focus();
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");

            }
            else {

                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }


    }
}
