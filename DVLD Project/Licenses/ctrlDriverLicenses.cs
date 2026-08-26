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
    public partial class ctrlDriverLicenses : UserControl
    {
        private int _DriverID = -1;
        private clsDriver _Driver;
        private DataTable _LocalLicensesHistoryTable;
        private DataTable _InternationalLicensesHistoryTable;

        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }

        private void _LoadLocalLicensesHistory() {

            _LocalLicensesHistoryTable = clsLicense.GetDriverLicenses(_DriverID);
            dgvLocalLicenses.DataSource = _LocalLicensesHistoryTable;
            lblCountOfLocalRecords.Text = dgvLocalLicenses.Rows.Count.ToString();

            if (dgvLocalLicenses.Rows.Count > 0) {

                dgvLocalLicenses.Columns[0].HeaderText = "Lic ID";
                dgvLocalLicenses.Columns[0].Width = 80;

                dgvLocalLicenses.Columns[0].HeaderText = "App.ID";
                dgvLocalLicenses.Columns[0].Width = 80;

                dgvLocalLicenses.Columns[0].HeaderText = "Class Name";
                dgvLocalLicenses.Columns[0].Width = 200;

                dgvLocalLicenses.Columns[0].HeaderText = "Issue Date";
                dgvLocalLicenses.Columns[0].Width = 100;

                dgvLocalLicenses.Columns[0].HeaderText = "Expiration Date";
                dgvLocalLicenses.Columns[0].Width = 100;

                dgvLocalLicenses.Columns[0].HeaderText = "Is Active";
                dgvLocalLicenses.Columns[0].Width = 100;
            }
        }
        private void _LoadInternationaLicensesHistory() { }
        public void LoadData(int DriverID) { 
            
            _DriverID = DriverID;
            _Driver = clsDriver.FindDriver(_DriverID);

            if (_Driver == null) {

                MessageBox.Show($"There is no driver with ID [{_DriverID}]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LoadLocalLicensesHistory();
            _LoadInternationaLicensesHistory();

        }
        public void LoadDataByPersonID(int PersonID)
        {
            _Driver = clsDriver.FindDriverByPersonID(PersonID);

            if (_Driver == null)
            {

                MessageBox.Show($"There is no driver linked with Person ID [{PersonID}]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _DriverID = _Driver.DriverID;

            _LoadLocalLicensesHistory();
            _LoadInternationaLicensesHistory();

        }
        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmLicenseInfo((int)dgvLocalLicenses.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
        private void showLicenseInfoToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
        
        public void Clear() { 
            
            _LocalLicensesHistoryTable.Clear();
            _InternationalLicensesHistoryTable.Clear();
        }
    }
}
