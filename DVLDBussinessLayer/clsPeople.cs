using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBussinessLayer
{
    public class clsPeople {

        public enum enMode { AddNewPerson, UpdatePerson };
        public enMode Mode = enMode.AddNewPerson;

        public int _PersonID { get; set; }
        public string _NationalNo { get; set; }
        public string _FirstName { get; set; }
        public string _SecondName { get; set; }
        public string _ThirdName { get; set; }
        public string _LastName { get; set; }
        public byte _Gendor { get; set; }
        public DateTime _DateOfBirth { get; set; }
        public int _NationalityCountryID { get; set; }
        public string _Phone { get; set; }
        public string _Email { get; set; }
        public string _Address { get; set; }
        public string FullName {
            get { return _FirstName + " " + _SecondName + " " + _ThirdName + " " + _LastName; }
        }

        private string _ImagePath;
        public string ImagePath { 
        
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }


        public clsCountries CountryInfo;

        public clsPeople() { 
            
            _PersonID = -1;
            _NationalNo = "";
            _FirstName = "";
            _SecondName = "";
            _ThirdName = "";
            _LastName = "";
            _DateOfBirth = DateTime.Now;
            _Gendor = 0;
            _NationalityCountryID = -1;
            _Phone = "";
            _Email = "";
            _Address = "";
            _ImagePath = "";

            Mode = enMode.AddNewPerson;
        }
        private clsPeople(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, byte Gendor, string Address, string Phone, string Email, int NationalCountryID, string ImagePath)
        {

            _PersonID = PersonID;
            _NationalNo = NationalNo;
            _FirstName = FirstName;
            _SecondName = SecondName;
            _ThirdName = ThirdName;
            _LastName = LastName;
            _DateOfBirth = DateOfBirth;
            _Gendor = Gendor;
            _NationalityCountryID = NationalCountryID;
            _Phone = Phone;
            _Email = Email;
            _Address = Address;
            _ImagePath = ImagePath;
            CountryInfo = clsCountries.Find(_NationalityCountryID);

            Mode = enMode.UpdatePerson;
        }


        static public clsPeople FindPerson(int PersonID) {

            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalCountryID = -1;
            byte Gendor = 0;

            if (clsPeopleDataAccess.GetPersonInfoByPersonID(PersonID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalCountryID, ref ImagePath))
                return new clsPeople(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalCountryID, ImagePath);
            else
                return null;

        }
        static public clsPeople FindPerson(string NationalNo)
        {

            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalCountryID = -1, PersonID = -1;
            byte Gendor = 0;

            if (clsPeopleDataAccess.GetPersonInfoByNationalNo(NationalNo, ref PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalCountryID, ref ImagePath))
                return new clsPeople(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalCountryID, ImagePath);
            else
                return null;

        }
        static public DataTable GetAllPeople() { 
        
            return clsPeopleDataAccess.GetAllPeople();
        }
        static public bool IsPersonExist(string NationalNo)
        {
            return clsPeopleDataAccess.IsPersonExist(NationalNo);
        }
        static public bool IsPersonExist(int PersonID)
        {
            return clsPeopleDataAccess.IsPersonExist(PersonID);
        }
        static public bool DeletePerson(int PersonID)
        {

            return clsPeopleDataAccess.DeletePerson(PersonID);
        }
        private bool AddNewPerson() {

            this._PersonID = clsPeopleDataAccess.AddNewPerson(this._NationalNo, this._FirstName, this._SecondName, this._ThirdName, this._LastName, this._DateOfBirth, this._Gendor, this._Address, this._Phone, this._NationalityCountryID, this._Email, this._ImagePath);
            return (this._PersonID != -1);
        }
        private bool UpdatePerson() {

            return clsPeopleDataAccess.UpdatePerson(this._PersonID, this._NationalNo, this._FirstName, this._SecondName, this._ThirdName, this._LastName, this._DateOfBirth, this._Gendor, this._Address, this._Phone, this._Email, this._NationalityCountryID, this._ImagePath);
        }
        public bool Save()
        {

            switch (Mode)
            {

                case enMode.AddNewPerson:
                    if (AddNewPerson())
                    {

                        Mode = enMode.UpdatePerson;
                        return true;
                    }
                    break;
                case enMode.UpdatePerson:
                    return UpdatePerson();

            }
            return false;
        }
    }
}
