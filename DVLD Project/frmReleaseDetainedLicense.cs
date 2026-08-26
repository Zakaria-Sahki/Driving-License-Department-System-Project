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
    public partial class frmReleaseDetainedLicense : Form
    {
        private int _LicenseID = -1;

        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1065, 540);
        }
        public frmReleaseDetainedLicense(int LicenseID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1065, 540);
            _LicenseID = LicenseID;
            ctrlLicenseInfoWithFilter1.LoadLicenseInfo(_LicenseID);
            ctrlLicenseInfoWithFilter1.FilterEnabled = false;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
        private void ctrlLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _LicenseID = obj;
            lblLicenseID.Text = _LicenseID.ToString();
            linklblShowLicenseHistory.Enabled = (_LicenseID != -1);

            if (_LicenseID == -1)
            {

                return;
            }

            if (!ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {

                MessageBox.Show("Selected License is not detained, Choose another one.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRelease.Enabled = false;
                return;
            }

            lblApplicationFees.Text = clsApplicationType.GetApplicationTypeInfo((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).Fees.ToString();


            lblDetainID.Text = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.DetainID.ToString();
            lblDetainDate.Text = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.DetainDate.ToShortDateString();
            lblCreatedBy.Text = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.CreatedByUserInfo._UserName;
            lblFineFees.Text = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.FineFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblFineFees.Text) + Convert.ToSingle(lblApplicationFees.Text)).ToString();


            btnRelease.Enabled = true;
        }
        private void btnRelease_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to release this detained license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int ApplicationID = -1;
            bool IsReleased = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.ReleaseDetainLicense(clsGlobal.CurrentUserInfo._UserID, ref ApplicationID);

            if (!IsReleased)
            {
                MessageBox.Show("Faild to release the Detain License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Detained License released Successfully ", "Detained License Released", MessageBoxButtons.OK, MessageBoxIcon.Information);

            lblApplicationID.Text = ApplicationID.ToString();

            ctrlLicenseInfoWithFilter1.FilterEnabled = false;
            btnRelease.Enabled = false;
            linklblShowLicenseInfo.Enabled = true;


        }
        private void frmReleaseDetainedLicense_Activated(object sender, EventArgs e)
        {
            ctrlLicenseInfoWithFilter1.txtLicenseIDFocus();
        }



        //private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        //{
        //    lblLicenseID.Text = _LicenseID.ToString();
        //    linklblShowLicenseHistory.Enabled = (_LicenseID != -1);

        //    if (_LicenseID == -1)
        //    {
        //        return;
        //    }

            

        //    if (!ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
        //    {

        //        MessageBox.Show("Selected License is not detained, Choose another one.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        btnRelease.Enabled = false;
        //        return;
        //    }

            
        //    lblDetainID.Text = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.DetainID.ToString();
        //    lblDetainDate.Text = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.DetainDate.ToShortDateString();
        //    lblCreatedBy.Text = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.CreatedByUserInfo._UserName;
        //    lblFineFees.Text = ctrlLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.FineFees.ToString();
        //    lblApplicationFees.Text = clsApplicationType.GetApplicationTypeInfo((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).Fees.ToString();
        //    lblTotalFees.Text = (Convert.ToSingle(lblFineFees.Text) + Convert.ToSingle(lblApplicationFees.Text)).ToString();

        //    btnRelease.Enabled = true;

        //}

    }
}
