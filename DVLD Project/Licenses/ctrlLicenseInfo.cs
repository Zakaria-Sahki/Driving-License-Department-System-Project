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
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Project
{
    public partial class ctrlLicenseInfo : UserControl
    {
        
        private int _LicenseID = -1;
        private clsLicense _License;

        public int LicenseID { 
            
            get { return _LicenseID; }
        }
        public clsLicense _SelectedLicense
        {
            get { 
                
                return _License;
            }
        }

        public ctrlLicenseInfo()
        {
            InitializeComponent();
        }
        private void LoadPersonImage()
        {

            if (_License.DriverInfo.PersonInfo._Gendor == 0)
                pBProfilPicture.Image = Resources.Male_512;
            else
                pBProfilPicture.Image = Resources.Female_512;

            string _ImagePath = _License.DriverInfo.PersonInfo.ImagePath;
            if (_ImagePath != "")
            {
                if (File.Exists(_ImagePath))
                {

                    pBProfilPicture.Load(_ImagePath);
                }
                else
                {

                    MessageBox.Show($"Could not find this image: {_ImagePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }
        public void LoadData(int LicenseID) {

            _LicenseID = LicenseID;
            _License = clsLicense.FindLicenseByID(_LicenseID);

            if (_License == null) {

                MessageBox.Show($"Could not find license ID = {_LicenseID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LicenseID = -1;
                return;
            }

            lblClassName.Text = _License.LicenseClassInfo.ClassName;
            lblFullName.Text = _License.DriverInfo.PersonInfo.FullName;
            lblLicenseID.Text = _License.LicenseID.ToString();
            lblNationalNo.Text = _License.DriverInfo.PersonInfo._NationalNo;
            lblGendor.Text = (_License.DriverInfo.PersonInfo._Gendor == 0) ? "Male" : "Female";
            lblIssueDate.Text = _License.IssueDate.ToShortDateString();
            lblIssueReason.Text = _License.IssueReasonText;
            lblNotes.Text = (_License.Notes == "") ? "No Notes." : _License.Notes;
            lblIsActive.Text = (_License.IsActive) ? "Yes" : "No";
            lblDateOfBirth.Text = _License.DriverInfo.PersonInfo._DateOfBirth.ToShortDateString();
            lblDriverID.Text = _License.DriverID.ToString();
            lblExpirationDate.Text = _License.ExpirationDate.ToShortDateString();
            lblIsDetained.Text = (_License.IsDetained) ? "Yes" : "No";
            LoadPersonImage();
        }
    }
}
