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
    public partial class uCPersonInfo : UserControl
    {

        private int _PersonID = -1;
        private clsPeople _Person;
        public int PersonID { 
            
            get { return _PersonID; }
        }
        public clsPeople SelectedPersonInfo
        {
            get { return _Person; }
        }
        public uCPersonInfo()
        {
            InitializeComponent();
        }
        private string FullName() { 
            
            return _Person._FirstName + " " + _Person._SecondName + " " + _Person._ThirdName + " " + _Person._LastName;
        }
        private void LoadPersonImage() {

            if (_Person._Gendor == 0)
                pbProfilePicture.Image = Resources.Male_512;
            else
                pbProfilePicture.Image = Resources.Female_512;

            string ImagePath = _Person.ImagePath;
            if (ImagePath != "") {
                if (File.Exists(ImagePath))
                {

                    pbProfilePicture.ImageLocation = ImagePath;
                }
                else {

                    MessageBox.Show($"Could not find this image: {ImagePath}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                    
            }
        }
        private void _FillPersonInfo() {

            llEditPersonInfo.Enabled = true;
            _PersonID = _Person._PersonID;
            lblPersonID.Text = _Person._PersonID.ToString();
            lblNationalNo.Text = _Person._NationalNo;
            lblEmail.Text = _Person._Email;
            lblAddress.Text = _Person._Address;
            lblPhone.Text = _Person._Phone;
            lblName.Text = FullName();
            lblDateOfBirth.Text = _Person._DateOfBirth.ToString("dd/MM/yyyy");
            lblGendor.Text = (_Person._Gendor == 0) ? "Male" : "Female";
            lblCountry.Text = clsCountries.Find(_Person._NationalityCountryID)._CountryName;
            //lblCountry.Text = _Person.CountryInfo._CountryName;
            LoadPersonImage();
        }
        public void LoadPersonInfo(int PersonID) {

            _Person = clsPeople.FindPerson(PersonID);

            if (_Person == null) {

                ResetPersonInfo();
                MessageBox.Show($"No Person with PersonID [{PersonID}]", "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }
        public void LoadPersonInfo(string NationalNo)
        {

            _Person = clsPeople.FindPerson(NationalNo);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with National No. = " + NationalNo.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }
        public void ResetPersonInfo()
        {
            _PersonID = -1;
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblName.Text = "[????]";
            lblGendor.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pbProfilePicture.Image = Resources.Male_512;

        }
        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditPersonInfo frm = new frmAddEditPersonInfo(_PersonID);
            frm.ShowDialog();

            //Referesh
            LoadPersonInfo(_PersonID);
        }
    }
}
