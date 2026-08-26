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
    public partial class frmIssueDriverLicenseForTheFirstTime : Form
    {
        private clsLocalDrivingLicenseApplication LDLApp;
        private int _LocalDrivingLicenseApplicationID = -1;
        public frmIssueDriverLicenseForTheFirstTime(int LocalDrivingLicenseAppID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 560);
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseAppID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadData() {

            txtNotes.Focus();
            LDLApp = clsLocalDrivingLicenseApplication.FindLDLAppByLocalDrivingLicenseID(_LocalDrivingLicenseApplicationID);

            if (LDLApp == null) {

                MessageBox.Show("No Applicaiton with ID=" + _LocalDrivingLicenseApplicationID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if (!LDLApp.PassedAllTests()) {

                MessageBox.Show("Person Should Pass All Tests First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            int LicenseID = LDLApp.GetActiveLicenseID();
            if (LicenseID != -1)
            {

                MessageBox.Show("Person already has License before with License ID=" + LicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;

            }

            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingLicenseAppID(_LocalDrivingLicenseApplicationID);
        }
        private void frmIssueDriverLicenseForTheFirstTime_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            int LicenseID = LDLApp.IssueLicenseForTheFirstTime(txtNotes.Text.Trim(), clsGlobal.CurrentUserInfo._UserID);

            if (LicenseID != -1)
            {

                MessageBox.Show($"License Issued Successfully with License ID = [{LicenseID}]", "Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else {

                MessageBox.Show($"License was not Issued !", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
