using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBussinessLayer
{
    public class clsLicense {

        public enum enMode { AddNew, Update}
        public enMode Mode = enMode.AddNew;

        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 }

        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LicenseClassID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public float PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason { get; set; }
        public int CreatedByUserID { get; set; }
        public clsApplication ApplicationInfo { get; set; }
        public clsDriver DriverInfo { get; set; }
        public clsLicenseClass LicenseClassInfo { get; set; }
        public clsUsers CreatedUserInfo { get; set; }

        public string IssueReasonText {

            get { 
                
                return GetIssueReasonText(IssueReason);
            }
        }
        
        public clsDetainedLicense DetainedLicenseInfo { get; set; }
        public bool IsDetained
        {
            get {

                return clsDetainedLicense.IsDetainedLicense(this.LicenseID);
            }
        }


        public clsLicense() {

            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClassID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = true;
            this.IssueReason = enIssueReason.FirstTime;
            this.CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }
        private clsLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive, enIssueReason IssueReason, int CreatedByUserID)
        {

            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClassID = LicenseClassID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;

            this.ApplicationInfo = clsApplication.GetApplicationInfoByID(this.ApplicationID);
            this.DriverInfo = clsDriver.FindDriver(this.DriverID);
            this.LicenseClassInfo = clsLicenseClass.FindLicenseClass(this.LicenseClassID);
            this.CreatedUserInfo = clsUsers.FindUser(this.CreatedByUserID);
            this.DetainedLicenseInfo = clsDetainedLicense.FindDetainedLicenseByLicenseID(this.LicenseID);

            Mode = enMode.Update;
        }

        static public DataTable GetAllLicenses() {

            return clsLicenseDataAccess.GetAllLicenses();
        }
        static public clsLicense FindLicenseByID(int LicenseID) {

            int ApplicationID = 0, DriverID = 0, LicenseClassID = 0, CreatedByUserID = 0;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            bool IsActive = false;
            float PaidFees = 0;
            byte IssueReason = 0;
            bool IsFound = clsLicenseDataAccess.GetLicenseInfoByID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClassID, ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID);

            if (IsFound)
            {

                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);
            }
            else
                return null;
        }
        static public clsLicense FindLicenseByApplicationID(int ApplicationID)
        {

            int LicenseID = 0, DriverID = 0, LicenseClassID = 0, CreatedByUserID = 0;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            bool IsActive = false;
            float PaidFees = 0;
            byte IssueReason = 0;
            bool IsFound = clsLicenseDataAccess.GetLicenseInfoByAppID(ApplicationID, ref LicenseID, ref DriverID, ref LicenseClassID, ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID);

            if (IsFound)
            {

                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);
            }
            else
                return null;
        }
        private bool AddNewLicense() {

            this.LicenseID = clsLicenseDataAccess.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClassID, this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);
            return (this.LicenseID != -1);
        }
        private bool UpdateLicense() {

            return clsLicenseDataAccess.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseClassID, this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);
        }
        public bool Save() {

            switch (Mode) {

                case enMode.AddNew:
                    if (AddNewLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return UpdateLicense();

            }
            return false;
        }
        static public DataTable GetPersonLicenses(int PersonID)
        {

            return clsLicenseDataAccess.GetPersonLicenses(PersonID);
        }
        static public DataTable GetDriverLicenses(int DriverID)
        {

            return clsLicenseDataAccess.GetDriverLicenses(DriverID);
        }


        static public bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID) {

            return (GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1);
        }
        static public int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID) {

            return clsLicenseDataAccess.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
        }
        public bool IsLicenseExpired() {

            return (this.ExpirationDate < DateTime.Now);
        }
        public bool DeactivateCurrentLicense() {

            return clsLicenseDataAccess.DeactivateLicense(this.LicenseID);
        }
        static public string GetIssueReasonText(enIssueReason IssueReason) {

            string Text = "";
            switch (IssueReason) {

                case enIssueReason.FirstTime:
                    Text = "First Time";
                    break;
                case enIssueReason.Renew:
                    Text = "Renew";
                    break;
                case enIssueReason.DamagedReplacement:
                    Text = "Replacement for Damaged";
                    break;
                case enIssueReason.LostReplacement:
                    Text = "Replacement for Lost";
                    break;
            }
            return Text;
        }


        public clsLicense RenewLicense(string Notes, int CreatedByUserID) { 
            
            clsApplication Application = new clsApplication();
            Application._ApplicationTypeID = (int)clsApplication.enApplicationType.RenewDrivingLicense;
            Application._ApplicationDate = DateTime.Now;
            Application._LastStatusDate = DateTime.Now;
            Application._ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application._CreatedByUserID = CreatedByUserID;
            Application._PaidFees = clsApplicationType.GetApplicationTypeInfo(Application._ApplicationTypeID).Fees;
            Application._ApplicantPersonID = this.DriverInfo.PersonID;

            if (!Application.Save()) {

                return null;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = Application._ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;
            int DefaultValidityLength = this.LicenseClassInfo.DefaultValidityLength;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.Notes = Notes;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = clsLicense.enIssueReason.Renew;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (!NewLicense.Save()) {

                return null;
            }

            DeactivateCurrentLicense();

            return NewLicense;
        }
        public clsLicense ReplacementLicense(enIssueReason IssueReason, int CreatedByUserID)
        {

            clsApplication Application = new clsApplication();
            Application._ApplicationTypeID = (IssueReason == enIssueReason.LostReplacement) ? (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense : (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense;
            Application._ApplicationDate = DateTime.Now;
            Application._LastStatusDate = DateTime.Now;
            Application._ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application._CreatedByUserID = CreatedByUserID;
            Application._PaidFees = clsApplicationType.GetApplicationTypeInfo(Application._ApplicationTypeID).Fees;
            Application._ApplicantPersonID = this.DriverInfo.PersonID;

            if (!Application.Save())
            {

                return null;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = Application._ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;
            NewLicense.PaidFees = 0; // no fees for the license because it's a replacement. 
            NewLicense.Notes = this.Notes;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (!NewLicense.Save())
            {

                return null;
            }

            DeactivateCurrentLicense();

            return NewLicense;
        }

        public int Detain(float FineFees, int CreatedByUserID) { 
            
            clsDetainedLicense detainedLicense = new clsDetainedLicense();
            detainedLicense.LicenseID = this.LicenseID;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = Convert.ToSingle(FineFees);
            detainedLicense.CreatedByUserID = CreatedByUserID;

            if (detainedLicense.Save())
            {

                return detainedLicense.DetainID;
            }
            else {

                return -1;
            }
        }

        public bool ReleaseDetainLicense(int CreatedByUserID, ref int ApplicationID) {


            clsApplication Application = new clsApplication();
            Application._ApplicationTypeID = (int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense;
            Application._ApplicationDate = DateTime.Now;
            Application._LastStatusDate = DateTime.Now;
            Application._ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application._CreatedByUserID = CreatedByUserID;
            Application._PaidFees = clsApplicationType.GetApplicationTypeInfo(Application._ApplicationTypeID).Fees;
            Application._ApplicantPersonID = this.DriverInfo.PersonID;
            

            if (!Application.Save()) {

                ApplicationID = -1;
                return false;
            }

            if (this.DetainedLicenseInfo.ReleaseDetainedLicense(CreatedByUserID, Application._ApplicationID))
            {
                ApplicationID = Application._ApplicationID;
                return true;
            }
            else
                return false;


        }
    }
}
