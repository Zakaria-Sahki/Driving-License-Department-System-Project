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
    public partial class frmReplacementLicenseForDamageOrLost : Form
    {

        private int _NewLicenseID = -1;
        private clsApplication.enApplicationType ApplicationType;
        private clsLicense.enIssueReason IssueReason;

        public frmReplacementLicenseForDamageOrLost()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1050, 500);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void CheckApplicationType() {

            if (rbDamagedLicense.Checked)
            {

                ApplicationType = clsApplication.enApplicationType.ReplaceDamagedDrivingLicense;
                IssueReason = clsLicense.enIssueReason.DamagedReplacement;
            }
            else {

                ApplicationType = clsApplication.enApplicationType.ReplaceLostDrivingLicense;
                IssueReason = clsLicense.enIssueReason.LostReplacement;
            }
            this.Text = clsApplicationType.GetApplicationTypeInfo((int)ApplicationType).Title;
            lblAppFees.Text = clsApplicationType.GetApplicationTypeInfo((int)ApplicationType).Fees.ToString();
        }
        private void frmReplacementLicenseForDamageOrLost_Load(object sender, EventArgs e)
        {

            CheckApplicationType();
            lblAppDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedBy.Text = clsGlobal.CurrentUserInfo._UserName;

            linklblShowLicensesHistory.Enabled = false;
            linklblShowNewLicenseInfo.Enabled = false;
            btnIssueReplacement.Enabled = false;
        }
        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {

            int SelectedLicenseID = obj;

            lblOldLicenseID.Text = SelectedLicenseID.ToString();
            linklblShowLicensesHistory.Enabled = (SelectedLicenseID != -1);

            if (SelectedLicenseID == -1) {

                return;
            }

            // don't allow a replacement if is expired.
            if (ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired()) {

                MessageBox.Show($"Selected License is Expired, choose another License.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueReplacement.Enabled = false;
                return;
            }

            // don't allow a replacement if is not active.
            if (!ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive) {

                MessageBox.Show($"Selected License is not active, choose another License.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueReplacement.Enabled = false;
                return;
            }

            btnIssueReplacement.Enabled = true;
        }
        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            CheckApplicationType();
        }
        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            CheckApplicationType();
        }
        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue a Replacement for the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsLicense NewLicense = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.ReplacementLicense(IssueReason, clsGlobal.CurrentUserInfo._UserID);

            if (NewLicense == null) {

                MessageBox.Show($"Failed to issue a Replacement for this License :-(", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _NewLicenseID = NewLicense.LicenseID;
            lblReplacedLicenseID.Text = _NewLicenseID.ToString();
            lblRApplicationID.Text = NewLicense.ApplicationID.ToString();
            MessageBox.Show($"License Replaced Successfully with ID = {_NewLicenseID}", "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssueReplacement.Enabled = false;
            ctrlLicenseInfoWithFilter1.FilterEnabled = false;
            gbReplacementFor.Enabled = false;
            linklblShowNewLicenseInfo.Enabled = true;
        }
        private void linklblShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }

        private void linklblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmLicenseHistory(ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }
    }
}
