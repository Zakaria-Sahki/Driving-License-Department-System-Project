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
    public partial class frmTestAppointments : Form
    {
        private DataTable _AppointmentsTable;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private int _LocalDrivingLicenseAppID = -1;
        public clsLocalDrivingLicenseApplication LocalDrivingLicenseApp;

        public frmTestAppointments(int LocalApplicationID, clsTestType.enTestType TestType)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(800, 660);
            _LocalDrivingLicenseAppID = LocalApplicationID;
            LocalDrivingLicenseApp = clsLocalDrivingLicenseApplication.FindLDLAppByLocalDrivingLicenseID(_LocalDrivingLicenseAppID);
            
            _TestTypeID = TestType;
        }

        private void _RefreshMainPartOfForm() {

            switch (_TestTypeID) {

                case clsTestType.enTestType.VisionTest:
                    pBIconOfTest.Image = Resources.Vision_512;
                    lblTitleOfTest.Text = "Vision Test Appointments";
                    this.Text = "Vision Test Appointments";
                    break;

                case clsTestType.enTestType.WrittenTest:
                    pBIconOfTest.Image = Resources.Written_Test_512;
                    lblTitleOfTest.Text = "Written Test Appointments";
                    this.Text = "Written Test Appointments";
                    break;

                case clsTestType.enTestType.StreetTest:
                    pBIconOfTest.Image = Resources.driving_test_512;
                    lblTitleOfTest.Text = "Street Test Appointments";
                    this.Text = "Street Test Appointments";
                    break;

                default:
                    pBIconOfTest.Image = Resources.Vision_512;
                    lblTitleOfTest.Text = "Error";
                    this.Text = "Error";
                    break;
            }
        }
        private void _LoadApplicationInfo() {

            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingLicenseAppID(_LocalDrivingLicenseAppID);
            _AppointmentsTable = clsTestAppointments.GetAllAppointmentsByLDLAppID_And_TestType(_LocalDrivingLicenseAppID, _TestTypeID);
            dgvTestAppointments.DataSource = _AppointmentsTable;
            lblRecordsCount.Text = dgvTestAppointments.Rows.Count.ToString();

            if (dgvTestAppointments.Rows.Count > 0) {

                dgvTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvTestAppointments.Columns[0].Width = 100;

                dgvTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvTestAppointments.Columns[1].Width = 200;

                dgvTestAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvTestAppointments.Columns[2].Width = 100;

                dgvTestAppointments.Columns[3].HeaderText = "Is Locked";
                dgvTestAppointments.Columns[3].Width = 100;
            }
        }
        private void frmTestAppointments_Load(object sender, EventArgs e)
        {
            _RefreshMainPartOfForm();
            _LoadApplicationInfo();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form frm = new frmScheduleTest(_LocalDrivingLicenseAppID, _TestTypeID, (int)dgvTestAppointments.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmTestAppointments_Load(null, null);
        }
        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmTakeTest((int)dgvTestAppointments.CurrentRow.Cells[0].Value, _TestTypeID);
            frm.ShowDialog();
            frmTestAppointments_Load(null, null);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindLDLAppByLocalDrivingLicenseID(_LocalDrivingLicenseAppID);


            if (localDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestTypeID))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            //---
            clsTest LastTest = localDrivingLicenseApplication.GetLastTestPerTestType(_TestTypeID);

            if (LastTest == null)
            {
                frmScheduleTest frm1 = new frmScheduleTest(_LocalDrivingLicenseAppID, _TestTypeID);
                frm1.ShowDialog();
                frmTestAppointments_Load(null, null);
                return;
            }

            //if person already passed the test s/he cannot retak it.

            if (LastTest.TestResult == true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            frmScheduleTest frm2 = new frmScheduleTest(LastTest.TestAppointmentInfo.LDL_AppID, _TestTypeID);
            frm2.ShowDialog();
            frmTestAppointments_Load(null, null);

        }
    }
}

