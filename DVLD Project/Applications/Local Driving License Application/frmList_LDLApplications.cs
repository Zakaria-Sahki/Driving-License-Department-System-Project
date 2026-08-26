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
    public partial class frmList_LDLApplications : Form
    {
        static private DataTable _AllLocalAppTable;

        public frmList_LDLApplications()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1160, 560);
        }
        private void frmList_LDLApplications_Load(object sender, EventArgs e)
        {
            _AllLocalAppTable = clsLocalDrivingLicenseApplication.GetAll_LDLApplications();
            dgvLDLApplications.DataSource = _AllLocalAppTable;
            lblNumberOfRecords.Text = dgvLDLApplications.Rows.Count.ToString();

            if (dgvLDLApplications.Rows.Count > 0) {

                dgvLDLApplications.Columns[0].HeaderText = "LD.L AppID";
                dgvLDLApplications.Columns[0].Width = 100;

                dgvLDLApplications.Columns[1].HeaderText = "Driving Class";
                dgvLDLApplications.Columns[1].Width = 230;

                dgvLDLApplications.Columns[2].HeaderText = "National No.";
                dgvLDLApplications.Columns[2].Width = 100;

                dgvLDLApplications.Columns[3].HeaderText = "Full Name";
                dgvLDLApplications.Columns[3].Width = 230;

                dgvLDLApplications.Columns[4].HeaderText = "Application Date";
                dgvLDLApplications.Columns[4].Width = 130;

                dgvLDLApplications.Columns[5].HeaderText = "Passed Tests";
                dgvLDLApplications.Columns[5].Width = 100;

                dgvLDLApplications.Columns[6].HeaderText = "Status";
                dgvLDLApplications.Columns[6].Width = 80;
            }

            cbFilterBy.SelectedIndex = 0;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            txtFilterSearch.Visible = (cbFilterBy.Text != "None" && cbFilterBy.Text != "Status");
            cbStatusFilter.Visible = (cbFilterBy.Text == "Status");

            if (cbFilterBy.Text == "None") {

                _AllLocalAppTable.DefaultView.RowFilter = "";
                lblNumberOfRecords.Text = dgvLDLApplications.Rows.Count.ToString();
            }

            if (cbFilterBy.Text == "Status")
                cbStatusFilter.SelectedIndex = 0;

            if (cbFilterBy.Text != "None" && cbFilterBy.Text != "Status")
                txtFilterSearch.Text = "";

        }
        private void txtFilterSearch_TextChanged(object sender, EventArgs e)
        {
            string FilterName = "";

            switch (cbFilterBy.Text)
            {

                case "None": 
                    FilterName = "None";
                    break;
                case "L.D.LAppID": // this is the caption of header 
                    FilterName = "LocalDrivingLicenseApplicationID"; // the name of column in database
                    break;
                case "National No.":
                    FilterName = "NationalNo";
                    break;
                case "Full Name":
                    FilterName = "FullName";
                    break;
            }

            if (txtFilterSearch.Text.Trim() == "" || FilterName == "None")
            {

                _AllLocalAppTable.DefaultView.RowFilter = "";
                lblNumberOfRecords.Text = dgvLDLApplications.Rows.Count.ToString();
                return;
            }

            if (FilterName == "NationalNo" || FilterName == "FullName")
            {

                _AllLocalAppTable.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterName, txtFilterSearch.Text.Trim());
            }
            else
            {

                _AllLocalAppTable.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterName, txtFilterSearch.Text.Trim());

            }

            lblNumberOfRecords.Text = dgvLDLApplications.Rows.Count.ToString();
        }
        private void txtFilterSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "L.D.LAppID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void cbStatusFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStatusFilter.Text == "New")
                _AllLocalAppTable.DefaultView.RowFilter = string.Format("[{0}] = '{1}'", "Status", cbStatusFilter.Text.Trim());
            else if (cbStatusFilter.Text == "Cancelled")
                _AllLocalAppTable.DefaultView.RowFilter = string.Format("[{0}] = '{1}'", "Status", cbStatusFilter.Text.Trim());
            else if (cbStatusFilter.Text == "Completed")
                _AllLocalAppTable.DefaultView.RowFilter = string.Format("[{0}] = '{1}'", "Status", cbStatusFilter.Text.Trim());
            else
                _AllLocalAppTable.DefaultView.RowFilter = "";

            lblNumberOfRecords.Text = dgvLDLApplications.Rows.Count.ToString();
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditLocalDLApplication();
            frm.ShowDialog();
            frmList_LDLApplications_Load(null, null);
        }
        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to delete this LdL Application with ID = [{(int)dgvLDLApplications.CurrentRow.Cells[0].Value}]", "Delete Application", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                clsLocalDrivingLicenseApplication LocalDrivingLicenseApp = clsLocalDrivingLicenseApplication.FindLDLAppByLocalDrivingLicenseID((int)dgvLDLApplications.CurrentRow.Cells[0].Value);
                if (LocalDrivingLicenseApp != null) {

                    if (LocalDrivingLicenseApp.DeleteLocalDrivingLicenseApp())
                    {

                        MessageBox.Show("The Application Deleted successfully.", "Successfull Operation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmList_LDLApplications_Load(null, null);
                    }
                    else
                    {

                        MessageBox.Show("Could not Delete Application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure you want to cancel this LdL Application with ID = [{(int)dgvLDLApplications.CurrentRow.Cells[0].Value}]", "Cancel Application", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {

                clsLocalDrivingLicenseApplication LDLApp = clsLocalDrivingLicenseApplication.FindLDLAppByLocalDrivingLicenseID((int)dgvLDLApplications.CurrentRow.Cells[0].Value);

                if (LDLApp != null) {

                    if (LDLApp.Cancel())
                    {

                        MessageBox.Show("The Application Canceled successfully.", "Successfull Operation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmList_LDLApplications_Load(null, null);
                    }
                    else
                    {

                        MessageBox.Show("Could not Cancel Application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditLocalDLApplication((int)dgvLDLApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmList_LDLApplications_Load(null, null);
        }
        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowLocalDrivingLicenseApplicationInfo((int)dgvLDLApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmList_LDLApplications_Load(null, null);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseAppID = (int)dgvLDLApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDLApp = clsLocalDrivingLicenseApplication.FindLDLAppByLocalDrivingLicenseID(LocalDrivingLicenseAppID);

            if (LocalDLApp._ApplicationStatus == clsApplication.enApplicationStatus.Cancelled) {

                editApplicationToolStripMenuItem.Enabled = false;
                deleteApplicationToolStripMenuItem.Enabled = false;
                cancelApplicationToolStripMenuItem.Enabled = false;
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                sechduleTestsToolStripMenuItem.Enabled = false;
                showApplicationDetailsToolStripMenuItem.Enabled = true;
                showLicenseToolStripMenuItem.Enabled = false;
                showPersonLicneseHistoryToolStripMenuItem.Enabled = true;
                return;
            }

            if (LocalDLApp._ApplicationStatus == clsApplication.enApplicationStatus.Completed) {

                editApplicationToolStripMenuItem.Enabled = false;
                deleteApplicationToolStripMenuItem.Enabled = false;
                cancelApplicationToolStripMenuItem.Enabled = false;
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                sechduleTestsToolStripMenuItem.Enabled = false;
                showLicenseToolStripMenuItem.Enabled = true;
                showApplicationDetailsToolStripMenuItem.Enabled = true;
                showPersonLicneseHistoryToolStripMenuItem .Enabled = true;
                return;
            }

            editApplicationToolStripMenuItem.Enabled = true;
            deleteApplicationToolStripMenuItem.Enabled = true;
            cancelApplicationToolStripMenuItem.Enabled = true;
            showLicenseToolStripMenuItem.Enabled = false;


            bool IsVisionTestPassed = LocalDLApp.DoesPassTestType(clsTestType.enTestType.VisionTest);
            bool IsWrittenTestPassed = LocalDLApp.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            bool IsStreetTestPassed = LocalDLApp.DoesPassTestType(clsTestType.enTestType.StreetTest);



            visionTestToolStripMenuItem.Enabled = !IsVisionTestPassed;
            writtenTestToolStripMenuItem.Enabled = !IsWrittenTestPassed && LocalDLApp.DoesPassPreviousTest(clsTestType.enTestType.WrittenTest);
            streetTestToolStripMenuItem.Enabled = !IsStreetTestPassed && LocalDLApp.DoesPassPreviousTest(clsTestType.enTestType.StreetTest);

            sechduleTestsToolStripMenuItem.Enabled = !(IsVisionTestPassed && IsWrittenTestPassed && IsStreetTestPassed);
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (IsVisionTestPassed && IsWrittenTestPassed && IsStreetTestPassed) && (LocalDLApp._ApplicationStatus == clsApplication.enApplicationStatus.New);

        }

        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmTestAppointments((int)dgvLDLApplications.CurrentRow.Cells[0].Value, clsTestType.enTestType.VisionTest);
            frm.ShowDialog();
            frmList_LDLApplications_Load(null, null);
        }
        private void writtenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmTestAppointments((int)dgvLDLApplications.CurrentRow.Cells[0].Value, clsTestType.enTestType.WrittenTest);
            frm.ShowDialog();
            frmList_LDLApplications_Load(null, null);
        }
        private void streetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmTestAppointments((int)dgvLDLApplications.CurrentRow.Cells[0].Value, clsTestType.enTestType.StreetTest);
            frm.ShowDialog();
            frmList_LDLApplications_Load(null, null);
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmIssueDriverLicenseForTheFirstTime((int)dgvLDLApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmList_LDLApplications_Load(null, null);
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLDLApplications.CurrentRow.Cells[0].Value;
            int LicenseID = clsLocalDrivingLicenseApplication.FindLDLAppByLocalDrivingLicenseID(LocalDrivingLicenseApplicationID).GetActiveLicenseID();

            if (LicenseID != -1)
            {
                Form frm = new frmLicenseInfo(LicenseID);
                frm.ShowDialog();

            }
            else
            {
                MessageBox.Show("No License Found!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void showPersonLicneseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LDLApp = clsLocalDrivingLicenseApplication.FindLDLAppByLocalDrivingLicenseID((int)dgvLDLApplications.CurrentRow.Cells[0].Value);
            
            Form frm = new frmLicenseHistory(LDLApp.PersonInfo._PersonID);
            frm.ShowDialog();
            frmList_LDLApplications_Load(null, null);
        }
    }
}
