using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBussinessLayer
{
    public class clsDetainedLicense {


        public enum enMode { AddNew, Update}
        public enMode Mode = enMode.AddNew;

        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public float FineFees { get; set; }
        public int CreatedByUserID { get; set; }


        public bool IsReleased { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }
        public int ReleaseApplicationID { get; set; }
        
        public clsLicense LicenseInfo { get; set; }
        public clsUsers CreatedByUserInfo { get; set; }
        public clsUsers ReleasedByUserInfo { get; set; }
        
        public clsDetainedLicense() {

            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MaxValue;
            this.ReleasedByUserID = -1;
            this.ReleaseApplicationID = -1;

            Mode = enMode.AddNew;
        }
        private clsDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, float FineFees, int CreatedByUserID, bool IsReleased, DateTime ReleaseDate, int ReleasedByUserID, int ReleasesApplicationID) {

            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleasesApplicationID;
            this.CreatedByUserInfo = clsUsers.FindUser(this.CreatedByUserID);

            this.ReleasedByUserInfo = clsUsers.FindUser(this.ReleasedByUserID);

            Mode = enMode.Update;
            // i made a licenseInfo object here and this is a big mistake.
        }


        static public DataTable GetAllDetainedLicenses() { 
            
            return clsDetainedLicenseDataAccess.GetAllDetainedLicense();
        }
        private bool AddNewDetainLicense() {

            this.DetainID = clsDetainedLicenseDataAccess.AddNewDetainedLicense(this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);

            return (this.DetainID != -1);
        }
        private bool UpdateDetainLicense() {

            return clsDetainedLicenseDataAccess.UpdateDetainedLicense(this.DetainID, this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);
        }
        public bool Save() {

            switch (Mode) {

                case enMode.AddNew:

                    if (AddNewDetainLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return UpdateDetainLicense();
            }
            return false;
        }
        static public clsDetainedLicense FindDetainedLicense(int DetainID) {


            int LicenseID = 0, CreatedByUserID = 0, ReleasedByUserID = 0, ReleaseApplicationID = 0;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.MaxValue;
            float FineFees = 0;
            bool isReleased = false;

            bool IsFound = clsDetainedLicenseDataAccess.FindDetainedLicenseByDetainID(DetainID, ref LicenseID, ref DetainDate, ref FineFees, ref CreatedByUserID, ref isReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID);

            if (IsFound)
            {

                return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, isReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
            }
            else
                return null;
        }
        static public clsDetainedLicense FindDetainedLicenseByLicenseID(int LicenseID) {

            int DetainID = 0, CreatedByUserID = 0, ReleasedByUserID = 0, ReleaseApplicationID = 0;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.MaxValue;
            float FineFees = 0;
            bool isReleased = false;

            bool IsFound = clsDetainedLicenseDataAccess.FindDetainedLicenseByLicenseID(LicenseID, ref DetainID, ref DetainDate, ref FineFees, ref CreatedByUserID, ref isReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID);

            if (IsFound)
            {

                return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, isReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
            }
            else
                return null;
        }
        static public bool IsDetainedLicense(int LicenseID) {

            return clsDetainedLicenseDataAccess.IsDetainedLicense(LicenseID);
        }
        public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID) {

            return clsDetainedLicenseDataAccess.ReleaseDetainedLicense(this.DetainID, ReleasedByUserID, ReleaseApplicationID);
        }
    

    }
}
