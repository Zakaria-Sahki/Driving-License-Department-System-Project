using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBussinessLayer
{
    public class clsApplication {

        public enum enMode { AddNew, Update};
        public enMode Mode = enMode.AddNew;
        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7
        };
        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 };



        public int _ApplicationID { get; set; }
        public int _ApplicantPersonID { get; set; }

        public clsPeople PersonInfo;
        public string ApplicantFullName {

            get { 
                
                return clsPeople.FindPerson(_ApplicantPersonID).FullName;
            }
        }
        public DateTime _ApplicationDate { get; set; }
        public int _ApplicationTypeID { get; set; }
        public clsApplicationType ApplicationTypeInfo;
        public enApplicationStatus _ApplicationStatus { get; set; }
        public string StatusText
        {
            get {

                switch (_ApplicationStatus) {

                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }
        }
        public DateTime _LastStatusDate { get; set; }
        public float _PaidFees { get; set; }
        public int _CreatedByUserID { get; set; }
        
        public clsUsers CreatedByUserInfo;


        public clsApplication() {

            _ApplicationID = -1;
            _ApplicantPersonID = -1;
            _ApplicationDate = DateTime.Now;
            _ApplicationTypeID = -1;
            _ApplicationStatus = enApplicationStatus.New;
            _LastStatusDate = DateTime.Now;
            _PaidFees = 0;
            _CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }
        private clsApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, enApplicationStatus ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {

            _ApplicationID = ApplicationID;
            _ApplicantPersonID = ApplicantPersonID;
            PersonInfo = clsPeople.FindPerson(ApplicantPersonID);
            _ApplicationDate = ApplicationDate;
            _ApplicationTypeID = ApplicationTypeID;
            ApplicationTypeInfo = clsApplicationType.GetApplicationTypeInfo(ApplicationTypeID);
            _ApplicationStatus = ApplicationStatus;
            _LastStatusDate = LastStatusDate;
            _PaidFees = PaidFees;
            _CreatedByUserID = CreatedByUserID;
            CreatedByUserInfo = clsUsers.FindUser(CreatedByUserID);
            Mode = enMode.Update;
            
        }
        

        static public clsApplication GetApplicationInfoByID(int AppID) {

            int ApplicantPersonID = -1, AppTypeID = -1, CreatedByUser = -1;
            byte AppStatus = 0;
            float PaidFees = 0;
            DateTime AppDate = DateTime.Now, LastStatusDate = DateTime.Now;

            if (clsApplicationDataAccess.GetApplicationInfoByID(AppID, ref ApplicantPersonID, ref AppDate, ref AppTypeID, ref AppStatus, ref LastStatusDate, ref PaidFees, ref CreatedByUser))
            {

                return new clsApplication(AppID, ApplicantPersonID, AppDate, AppTypeID, (enApplicationStatus)AppStatus, LastStatusDate, PaidFees, CreatedByUser);
            }
            else {

                return null;
            }
        
        }
        private bool AddNewApplication() {

            this._ApplicationID = clsApplicationDataAccess.AddNewApplication(this._ApplicantPersonID, this._ApplicationDate, this._ApplicationTypeID, (byte)this._ApplicationStatus, this._LastStatusDate, this._PaidFees, this._CreatedByUserID);

            return (this._ApplicationID != -1);
        }
        private bool UpdateApplication() {

            return clsApplicationDataAccess.UpdateApplication(this._ApplicationID, (byte)this._ApplicationStatus, this._LastStatusDate);
        }
        public bool Save() {

            switch (Mode) {

                case enMode.AddNew:
                    if (AddNewApplication())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else {

                        return false;
                    }
                case enMode.Update:
                    return UpdateApplication();
            }
            return false;
        }
        public bool Cancel() {

            return clsApplicationDataAccess.UpdateApplication(this._ApplicationID, (byte)enApplicationStatus.Cancelled, DateTime.Now);
        }
        public bool SetComplete()
        {

            return clsApplicationDataAccess.UpdateApplication(this._ApplicationID, (byte)enApplicationStatus.Completed, DateTime.Now);
        }
        public bool Delete() {

            return clsApplicationDataAccess.DeleteApplication(this._ApplicationID);
        }
        static public bool IsExistApplication(int ApplicationID) {

            return clsApplication.IsExistApplication(ApplicationID);
        }
        public static int GetActiveApplicationID(int PersonID, clsApplication.enApplicationType ApplicationTypeID)
        {
            return clsApplicationDataAccess.GetActiveApplicationID(PersonID, (int)ApplicationTypeID);
        }
        public int GetActiveApplicationID(clsApplication.enApplicationType ApplicationTypeID)
        {
            return GetActiveApplicationID(this._ApplicantPersonID, ApplicationTypeID);
        }
        static public bool DoesPersonHasActiveApplication(int ApplicantPerson, int ApplicationTypeID) {

            return clsApplicationDataAccess.DoesPersonHaveActiveApplication(ApplicantPerson, ApplicationTypeID);
        }
        public bool DoesPersonHasActiveApplication(int ApplicationTypeID) {

            return clsApplicationDataAccess.DoesPersonHaveActiveApplication(this._ApplicantPersonID, ApplicationTypeID);
        }
        public static int GetActiveApplicationIDForLicenseClass(int PersonID, clsApplication.enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            return clsApplicationDataAccess.GetActiveApplicationIDForLicenseClass(PersonID, (int)ApplicationTypeID, LicenseClassID);
        }
    }
}
