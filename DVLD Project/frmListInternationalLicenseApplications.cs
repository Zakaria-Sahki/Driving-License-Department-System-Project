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
    public partial class frmListInternationalLicenseApplications : Form
    {
        private DataTable _dtInternationalLiceses;

        public frmListInternationalLicenseApplications()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(860, 500);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnNewInternationalLicense_Click(object sender, EventArgs e)
        {
            Form frm = new frmNewInternationalLicenseApplication();
            frm.ShowDialog();
            frmListInternationalLicenseApplications_Load(null, null);
        }
        private void frmListInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtInternationalLiceses = clsInternationalLicenses.GetAllInternationalLicenses();
            dgvInternationalLicenses.DataSource = _dtInternationalLiceses;
            lblCountRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
            cbFilterBy.SelectedIndex = 0;

            if (dgvInternationalLicenses.Rows.Count > 0) {

                dgvInternationalLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenses.Columns[0].Width = 100;

                dgvInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[1].Width = 100;

                dgvInternationalLicenses.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicenses.Columns[2].Width = 80;

                dgvInternationalLicenses.Columns[3].HeaderText = "L.License ID";
                dgvInternationalLicenses.Columns[3].Width = 100;

                dgvInternationalLicenses.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[4].Width = 120;

                dgvInternationalLicenses.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[5].Width = 120;

                dgvInternationalLicenses.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[6].Width = 100;
            }
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            _dtInternationalLiceses.DefaultView.RowFilter = "";
            lblCountRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
            

            if (cbFilterBy.Text == "None" || cbFilterBy.Text == "Is Active")
                txtFilterSearch.Visible = false;

            cbIsActiveFilter.Visible = (cbFilterBy.Text == "Is Active");

            if (cbFilterBy.Text == "Is Active")
                cbIsActiveFilter.SelectedIndex = 0;


            if (cbFilterBy.Text != "None" && cbFilterBy.Text != "Is Active") {

                txtFilterSearch.Visible = true;
                txtFilterSearch.Text = string.Empty;
                txtFilterSearch.Focus();
            }
                
        }
        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvInternationalLicenses.CurrentRow.Cells[2].Value;
            int PersonID = clsDriver.FindDriver(DriverID).PersonID;
            Form frm = new frmPersonDetails(PersonID);
            frm.ShowDialog();
            frmListInternationalLicenseApplications_Load(null, null);
        }
        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int InternationalLicenseID = (int)dgvInternationalLicenses.CurrentRow.Cells[0].Value;
            Form frm = new frmInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();
            frmListInternationalLicenseApplications_Load(null, null);
        }
        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvInternationalLicenses.CurrentRow.Cells[2].Value;
            int PersonID = clsDriver.FindDriver(DriverID).PersonID;
            Form frm = new frmLicenseHistory(PersonID);
            frm.ShowDialog();
            frmListInternationalLicenseApplications_Load(null, null);
        }
        private void cbIsActiveFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbIsActiveFilter.Text) {

                case "All":
                    _dtInternationalLiceses.DefaultView.RowFilter = "";
                    break;

                case "Yes":
                    _dtInternationalLiceses.DefaultView.RowFilter = string.Format("[{0}] = {1}", "IsActive", "1");

                    break;

                case "No":
                    _dtInternationalLiceses.DefaultView.RowFilter = string.Format("[{0}] = {1}", "IsActive", "0");
                    break;

                default:
                    _dtInternationalLiceses.DefaultView.RowFilter = "";
                    break;
            }

            lblCountRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();

        }
        private void txtFilterSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void txtFilterSearch_TextChanged(object sender, EventArgs e)
        {

            string FilterColumn = "";
            switch (cbFilterBy.Text) {

                case "None":
                    FilterColumn = "None";
                    break;

                case "International License ID":
                    FilterColumn = "InternationalLicenseID";
                    break;

                case "Application ID":
                    FilterColumn = "ApplicationID";
                    break;

                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "Local License ID":
                    FilterColumn = "IssuedUsingLocalLicenseID";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }

            if (FilterColumn == "None" || txtFilterSearch.Text.Trim() == "") {

                _dtInternationalLiceses.DefaultView.RowFilter = "";
                lblCountRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
                return;
            }

            _dtInternationalLiceses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterSearch.Text.Trim());
            lblCountRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
        }
    }
}
