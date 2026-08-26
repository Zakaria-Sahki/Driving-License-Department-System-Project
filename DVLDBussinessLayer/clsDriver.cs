using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBussinessLayer
{
    public class clsDriver {

        public enum enMode { AddNew, Update};
        public enMode Mode = enMode.AddNew;

        public int DriverID { get; set; }
        public int PersonID { get; set; }

        public clsPeople PersonInfo { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByUserID { get; set; }


        public clsDriver() {

            DriverID = -1;
            PersonID = -1;
            CreatedDate = DateTime.Now;
            CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }
        private clsDriver(int DriverID, int PersonID, DateTime CreatedDate, int CreatedByUserID) {

            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.PersonInfo = clsPeople.FindPerson(PersonID);
            this.CreatedDate = CreatedDate;
            this.CreatedByUserID = CreatedByUserID;
            Mode = enMode.Update;
        }
        
        static public DataTable GetAllDrivers() { 
            
            return clsDriverDataAccess.GetAllDrivers();
        }
        static public clsDriver FindDriver(int DriverID) {

            int PersonID = 0, CreatedByUserID = 0;
            DateTime CreatedDate = DateTime.Now;
            bool IsFound = clsDriverDataAccess.GetDriverInfoByID(DriverID, ref PersonID, ref CreatedDate, ref CreatedByUserID);

            if (IsFound) {

                return new clsDriver(DriverID, PersonID, CreatedDate, CreatedByUserID);
            }
            else
                return null;
        }
        static public clsDriver FindDriverByPersonID(int PersonID)
        {

            int DriverID = 0, CreatedByUserID = 0;
            DateTime CreatedDate = DateTime.Now;
            bool IsFound = clsDriverDataAccess.GetDriverInfoByPersonID(PersonID, ref DriverID, ref CreatedDate, ref CreatedByUserID);

            if (IsFound)
            {

                return new clsDriver(DriverID, PersonID, CreatedDate, CreatedByUserID);
            }
            else
                return null;
        }


        private bool AddNewDriver() {

            this.DriverID = clsDriverDataAccess.AddNewDriver(this.PersonID, this.CreatedByUserID);
            return (this.DriverID != -1);
        }
        private bool UpdateDriver() {

            return clsDriverDataAccess.UpdateDriver(this.DriverID, this.PersonID, this.CreatedByUserID);
        }
        public bool Save() {

            switch (Mode) {

                case enMode.AddNew:
                    if (AddNewDriver())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                
                case enMode.Update:
                    return UpdateDriver();
            }
            return false;
        }

        //static public DataTable GetLicenses(int DriverID) {

        //    //return clsLicenses.GetDriverLicenses(DriverID);
        //}
        //static public DataTable GetInternationalLicenses(int DriverID) {

        //    //return clsInternationalLicense.GetDriverInternationalLicenses(DriverID);
        //}
    }
}
