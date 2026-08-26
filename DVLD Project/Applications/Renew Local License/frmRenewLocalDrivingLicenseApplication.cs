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
    public partial class frmRenewLocalDrivingLicenseApplication : Form
    {
        private int _NewLicenseID = -1;
        public frmRenewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 660);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void _ResetDefaultForm() {


            linklblShowLicensesHistory.Enabled = false;
            linklblShowNewLicenseInfo.Enabled = false;
            btnRenew.Enabled = false;
            ctrlLicenseInfoWithFilter1.txtLicenseIDFocus();
        }
        private void frmRenewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultForm();
            lblCreatedBy.Text = clsGlobal.CurrentUserInfo._UserName;
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = lblApplicationDate.Text;
            lblApplicationFees.Text = clsApplicationType.GetApplicationTypeInfo((int)clsApplication.enApplicationType.RenewDrivingLicense).Fees.ToString();
        }
        private void linklblShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }
        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;
            lblOldLicenseID.Text = SelectedLicenseID.ToString();
            linklblShowLicensesHistory.Enabled = (SelectedLicenseID != -1);

            if (SelectedLicenseID == -1)
            {

                return;
            }



            lblExpirationDate.Text = DateTime.Now.AddYears(ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.DefaultValidityLength).ToShortDateString();
            lblLicenseFees.Text = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.ClassFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text.Trim()) + Convert.ToSingle(lblLicenseFees.Text.Trim())).ToString();
            txtNotes.Text = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.Notes;

            if (!ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {

                MessageBox.Show($"Selected License is not yet expiared, it will expire on: {ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.ExpirationDate.ToShortDateString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenew.Enabled = false;
                return;
            }

            if (!ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {

                MessageBox.Show($"Selected License is not active, choose another License.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenew.Enabled = false;
                return;
            }
            btnRenew.Enabled = true;
        }
        private void btnRenew_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to Renew the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsLicense NewLicense = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.RenewLicense(txtNotes.Text.Trim(), clsGlobal.CurrentUserInfo._UserID);

            if (NewLicense == null)
            {

                MessageBox.Show($"Failed to Renew License :-(", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show($"License Renewed Successfully with ID = [{NewLicense.LicenseID}]", "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _NewLicenseID = NewLicense.LicenseID;
            lblRenewedLicenseID.Text = _NewLicenseID.ToString();
            lblRenewAppID.Text = NewLicense.ApplicationID.ToString();

            linklblShowNewLicenseInfo.Enabled = true;
            btnRenew.Enabled = false;
            ctrlLicenseInfoWithFilter1.FilterEnabled = false;

        }

        private void linklblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmLicenseHistory(ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();

        }
    }
}
