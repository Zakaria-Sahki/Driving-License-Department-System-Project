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
    public partial class frmDriversList : Form
    {
        private DataTable DriversTable;

        public frmDriversList()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(960, 550);
        }

        private void frmDriversList_Load(object sender, EventArgs e)
        {
            DriversTable = clsDriver.GetAllDrivers();
            dgvDrivers.DataSource = DriversTable;
            lblCountRecords.Text = dgvDrivers.Rows.Count.ToString();

            if (dgvDrivers.Rows.Count > 0)
            {

                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[0].Width = 80;

                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[1].Width = 80;

                dgvDrivers.Columns[2].HeaderText = "National No.";
                dgvDrivers.Columns[2].Width = 90;

                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[3].Width = 250;

                dgvDrivers.Columns[4].HeaderText = "Date";
                dgvDrivers.Columns[4].Width = 150;

                dgvDrivers.Columns[5].HeaderText = "Active Licenses";
                dgvDrivers.Columns[5].Width = 150;
            }
            cbFilterBy.SelectedIndex = 0;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "None") { 
                
                txtSearchFilter.Visible = false;
            }
            else if (cbFilterBy.Text != "None") {

                txtSearchFilter.Visible = true;
            }
            txtSearchFilter.Text = "";
            txtSearchFilter.Focus();
        }
        private void txtSearchFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterName = "";

            switch (cbFilterBy.Text) {

                case "None":
                    FilterName = "";
                    break;

                case "Driver ID":
                    FilterName = "DriverID";
                    break;

                case "Person ID":
                    FilterName = "PersonID";
                    break;

                case "National No.":
                    FilterName = "NationalNo";
                    break;

                case "Full Name":
                    FilterName = "FullName";
                    break;

                default:
                    FilterName = "None";
                    break;
            }

            if (FilterName == "None" || txtSearchFilter.Text.Trim() == "") {

                DriversTable.DefaultView.RowFilter = "";
                lblCountRecords.Text = dgvDrivers.Rows.Count.ToString();
                return;
            }
                
            if (FilterName == "DriverID" || FilterName == "PersonID")
                DriversTable.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterName, txtSearchFilter.Text.Trim());
            if (FilterName == "NationalNo" || FilterName == "FullName")
                DriversTable.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterName, txtSearchFilter.Text.Trim());

            lblCountRecords.Text = dgvDrivers.Rows.Count.ToString();
        }
        private void txtSearchFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "Driver ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmPersonDetails((int)dgvDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
            frmDriversList_Load(null, null);
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmLicenseHistory((int)dgvDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
            frmDriversList_Load(null, null);
        }
    }
}
