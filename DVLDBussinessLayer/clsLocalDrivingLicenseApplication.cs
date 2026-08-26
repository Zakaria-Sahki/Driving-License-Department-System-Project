using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBussinessLayer
{
    public class clsLocalDrivingLicenseApplication : clsApplication {

        public enum enMode { AddNew, Update};
        public enMode Mode = enMode.AddNew;

        public int LDLAppID { get; set; }
        public int LicenseClassID { get; set; }
        public string PersonFullName {

            get {

                return base.ApplicantFullName;
            }
        }

        public clsLicenseClass LicenseClassInfo;

        public clsLocalDrivingLicenseApplication() {

            LDLAppID = -1;
            LicenseClassID = -1;
            Mode = enMode.AddNew;
        }
        private clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int ApplicantPersonID,
            DateTime ApplicationDate, int ApplicationTypeID,
             enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID, int LicenseClassID)
        {

            this.LDLAppID = LocalDrivingLicenseApplicationID;
            this._ApplicantPersonID = ApplicantPersonID;
            this.PersonInfo = clsPeople.FindPerson(ApplicantPersonID);
            this._ApplicationID = ApplicationID;
            this._ApplicationDate = ApplicationDate;
            this._ApplicationTypeID = (int)ApplicationTypeID;
            this._ApplicationStatus = ApplicationStatus;
            this._LastStatusDate = LastStatusDate;
            this._PaidFees = PaidFees;
            this._CreatedByUserID = CreatedByUserID;
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.FindLicenseClass(LicenseClassID);
            this.CreatedByUserInfo = clsUsers.FindUser(CreatedByUserID);
            Mode = enMode.Update;
        }

        private bool AddNewLocalDrivingLicenseApplication()
        {

            this.LDLAppID = clsLocalDrivingLicenseApplicationDataAccess.AddNewLdlApplication(this._ApplicationID, this.LicenseClassID);
            return (this.LDLAppID != -1);
        }
        private bool UpdateLocalDrivingLicenseApplication() {

            return clsLocalDrivingLicenseApplicationDataAccess.UpdateApplication(this._ApplicationID, this.LDLAppID, this.LicenseClassID);
        }
        static public clsLocalDrivingLicenseApplication FindLDLAppByLocalDrivingLicenseID(int LDLAppID)
        {

            int ApplicationID = 0, LicenseClassID = 0;
            bool IsFound = clsLocalDrivingLicenseApplicationDataAccess.GetLocaldlApplicationInfoByID(LDLAppID, ref ApplicationID, ref LicenseClassID);


            if (IsFound)
            {
                clsApplication Application = clsApplication.GetApplicationInfoByID(ApplicationID);

                return new clsLocalDrivingLicenseApplication(LDLAppID, Application._ApplicationID, Application._ApplicantPersonID, Application._ApplicationDate, Application._ApplicationTypeID, Application._ApplicationStatus, Application._LastStatusDate, Application._PaidFees, Application._CreatedByUserID, LicenseClassID);
            }
            else
                return null;
        }
        static public clsLocalDrivingLicenseApplication FindLDLAppByApplicationID(int ApplicationID)
        {

            int LDLAppID = 0, LicenseClassID = 0;
            bool IsFound = clsLocalDrivingLicenseApplicationDataAccess.GetLocaldlApplicationInfoByApplicationID(ApplicationID, ref LDLAppID, ref LicenseClassID);

            if (IsFound)
            {

                clsApplication Application = clsApplication.GetApplicationInfoByID(ApplicationID);
                return new clsLocalDrivingLicenseApplication(LDLAppID, Application._ApplicationID, Application._ApplicantPersonID, Application._ApplicationDate, Application._ApplicationTypeID, Application._ApplicationStatus, Application._LastStatusDate, Application._PaidFees, Application._CreatedByUserID, LicenseClassID);
            }
            else
                return null;
        }
        public bool Save() {

            base.Mode = (clsApplication.enMode) Mode;
            if (!base.Save())
                return false;

            switch (Mode) {

                case enMode.AddNew:
                    if (AddNewLocalDrivingLicenseApplication())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else {

                        return false;
                    }
                case enMode.Update:
                    return UpdateLocalDrivingLicenseApplication();
            }
            return false;
        }
        static public DataTable GetAll_LDLApplications()
        {

            return clsLocalDrivingLicenseApplicationDataAccess.GetAll_LDLApplications();
        }
        public bool DeleteLocalDrivingLicenseApp()
        {

            bool IsLocalDrivingApplicationDeleted = false;
            bool IsBaseApplicationDeleted = false;
            //First we delete the Local Driving License Application
            IsLocalDrivingApplicationDeleted = clsLocalDrivingLicenseApplicationDataAccess.DeleteLDLApplication(this.LDLAppID);

            if (!IsLocalDrivingApplicationDeleted)
                return false;
            //Then we delete the base Application
            IsBaseApplicationDeleted = base.Delete();
            return IsBaseApplicationDeleted;

        }





        // we will understand them later.

        public bool DoesPassTestType(clsTestType.enTestType TestTypeID)

        {
            return clsLocalDrivingLicenseApplicationDataAccess.DoesPassTestType(this.LDLAppID, (int)TestTypeID);
        }
        public bool DoesPassPreviousTest(clsTestType.enTestType CurrentTestType)
        {

            switch (CurrentTestType)
            {
                case clsTestType.enTestType.VisionTest:
                    //in this case no required prvious test to pass.
                    return true;

                case clsTestType.enTestType.WrittenTest:
                    //Written Test, you cannot sechdule it before person passes the vision test.
                    //we check if pass visiontest 1.

                    return this.DoesPassTestType(clsTestType.enTestType.VisionTest);


                case clsTestType.enTestType.StreetTest:

                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    return this.DoesPassTestType(clsTestType.enTestType.WrittenTest);

                default:
                    return false;
            }
        }
        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)

        {
            return clsLocalDrivingLicenseApplicationDataAccess.DoesPassTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public bool DoesAttendTestType(clsTestType.enTestType TestTypeID)

        {
            return clsLocalDrivingLicenseApplicationDataAccess.DoesAttendTestType(this.LDLAppID, (int)TestTypeID);
        }
        public byte TotalTrialsPerTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationDataAccess.TotalTrialsPerTest(this.LDLAppID, (int)TestTypeID);
        }
        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)

        {
            return clsLocalDrivingLicenseApplicationDataAccess.TotalTrialsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public static bool AttendedTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)

        {
            return clsLocalDrivingLicenseApplicationDataAccess.TotalTrialsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID) > 0;
        }
        public bool AttendedTest(clsTestType.enTestType TestTypeID)

        {
            return clsLocalDrivingLicenseApplicationDataAccess.TotalTrialsPerTest(this.LDLAppID, (int)TestTypeID) > 0;
        }
        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)

        {

            return clsLocalDrivingLicenseApplicationDataAccess.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public bool IsThereAnActiveScheduledTest(clsTestType.enTestType TestTypeID)

        {

            return clsLocalDrivingLicenseApplicationDataAccess.IsThereAnActiveScheduledTest(this.LDLAppID, (int)TestTypeID);
        }


        // ===========================================


        public clsTest GetLastTestPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTest.FindLastTestPerPersonAndLicenseClass(this._ApplicantPersonID, this.LicenseClassID, TestTypeID);
        }
        public byte GetPassedTestCount()
        {
            return clsTest.GetPassedTestCount(this.LDLAppID);
        }
        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTest.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }
        public bool PassedAllTests()
        {
            return clsTest.IsPassedAllTests(this.LDLAppID);
        }
        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return clsTest.IsPassedAllTests(LocalDrivingLicenseApplicationID);
        }


        public int IssueLicenseForTheFirstTime(string Notes, int CreatedByUserID) {

            clsDriver Driver;

            Driver = clsDriver.FindDriverByPersonID(this._ApplicantPersonID);

            if (Driver == null)
            {

                Driver = new clsDriver();
                Driver.PersonID = this._ApplicantPersonID;
                Driver.CreatedDate = DateTime.Now;
                Driver.CreatedByUserID = CreatedByUserID;

                if (!Driver.Save())
                {

                    // Message
                    return -1;
                }
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = this._ApplicationID;
            NewLicense.DriverID = Driver.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = clsLicense.enIssueReason.FirstTime;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (NewLicense.Save())
            {
                this.SetComplete();
                return NewLicense.LicenseID;
            }
            else
                return -1;
        }


        public int GetActiveLicenseID()
        {//this will get the license id that belongs to this application
            return clsLicense.GetActiveLicenseIDByPersonID(this._ApplicantPersonID, this.LicenseClassID);
        }
    }
}
