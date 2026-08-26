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
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        // Define a custom event handler delegate with parameters
        public event Action<int> OnPersonSelected;
        // Create a protected method to raise the event with a parameter
        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelected;
            if (handler != null)
            {
                handler(PersonID); // Raise the event with the parameter
            }
        }

        private bool _ShowAddPerson = true;
        public bool ShowAddPerson { 
            
            get { return _ShowAddPerson; }
            set
            {
                _ShowAddPerson = value;
                btnAddNewPerson.Visible = _ShowAddPerson;
            }
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled { 
            
            get { return _FilterEnabled; }
            set { 
                
                _FilterEnabled = value;
                gbFilter.Enabled = _FilterEnabled;
            }
        }
        private int _PersonID = -1;
        public int PersonID {

            get { return uCPersonInfo1.PersonID; }
        }
        public clsPeople SelectedPersonInfo {

            get { return uCPersonInfo1.SelectedPersonInfo; }
        }

        public enum enMode { FilterByPersonID, FilterByNationalNo};
        enMode _Mode;

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 0)
                _Mode = enMode.FilterByPersonID;
            else if (cbFilterBy.SelectedIndex == 1)
                _Mode = enMode.FilterByNationalNo;

            txtSerach.Text = string.Empty;
            txtSerach.Focus();
        }
        private void LoadPersonDataToPersonCardCtrl(object sender, int PersonID) {

            cbFilterBy.SelectedIndex = 0;
            txtSerach.Text = PersonID.ToString().Trim();
            uCPersonInfo1.LoadPersonInfo(PersonID);
        }

        // i make it public to validate in the user addition
        private void btnSearch_Click(object sender, EventArgs e)
        {
            switch (_Mode) { 
                
                case enMode.FilterByPersonID:
                    uCPersonInfo1.LoadPersonInfo(Convert.ToInt32(txtSerach.Text.Trim()));
                    break;

                case enMode.FilterByNationalNo:
                    uCPersonInfo1.LoadPersonInfo(txtSerach.Text.ToString().Trim());
                    break;
            }

            if (OnPersonSelected != null && FilterEnabled)
                OnPersonSelected(uCPersonInfo1.PersonID);
        }
        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPersonInfo frm = new frmAddEditPersonInfo();
            frm.DataBack += LoadPersonDataToPersonCardCtrl;
            frm.ShowDialog();
        }
        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 1;
            txtSerach.Focus();
        }
        private void txtSerach_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSerach.Text.Trim()))
            {

                e.Cancel = true;
                txtSerach.Focus();
                errorProvider1.SetError(txtSerach, "This Field is required!");
            }
            else {

                errorProvider1.SetError(txtSerach, null);
            }
        }
        private void txtSerach_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is Enter (character code 13)
            if (e.KeyChar == (char)13)
            {

                btnSearch.PerformClick();
            }

            //this will allow only digits if person id is selected
            if (cbFilterBy.SelectedIndex == 0)
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        public void LoadData(int PersonID) {

            cbFilterBy.SelectedIndex = 0;
            txtSerach.Text = PersonID.ToString().Trim();
            uCPersonInfo1.LoadPersonInfo(PersonID);
        }

        public void FilterFocus() {

            txtSerach.Focus();
        }
    }
}
