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
    public partial class frmNewInternationalLicenseApplication : Form
    {
        private int _LicenseID = -1;
        private int _InternationaLicenseID = -1;
        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1040, 555);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to issue an interrnational License for this person ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            clsInternationalLicenses InternationalLicense = new clsInternationalLicenses();
            
            InternationalLicense._ApplicantPersonID = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;
            InternationalLicense._ApplicationDate = DateTime.Now;
            InternationalLicense._ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            InternationalLicense._LastStatusDate = DateTime.Now;
            InternationalLicense._PaidFees = clsApplicationType.GetApplicationTypeInfo((int)clsApplication.enApplicationType.NewInternationalLicense).Fees;
            InternationalLicense._CreatedByUserID = clsGlobal.CurrentUserInfo._UserID;

            InternationalLicense.DriverID = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            InternationalLicense.IsActive = true;

            if (InternationalLicense.Save())
            {

                MessageBox.Show($"International license Issued Successfully with ID = {InternationalLicense.InternationalLicenseID}", "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _InternationaLicenseID = InternationalLicense.InternationalLicenseID;
                lblInternationalLicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();
                lblInternationalAppID.Text = InternationalLicense._ApplicationID.ToString();
                linklblShowLicenseInfo.Enabled = true;
                ctrlLicenseInfoWithFilter1.FilterEnabled = false;
                btnIssue.Enabled = false;
                return;
            }
            else {

                MessageBox.Show("Faild to Issue International License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }
        private void linklblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmInternationalLicenseInfo(_InternationaLicenseID);
            frm.ShowDialog();
        }
        private void linklblShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmLicenseHistory(ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }
        private void frmNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToShortDateString();
            lblCreatedBy.Text = clsGlobal.CurrentUserInfo._UserName;
            lblFees.Text = clsApplicationType.GetApplicationTypeInfo((int)clsApplication.enApplicationType.NewInternationalLicense).Fees.ToString();
        }
        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _LicenseID = obj;
            lblLocalLicenseID.Text = _LicenseID.ToString();
            linklblShowLicenseHistory.Enabled = (_LicenseID != -1);

            if (_LicenseID == -1) {

                return;
            }

            if (ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassID != 3)
            {

                MessageBox.Show($"Selected License should be Class 3., select another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssue.Enabled = false;
                return;
            }

            int InternationalLicenseID = clsInternationalLicenses.GetActiveInternationalLicenseIDByDriverID(ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID);

            if (InternationalLicenseID != -1) {

                MessageBox.Show($"Person already have an active international license with ID = {InternationalLicenseID}", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                linklblShowLicenseInfo.Enabled = true;
                _InternationaLicenseID = InternationalLicenseID; ;
                btnIssue.Enabled = false;
                return;
            }

            btnIssue.Enabled = true;
        }
    }
}
