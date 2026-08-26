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
    public partial class frmLicenseHistory : Form
    {
        private int _PersonID = -1;

        public frmLicenseHistory()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(935, 670);
        }
        public frmLicenseHistory(int PersonID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(935, 670);
            this._PersonID = PersonID;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _PersonID = obj;

            if (_PersonID == -1)
            {
                ctrlDriverLicenses1.Clear();
            }
            else {

                ctrlDriverLicenses1.LoadDataByPersonID(_PersonID);
            }
        }
        private void frmLicenseHistory_Load(object sender, EventArgs e)
        {

            if (_PersonID != -1)
            {

                ctrlPersonCardWithFilter1.LoadData(_PersonID);
                ctrlPersonCardWithFilter1.FilterEnabled = false;
                ctrlDriverLicenses1.LoadDataByPersonID(_PersonID);
            }
            else {

                ctrlPersonCardWithFilter1.Enabled = true;
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }
    }
}
