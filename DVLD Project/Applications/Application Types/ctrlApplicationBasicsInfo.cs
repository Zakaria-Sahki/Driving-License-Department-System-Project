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
    public partial class ctrlApplicationBasicsInfo : UserControl
    {
        public ctrlApplicationBasicsInfo()
        {
            InitializeComponent();
        }

        clsApplication _Application;

        private int _ApplicationID = -1;
        public int ApplicationID
        {
            get { return _ApplicationID; }
        }

        public void LoadApplicationInfo(int ApplicationID)
        {

            _Application = clsApplication.GetApplicationInfoByID(ApplicationID);
            if (_Application == null)
            {
                ResetApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _FillApplicationInfo();
        }
        private void _FillApplicationInfo()
        {
            _ApplicationID = _Application._ApplicationID;
            lblApplicationID.Text = _Application._ApplicationID.ToString();
            lblStatus.Text = _Application.StatusText;
            lblApplicationType.Text = _Application.ApplicationTypeInfo.Title;
            lblFees.Text = _Application._PaidFees.ToString();
            lblApplicantFullName.Text = _Application.ApplicantFullName;
            lblApplicationDate.Text = _Application._ApplicationDate.ToShortDateString();
            lblLastStatusDate.Text = _Application._LastStatusDate.ToShortDateString();
            lblCreatedUserName.Text = _Application.CreatedByUserInfo._UserName;
        }
        public void ResetApplicationInfo()
        {
            _ApplicationID = -1;

            lblApplicationID.Text = "[????]";
            lblStatus.Text = "[????]";
            lblApplicationType.Text = "[????]";
            lblFees.Text = "[????]";
            lblApplicantFullName.Text = "[????]";
            lblApplicationDate.Text = "[????]";
            lblLastStatusDate.Text = "[????]";
            lblCreatedUserName.Text = "[????]";

        }

        private void LinkLblViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmPersonDetails(_Application._ApplicantPersonID);
            frm.ShowDialog();
            LoadApplicationInfo(_ApplicationID);
        }
    }
}
