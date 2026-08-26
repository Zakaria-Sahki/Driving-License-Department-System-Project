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
    public partial class frmTakeTest : Form
    {
        private clsLocalDrivingLicenseApplication LocalDLApplication;
        private clsTestAppointments TestAppointment;
        private clsTest _Test;
        private int _TestID = -1;
        private int _TestAppointmentID;
        public clsTestType.enTestType _TestTypeID;

        public frmTakeTest(int TestAppointmentID, clsTestType.enTestType TestType)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(425, 630);
            _TestAppointmentID = TestAppointmentID;
            _TestTypeID = TestType;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {

            ctrlScheduledTest1.TestTypeID = _TestTypeID;
            ctrlScheduledTest1.LoadInfo(_TestAppointmentID);

            if (ctrlScheduledTest1.TestAppointmentID == -1)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;

            int _TestID = ctrlScheduledTest1.TestID;

            if (_TestID != -1)
            {

                _Test = clsTest.Find(_TestID);

                if (_Test.TestResult)
                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;

                lblUserMessage.Visible = true;
                txtNotes.Text = _Test.Notes;
                rbPass.Enabled = false;
                rbFail.Enabled = false;
            }
            else
            {

                _Test = new clsTest();
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            _Test.TestAppointmentID = _TestAppointmentID;
            _Test.TestResult = rbPass.Checked;
            _Test.Notes = txtNotes.Text.Trim();
            _Test.CreatedByUserID = clsGlobal.CurrentUserInfo._UserID;


            if (MessageBox.Show("Are you sure you want to save? after that you cannot change the Pass/Fail results after you save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;
            

            if (_Test.Save())
            {

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: data is not saved Successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
