using DVLD_Project.Properties;
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
    public partial class ctrlScheduleTest : UserControl
    {

        public enum enMode { AddNew, Update }
        private enMode _Mode;

        public enum enCreationMode { FirstTimeSchedule = 1, RetakeTestSchedule = 2}
        private enCreationMode _CreationMode;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private clsTestAppointments _TestAppointments;
        private int _TestAppointmentID = -1;
        private int _LocalDrivingLicenseApplicationID = -1;
        public clsTestType.enTestType TestTypeID {

            get { return _TestTypeID; }

            set {

                _TestTypeID = value;
                
                switch (_TestTypeID) {

                    case clsTestType.enTestType.VisionTest:
                        pictureBoxIcon.Image = Resources.Vision_512;
                        gBAll.Text = "Vision Test";
                        break;
                    case clsTestType.enTestType.WrittenTest:
                        pictureBoxIcon.Image = Resources.Written_Test_512;
                        gBAll.Text = "Written Test";
                        break;
                    case clsTestType.enTestType.StreetTest:
                        pictureBoxIcon.Image = Resources.driving_test_512;
                        gBAll.Text = "Street Test";
                        break;
                }
            }
        }

        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        public void LoadInfo(int LocalDrivingLicenseApplicationID, int TestAppointmentID = -1) {

            if (TestAppointmentID != -1)
                _Mode = enMode.Update;
            else
                _Mode = enMode.AddNew;

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = TestAppointmentID;

            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindLDLAppByLocalDrivingLicenseID(_LocalDrivingLicenseApplicationID);


            if (_LocalDrivingLicenseApplication == null) {

                MessageBox.Show($"Error: No Local Driving License Application with ID = [{_LocalDrivingLicenseApplicationID}]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;    
            }

            if (_LocalDrivingLicenseApplication.DoesAttendTestType(_TestTypeID))
                _CreationMode = enCreationMode.RetakeTestSchedule;
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;

            if (_CreationMode == enCreationMode.RetakeTestSchedule)
            {

                lblTitle.Text = "Schedule Retake Test";
                gBoxRetakeInfo.Enabled = true;
                lblRetakeAppFees.Text = clsApplicationType.GetApplicationTypeInfo((int)clsApplication.enApplicationType.RetakeTest).Fees.ToString();
            }
            else {

                lblTitle.Text = "Schedule Test";
                lblRetakeAppFees.Text = "0";
                gBoxRetakeInfo.Enabled = false;

            }
            
            lblLDL_AppID.Text = _LocalDrivingLicenseApplication.LDLAppID.ToString();
            lblicenseClass.Text = clsLicenseClass.FindLicenseClass(_LocalDrivingLicenseApplication.LicenseClassID).ClassName;
            lblFullName.Text = _LocalDrivingLicenseApplication.ApplicantFullName;
            lblTrial.Text = _LocalDrivingLicenseApplication.TotalTrialsPerTest(_TestTypeID).ToString();


            if (_Mode == enMode.AddNew)
            {

                lblFees.Text = clsTestType.FindTestByID((int)_TestTypeID).Fees.ToString();
                dtpAppointmentDate.MinDate = DateTime.Now;
                lblRetakeTestAppID.Text = "N/A";
                _TestAppointments = new clsTestAppointments();
            }
            else {

                if (!LoadTestAppointmentData())
                    return;
            }


            lblTotalFees.Text = (Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRetakeAppFees.Text)).ToString();

            if (!HandleActiveTestAppointmentConstraint())
                return;
            if (!HandleAppointmentLockedConstraint())
                return;
            if (!HandlePrviousTestConstraint())
                return;
            
        }
        private bool LoadTestAppointmentData() {

            _TestAppointments = clsTestAppointments.FindTestAppointment(_TestAppointmentID);
            
            if (_TestAppointments == null)
            {

                MessageBox.Show($"Error: No Test Appointment with ID = [{_TestAppointmentID}]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            lblFees.Text = _TestAppointments.PaidFees.ToString();

            if (DateTime.Compare(DateTime.Now, _TestAppointments.AppointmentDate) < 0)
                dtpAppointmentDate.MinDate = DateTime.Now;
            else
                dtpAppointmentDate.MinDate = _TestAppointments.AppointmentDate;


            dtpAppointmentDate.Value = _TestAppointments.AppointmentDate;


            if (_TestAppointments.RetakeTestAppID == -1)
            {

                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }
            else {

                lblRetakeTestAppID.Text = _TestAppointments.RetakeTestAppID.ToString(); ;
                lblRetakeAppFees.Text = _TestAppointments.RetakeTestAppInfo._PaidFees.ToString();
                gBoxRetakeInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
            }
            return true;
        }
        private bool HandleActiveTestAppointmentConstraint() {

            if (_Mode == enMode.AddNew && clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_LocalDrivingLicenseApplicationID, _TestTypeID))
            {

                MessageBox.Show("Person Already has an active appointment for this test, you cannot add new appointment", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtpAppointmentDate.Enabled = false;
                btnSave.Enabled = false;
                return false;
            }
            return true;
        }
        private bool HandleAppointmentLockedConstraint() {

            if (_TestAppointments.IsLocked)
            {
                lblAppointmentLockedMsg.Visible = true;
                btnSave.Enabled = false;
                dtpAppointmentDate.Enabled = false;
                lblAppointmentLockedMsg.Text = "Poerson already sat for the test, appointment locked.";
                return false;
            }
            else {

                lblAppointmentLockedMsg.Visible = false;
            }
                return true;
        }
        private bool HandlePrviousTestConstraint()
        {

            //we need to make sure that this person passed the prvious required test before apply to the new test.
            //person cannnot apply for written test unless s/he passes the vision test.
            //person cannot apply for street test unless s/he passes the written test.

            switch (TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    //in this case no required prvious test to pass.
                    lblAppointmentLockedMsg.Visible = false;

                    return true;

                case clsTestType.enTestType.WrittenTest:
                    //Written Test, you cannot sechdule it before person passes the vision test.
                    //we check if pass visiontest 1.
                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest))
                    {
                        lblAppointmentLockedMsg.Text = "Cannot Sechule, Vision Test should be passed first";
                        lblAppointmentLockedMsg.Visible = true;
                        btnSave.Enabled = false;
                        dtpAppointmentDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblAppointmentLockedMsg.Visible = false;
                        btnSave.Enabled = true;
                        dtpAppointmentDate.Enabled = true;
                    }


                    return true;

                case clsTestType.enTestType.StreetTest:

                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest))
                    {
                        lblAppointmentLockedMsg.Text = "Cannot Sechule, Written Test should be passed first";
                        lblAppointmentLockedMsg.Visible = true;
                        btnSave.Enabled = false;
                        dtpAppointmentDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblAppointmentLockedMsg.Visible = false;
                        btnSave.Enabled = true;
                        dtpAppointmentDate.Enabled = true;
                    }


                    return true;

            }
            return true;
        }
        private bool HandleRetakeApplication()
        {

            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {

                clsApplication Application = new clsApplication();

                Application._ApplicantPersonID = _LocalDrivingLicenseApplication._ApplicantPersonID;
                Application._ApplicationDate = DateTime.Now;
                Application._LastStatusDate = DateTime.Now;
                Application._ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application._CreatedByUserID = clsGlobal.CurrentUserInfo._UserID;
                Application._PaidFees = clsApplicationType.GetApplicationTypeInfo((int)clsApplication.enApplicationType.RetakeTest).Fees;
                Application._ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;

                if (!Application.Save()) {

                    _TestAppointments.RetakeTestAppID = -1;
                    MessageBox.Show("Failed to Create Application","Faild",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                _TestAppointments.RetakeTestAppID = Application._ApplicationID;
            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!HandleRetakeApplication())
                return;

            _TestAppointments.TestTypeID = _TestTypeID;
            _TestAppointments.LDL_AppID = _LocalDrivingLicenseApplicationID;
            _TestAppointments.AppointmentDate = dtpAppointmentDate.Value;
            _TestAppointments.PaidFees = Convert.ToSingle(lblFees.Text);
            _TestAppointments.CreatedByUserID = clsGlobal.CurrentUserInfo._UserID;

            if (_TestAppointments.Save())
            {

                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data is not saved successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
