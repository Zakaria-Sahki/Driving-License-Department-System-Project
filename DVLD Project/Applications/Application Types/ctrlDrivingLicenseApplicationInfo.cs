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
    public partial class ctrlDrivingLicenseApplicationInfo : UserControl
    {

        private int _DLAppID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication1;
        public int DlAppID {

            get { return _DLAppID; }
        }

        public ctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        public void LoadApplicationInfoByLocalDrivingLicenseAppID(int LocalDLAppID) {

            _LocalDrivingLicenseApplication1 = clsLocalDrivingLicenseApplication.FindLDLAppByLocalDrivingLicenseID(LocalDLAppID);

            if (_LocalDrivingLicenseApplication1 == null) {

                _ResetLocalDrivingLicenseApplicationInfo();
                MessageBox.Show($"No application with local app ID = [{LocalDLAppID}].", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillLocalDrivingLicenseApplicationInfo();
        }
        public void LoadApplicationInfoByApplicationID(int ApplicationID) {

            _LocalDrivingLicenseApplication1 = clsLocalDrivingLicenseApplication.FindLDLAppByApplicationID(ApplicationID);

            if (_LocalDrivingLicenseApplication1 == null)
            {

                _ResetLocalDrivingLicenseApplicationInfo();
                MessageBox.Show($"No application with AppID = [{ApplicationID}].", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillLocalDrivingLicenseApplicationInfo();
        }
        private void _ResetLocalDrivingLicenseApplicationInfo() {

            _DLAppID = -1;
            lblDlAppID.Text = "[????]";
            lblLicenseClass.Text = "[????]";
            lblPassedTests.Text = "[????]";
            ctrlApplicationBasicsInfo1.ResetApplicationInfo();

        }
        private void _FillLocalDrivingLicenseApplicationInfo()
        {

            //_LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();

            //incase there is license enable the show link.
            //llShowLicenceInfo.Enabled = (_LicenseID != -1);


            lblDlAppID.Text = _LocalDrivingLicenseApplication1.LDLAppID.ToString();
            lblLicenseClass.Text = clsLicenseClass.FindLicenseClass(_LocalDrivingLicenseApplication1.LicenseClassID).ClassName;
            //lblPassedTests.Text = _LocalDrivingLicenseApplication1.GetPassedTestCount().ToString() + "/3";
            lblPassedTests.Text = "0/3";
            ctrlApplicationBasicsInfo1.LoadApplicationInfo(_LocalDrivingLicenseApplication1._ApplicationID);
        }
        private void LinklblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Still not implemented!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
    }
}
