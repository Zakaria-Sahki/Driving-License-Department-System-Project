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
    public partial class frmDetainedLicensesList : Form
    {
        private DataTable _dtAllDetainedLicense;

        public frmDetainedLicensesList()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1105, 520);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnDetain_Click(object sender, EventArgs e)
        {
            Form frm = new frmDetainLicense();
            frm.ShowDialog();
            frmDetainedLicensesList_Load(null, null);
        }
        private void btnRelease_Click(object sender, EventArgs e)
        {
            Form frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
            frmDetainedLicensesList_Load(null, null);
        }
        private void frmDetainedLicensesList_Load(object sender, EventArgs e)
        {

            _dtAllDetainedLicense = clsDetainedLicense.GetAllDetainedLicenses();
            dgvDetainedLicenses.DataSource = _dtAllDetainedLicense;
            lblCountRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();

            if (dgvDetainedLicenses.Rows.Count > 0)
            {

                dgvDetainedLicenses.Columns[0].HeaderText = "D.ID";
                dgvDetainedLicenses.Columns[0].Width = 70;

                dgvDetainedLicenses.Columns[1].HeaderText = "L.ID";
                dgvDetainedLicenses.Columns[1].Width = 70;

                dgvDetainedLicenses.Columns[2].HeaderText = "D.Date";
                dgvDetainedLicenses.Columns[2].Width = 120;

                dgvDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvDetainedLicenses.Columns[3].Width = 80;

                dgvDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvDetainedLicenses.Columns[4].Width = 90;

                dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicenses.Columns[5].Width = 120;

                dgvDetainedLicenses.Columns[6].HeaderText = "N.No.";
                dgvDetainedLicenses.Columns[6].Width = 70;

                dgvDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvDetainedLicenses.Columns[7].Width = 200;

                dgvDetainedLicenses.Columns[8].HeaderText = "Release App.ID";
                dgvDetainedLicenses.Columns[8].Width = 120;
            }

            cbFilterBy.SelectedIndex = 0;
        }
        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            clsLicense License = clsLicense.FindLicenseByID(LicenseID);

            Form frm = new frmPersonDetails(License.DriverInfo.PersonID);
            frm.ShowDialog();
            frmDetainedLicensesList_Load(null, null);
        }
        private void showLicensesDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            Form frm = new frmLicenseInfo(LicenseID);
            frm.ShowDialog();
            frmDetainedLicensesList_Load(null, null);
        }
        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            clsLicense License = clsLicense.FindLicenseByID(LicenseID);

            Form frm = new frmLicenseHistory(License.DriverInfo.PersonID);
            frm.ShowDialog();
            frmDetainedLicensesList_Load(null, null);
        }
        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            Form frm = new frmReleaseDetainedLicense(LicenseID);
            frm.ShowDialog();
            frmDetainedLicensesList_Load(null, null);

        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

            _dtAllDetainedLicense.DefaultView.RowFilter = "";
            lblCountRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
            txtFilterSearch.Text = string.Empty;
            

            if (cbFilterBy.SelectedIndex == 0 || cbFilterBy.SelectedIndex == 2)
                txtFilterSearch.Visible = false;

            cbIsReleasedFilter.Visible = (cbFilterBy.SelectedIndex == 2);

            if (cbFilterBy.SelectedIndex == 2) {

                cbIsReleasedFilter.SelectedIndex = 0;
            }


            if ((cbFilterBy.SelectedIndex != 0 && cbFilterBy.SelectedIndex != 2)) {

                txtFilterSearch.Visible = true;
                txtFilterSearch.Focus();
            }
        }
        private void cbIsReleasedFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (cbIsReleasedFilter.Text) {

                case "All":
                    _dtAllDetainedLicense.DefaultView.RowFilter = "";
                    break;
                case "Yes":
                    _dtAllDetainedLicense.DefaultView.RowFilter = string.Format("[{0}] = {1}", "IsReleased", "1");
                    break;
                case "No":
                    _dtAllDetainedLicense.DefaultView.RowFilter = string.Format("[{0}] = {1}", "IsReleased", "0");
                    break;
            }
            lblCountRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
        }
        private void txtFilterSearch_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFilterBy.Text) {

                case "None":
                    FilterColumn = "None";
                    break;

                case "Detain ID":
                    FilterColumn = "DetainID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Release Application ID":
                    FilterColumn = "ReleaseApplicationID";
                    break;

            }

            if (FilterColumn == "None" || txtFilterSearch.Text.Trim() == "") {

                _dtAllDetainedLicense.DefaultView.RowFilter = "";
                lblCountRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "DetainID" || FilterColumn == "ReleaseApplicationID")
                _dtAllDetainedLicense.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterSearch.Text.Trim());
            else
                _dtAllDetainedLicense.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterSearch.Text.Trim());

            lblCountRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
        }
        private void txtFilterSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id is selected.
            if (cbFilterBy.Text == "Detain ID" || cbFilterBy.Text == "Release Application ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void cmsDetainedLicenses_Opening(object sender, CancelEventArgs e)
        {
            bool IsReleased = (bool)dgvDetainedLicenses.CurrentRow.Cells[3].Value;

            releaseDetainedLicenseToolStripMenuItem.Enabled = !IsReleased;
        }
    }
}
