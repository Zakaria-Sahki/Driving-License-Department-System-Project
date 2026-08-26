using DVLD_Project.Properties;
using DVLDBussinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmAddEditPersonInfo : Form
    {
        public delegate void DataBackEventHandler(object sender, int PersonID);
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNewPerson, UpdatePerson}
        enMode _Mode = enMode.AddNewPerson;

        private int _PersonID;
        clsPeople _Person;

        public frmAddEditPersonInfo()
        {
            InitializeComponent();
            _Mode = enMode.AddNewPerson;
        }
        public frmAddEditPersonInfo(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
            _Mode = enMode.UpdatePerson;    
        }
        private void _FillCountriesInComboBox() {

            DataTable CountriesTable = clsCountries.GetAllCountries();

            foreach (DataRow Row in CountriesTable.Rows) {

                comboBox1.Items.Add(Row["CountryName"]);
            }
        }
        private void CheckingGendor(int Gendorbit) {

            if (Gendorbit == 0)
                rbMale.Checked = true;
            else if (Gendorbit == 1)
                rbFemale.Checked = true;
        }
        private void _ResetDefaultValues()
        {

            _FillCountriesInComboBox();

            if (_Mode == enMode.AddNewPerson)
            {

                lblTitle.Text = "Add New Person";
                _Person = new clsPeople();
            }
            else {

                lblTitle.Text = "Update Person";
            }

            if (rbMale.Checked) {

                pBProfilePhoto.Image = Resources.Male_512;
            }
            else {

                pBProfilePhoto.Image = Resources.Female_512;
            }

            LlRemove.Visible = (pBProfilePhoto.ImageLocation != null);

            dateTimePicker1.MaxDate = DateTime.Now.AddYears(-18);
            dateTimePicker1.Value = dateTimePicker1.MaxDate;
            dateTimePicker1.MinDate = DateTime.Now.AddYears(-100);


            comboBox1.SelectedIndex = comboBox1.FindString("Algeria");

            txtNationalNo.Text = "";
            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            rbMale.Checked = true;
        }
        private void LoadData() {

            _Person = clsPeople.FindPerson(_PersonID);

            if (_Person == null)
            {

                MessageBox.Show($"No Person with ID [{_PersonID}]", "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            lblPersonID.Text = _Person._PersonID.ToString();
            txtNationalNo.Text = _Person._NationalNo;
            txtFirstName.Text = _Person._FirstName;
            txtSecondName.Text = _Person._SecondName;
            txtThirdName.Text = _Person._ThirdName;
            txtLastName.Text = _Person._LastName;
            dateTimePicker1.Value = _Person._DateOfBirth;
            CheckingGendor(_Person._Gendor);
            txtEmail.Text = _Person._Email;
            txtPhone.Text = _Person._Phone;
            txtAddress.Text = _Person._Address;
            comboBox1.SelectedIndex = comboBox1.FindString(_Person.CountryInfo._CountryName);



            if (_Person.ImagePath != "")
            {

                pBProfilePhoto.ImageLocation = _Person.ImagePath;
            }
            LlRemove.Visible = (_Person.ImagePath != "");
        }
        private void frmAddEditPersonInfo_Load(object sender, EventArgs e)
        {

            _ResetDefaultValues();
            if (_Mode == enMode.UpdatePerson)
                LoadData();
        }
        private void LlSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Open";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                string SelectedFilePath = openFileDialog1.FileName;
                pBProfilePhoto.ImageLocation = SelectedFilePath;
                LlRemove.Visible = true;
            }
        }
        private void LlRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            pBProfilePhoto.ImageLocation = null;
            if (rbMale.Checked)
            {

                pBProfilePhoto.Image = DVLD_Project.Properties.Resources.Male_512;
            }
            else
            {
                pBProfilePhoto.Image = DVLD_Project.Properties.Resources.Female_512;
            }
            LlRemove.Visible = false;
        }
        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (pBProfilePhoto.ImageLocation == null)
                pBProfilePhoto.Image = DVLD_Project.Properties.Resources.Male_512;
        }
        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (pBProfilePhoto.ImageLocation == null)
                pBProfilePhoto.Image = DVLD_Project.Properties.Resources.Female_512;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private byte CheckedGendor()
        {

            if (rbMale.Checked)
            {
                return Convert.ToByte(rbMale.Tag);
            }
            else
            {

                return Convert.ToByte(rbFemale.Tag);
            }
        }
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {

                if (!clsValidation.ValidateEmail(txtEmail.Text))
                {

                    e.Cancel = true;
                    txtEmail.Focus();
                    errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
                }
                else
                {

                    errorProvider1.SetError(txtEmail, null);
                }
            }
        }
        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNationalNo.Text.Trim()))
            {

                e.Cancel = true;
                txtNationalNo.Focus();
                errorProvider1.SetError(txtNationalNo, "This field is Required!");
                return;
            }
            else {

                errorProvider1.SetError(txtNationalNo, null);
            }

            if (txtNationalNo.Text.Trim() != _Person._NationalNo && clsPeople.IsPersonExist(txtNationalNo.Text.Trim()))
            {

                e.Cancel = true;
                txtNationalNo.Focus();
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");
            }
            else {

                errorProvider1.SetError(txtNationalNo, null);
            }
        }
        private bool _HandlePersonImage() {

            if (_Person.ImagePath != pBProfilePhoto.ImageLocation) {

                if (_Person.ImagePath != "")
                {
                    try {

                        File.Delete(_Person.ImagePath);
                    }
                    catch (IOException IOEx) {

                        MessageBox.Show($"Error: {IOEx.Message}.");
                    }
                }

                if (pBProfilePhoto.ImageLocation != null) {

                    string SourceFileName = pBProfilePhoto.ImageLocation.ToString();
                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceFileName)) {

                        pBProfilePhoto.ImageLocation = SourceFileName;
                        return true;
                    }
                    else {

                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }



        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {

                e.Cancel = true;
                txtFirstName.Focus();
                errorProvider1.SetError(txtFirstName, "First Name Required!");

            }
        }
        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {

                e.Cancel = true;
                txtLastName.Focus();
                errorProvider1.SetError(txtLastName, "Last Name Required!");

            }
        }
        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {

                e.Cancel = true;
                txtPhone.Focus();
                errorProvider1.SetError(txtPhone, "Phone Required!");

            }
        }
        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {

                e.Cancel = true;
                txtAddress.Focus();
                errorProvider1.SetError(txtAddress, "Address Required!");

            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {


            if (!_HandlePersonImage())
                return;

            _Person._NationalNo = txtNationalNo.Text.Trim();
            _Person._FirstName = txtFirstName.Text.Trim();
            _Person._SecondName = txtSecondName.Text.Trim();
            _Person._ThirdName = txtThirdName.Text.Trim();
            _Person._LastName = txtLastName.Text.Trim();
            _Person._Email = txtEmail.Text.Trim();
            _Person._Phone = txtPhone.Text.Trim();
            _Person._Address = txtAddress.Text.Trim();
            _Person._Gendor = CheckedGendor();
            _Person._DateOfBirth = dateTimePicker1.Value;

            _Person._NationalityCountryID = clsCountries.Find(comboBox1.Text)._CountryID;

            // ImagePath still not implemented

            if (pBProfilePhoto.ImageLocation != null)
                _Person.ImagePath = pBProfilePhoto.ImageLocation;
            else
                _Person.ImagePath = "";

            if (_Person.Save())
            {
                lblPersonID.Text = _Person._PersonID.ToString();
                _Mode = enMode.UpdatePerson;
                lblTitle.Text = "Update Person";
                MessageBox.Show($"Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DataBack?.Invoke(this, _Person._PersonID);
            }
            else {

                MessageBox.Show($"Error: Data is not saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
