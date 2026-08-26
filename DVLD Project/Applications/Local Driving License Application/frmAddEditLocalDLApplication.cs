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
    public partial class frmAddEditLocalDLApplication : Form
    {
        private enum enMode { AddNew, Update};
        private enMode Mode;

        private int _LDLAppID;
        private int _SelectedPersonID = -1;
        clsLocalDrivingLicenseApplication LdlApplication;

        public frmAddEditLocalDLApplication()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(820, 630);
            Mode = enMode.AddNew;
        }
        public frmAddEditLocalDLApplication(int LDLApplicationID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(820, 630);
            Mode = enMode.Update;
            _LDLAppID = LDLApplicationID;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }

        // --------------------------------------

        private void _FillLicenseClassesInComboBox() {

            DataTable LicenseClasses = clsLicenseClass.GetAllLicenseClasses();
            foreach (DataRow Row in LicenseClasses.Rows) {

                cbLicenseClass.Items.Add(Row["ClassName"]);
            }
        }
        private void frmAddEditLocalDLApplication_Load(object sender, EventArgs e)
        {
            _RefreshDefaultValues();

            if (Mode == enMode.Update)
                LoadData();
        }
        private void _RefreshDefaultValues() {

            _FillLicenseClassesInComboBox();

            if (Mode == enMode.AddNew)
            {

                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                LdlApplication = new clsLocalDrivingLicenseApplication();

                tPApplicationInfo.Enabled = false;
                btnSave.Enabled = false;
                cbLicenseClass.SelectedIndex = 2;

                lblApplicationFees.Text = clsApplicationType.GetApplicationTypeInfo(Convert.ToInt32(clsApplication.enApplicationType.NewDrivingLicense)).Fees.ToString("0.##");
                lblCreatedBy.Text = clsGlobal.CurrentUserInfo._UserName;
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            }
            else {

                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";
                tPApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }
        }
        private void LoadData() {

            ctrlPersonCardWithFilter1.FilterEnabled = false;
            LdlApplication = clsLocalDrivingLicenseApplication.FindLDLAppByLocalDrivingLicenseID(_LDLAppID);

            if (LdlApplication == null) {

                MessageBox.Show($"No Application with ID = [{_LDLAppID}]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            ctrlPersonCardWithFilter1.LoadData(LdlApplication._ApplicantPersonID);
            lblDLAppID.Text = LdlApplication.LDLAppID.ToString();
            lblCreatedBy.Text = LdlApplication.CreatedByUserInfo._UserName;
            lblApplicationFees.Text = LdlApplication._PaidFees.ToString();
            lblApplicationDate.Text = LdlApplication._ApplicationDate.ToShortDateString();
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClass.FindLicenseClass(LdlApplication.LicenseClassID).ClassName);
        }

        // --------------------------------------

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (Mode == enMode.Update || _SelectedPersonID != -1) {

                btnSave.Enabled = true;
                tPApplicationInfo.Enabled = true;
                tabControl1.SelectedIndex = 1; // Login info tab
                return;
            }
            else {

                MessageBox.Show("Please Select a person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ctrlPersonCardWithFilter1.FilterFocus();
                return;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicneseClassID = clsLicenseClass.FindLicenseClass(cbLicenseClass.Text).LicenseClassID;
            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(_SelectedPersonID, clsApplication.enApplicationType.NewDrivingLicense, LicneseClassID);

            if (ActiveApplicationID != -1) {

                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return;
            }


            //check if user already have issued license of the same driving  class.
            
            //if (clsLicense.IsLicenseExistByPersonID(ctrlPersonCardWithFilter1.PersonID, LicenseClassID))
            //{

            //    MessageBox.Show("Person already have a license with the same applied driving class, Choose diffrent driving class", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}


            LdlApplication._ApplicantPersonID = _SelectedPersonID;
            LdlApplication._ApplicationDate = DateTime.Now;
            LdlApplication._ApplicationTypeID = 1;
            LdlApplication._ApplicationStatus = clsApplication.enApplicationStatus.New;
            LdlApplication._LastStatusDate = DateTime.Now;
            LdlApplication._PaidFees = Convert.ToSingle(lblApplicationFees.Text.Trim());
            LdlApplication._CreatedByUserID = clsGlobal.CurrentUserInfo._UserID;
            LdlApplication.LicenseClassID = LicneseClassID;




            if (LdlApplication.Save()) { 

                lblDLAppID.Text = LdlApplication.LDLAppID.ToString();
                Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {

                MessageBox.Show("Error: data is not saved Successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void frmAddEditLocalDLApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}
