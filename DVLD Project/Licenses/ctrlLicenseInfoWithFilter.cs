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
    public partial class ctrlLicenseInfoWithFilter : UserControl
    {

        // Define a custom event handler delegate with parameters
        public event Action<int> OnLicenseSelected;
        // Create a protected method to raise the event with a parameter
        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int> handler = OnLicenseSelected;
            if (handler != null)
            {
                handler(LicenseID); // Raise the event with the parameter
            }
        }

        private int _LicenseID = -1;
        private bool _FilterEnabled = true;
        public bool FilterEnabled { 
        
            get { return _FilterEnabled; }
            set {

                _FilterEnabled = value;
                gBFilterLicenseID.Enabled = _FilterEnabled;
            }
        }
        public int LicenseID {

            get { return ctrlLicenseInfo1.LicenseID; }
        }
        public clsLicense SelectedLicenseInfo {

            get { return ctrlLicenseInfo1._SelectedLicense; }
        }

        public ctrlLicenseInfoWithFilter()
        {
            InitializeComponent();
        }


        public void LoadLicenseInfo(int LicenseID) { 
            
            txtSearch.Text = LicenseID.ToString();
            ctrlLicenseInfo1.LoadData(LicenseID);
            _LicenseID = ctrlLicenseInfo1.LicenseID;
            if (OnLicenseSelected != null && FilterEnabled)
                // Raise the event with parameter
                OnLicenseSelected(_LicenseID);

        }
        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren()) {

                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSearch.Focus();
                return;
            }

            _LicenseID = Convert.ToInt32(txtSearch.Text.Trim());
            LoadLicenseInfo(_LicenseID);
            
        }
        public void txtLicenseIDFocus() {

            txtSearch.Focus();
        }
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            //this will allow only digits if license id is selected
            if (e.KeyChar == (char)13)
            {

                btnSearch.PerformClick();
            }
        }
        private void txtSearch_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text.Trim())){

                e.Cancel = true;
                errorProvider1.SetError(txtSearch, "This field is required!");
            }
            else {

                //e.Cancel = false;
                errorProvider1.SetError(txtSearch, null);
            }
        }
    }
}
