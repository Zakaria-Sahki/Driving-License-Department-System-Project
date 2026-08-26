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
    public partial class frmDetainLicense : Form
    {
        private int _LicenseID = -1;
        private int _DetainID = -1;
        public frmDetainLicense()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(980, 550);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _LicenseID = obj;
            linklblShowLicenseHistory.Enabled = (_LicenseID != -1);
            lblLicenseID.Text = _LicenseID.ToString();

            if (_LicenseID == -1) {

                return;
            }

            if (ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained) {

                MessageBox.Show("Selected License already detained, choose another one.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDetain.Enabled = false;
                return;
            }

            txtFineFees.Focus();
            btnDetain.Enabled = true;
        }
        private void frmDetainLicense_Load(object sender, EventArgs e)
        {

            lblDetainDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedBy.Text = clsGlobal.CurrentUserInfo._UserName;

        }
        private void frmDetainLicense_Activated(object sender, EventArgs e)
        {
            ctrlLicenseInfoWithFilter1.txtLicenseIDFocus();
        }
        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to detain this license ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            _DetainID = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.Detain(Convert.ToSingle(txtFineFees.Text.Trim()), clsGlobal.CurrentUserInfo._UserID);

            if (_DetainID == -1) {

                MessageBox.Show($"Faild to detain license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show($"License Detained Successfully with ID = {_DetainID}.", "License Detained", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lblDetainID.Text = _DetainID.ToString();
            
            btnDetain.Enabled = false;
            ctrlLicenseInfoWithFilter1.FilterEnabled = false;
            txtFineFees.Enabled = false;
            linklblShowLicenseInfo.Enabled = true;
        }
        private void linklblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmLicenseHistory(ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }
        private void linklblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }
        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Fees cannot be empty!");
                return;
            }
            else
                errorProvider1.SetError(txtFineFees, null);

            if (!clsValidation.IsNumber(txtFineFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Invalid Number.");
            }
            else
                errorProvider1.SetError(txtFineFees, null);
        }
    }
}
