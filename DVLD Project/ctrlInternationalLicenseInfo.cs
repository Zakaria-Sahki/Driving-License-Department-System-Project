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

namespace DVLD_Project
{
    public partial class ctrlInternationalLicenseInfo : UserControl
    {
        private int _InternationaLicenseID = -1;
        private clsInternationalLicenses internationalLicenses;
        public ctrlInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        private void LoadImage() {

            if (internationalLicenses.DriverInfo.PersonInfo._Gendor == 0)
                pB_ProfilePicture.Image = Resources.Male_512;
            else
                pB_ProfilePicture.Image = Resources.Female_512;

            string _ImagePath = internationalLicenses.DriverInfo.PersonInfo.ImagePath;
            if (_ImagePath != "")
            {
                if (File.Exists(_ImagePath))
                {

                    pB_ProfilePicture.Load(_ImagePath);
                }
                else
                {

                    MessageBox.Show($"Could not find this image: {_ImagePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

        }
        public void LoadData(int InternationaLicenseID) {
            
            _InternationaLicenseID = InternationaLicenseID;
            internationalLicenses = clsInternationalLicenses.Find(InternationaLicenseID);

            if (internationalLicenses == null)
            {

                MessageBox.Show($"Could not find International license ID = {_InternationaLicenseID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _InternationaLicenseID = -1;
                return;
            }

            lblName.Text = internationalLicenses.DriverInfo.PersonInfo.FullName;
            lblInternationalLicenseID.Text = internationalLicenses.InternationalLicenseID.ToString();
            lblLicenseID.Text = internationalLicenses.IssuedUsingLocalLicenseID.ToString();
            lblNationalNo.Text = internationalLicenses.DriverInfo.PersonInfo._NationalNo;
            lblGendor.Text = (internationalLicenses.DriverInfo.PersonInfo._Gendor == 0) ? "Male" : "Female";
            lblIssueDate.Text = internationalLicenses.IssueDate.ToShortDateString();
            lblApplicationID.Text = internationalLicenses._ApplicationID.ToString();
            lblIsActive.Text = (internationalLicenses.IsActive) ? "Yes" : "No";
            lblDateOfBirth.Text = internationalLicenses.DriverInfo.PersonInfo._DateOfBirth.ToShortDateString();
            lblDriverID.Text = internationalLicenses.DriverID.ToString();
            lblExpirationDate.Text = internationalLicenses.ExpirationDate.ToShortDateString();
            LoadImage();
        }
    }
}
