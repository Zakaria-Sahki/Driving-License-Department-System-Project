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
    public partial class frmShowLocalDrivingLicenseApplicationInfo : Form
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        public frmShowLocalDrivingLicenseApplicationInfo(int LocalApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalApplicationID;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmShowLocalDrivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingLicenseAppID(_LocalDrivingLicenseApplicationID);
        }
    }
}
