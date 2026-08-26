using DVLD_DataAccessLayer;
using DVLDBussinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLDBussinessLayer.clsTestType;

namespace DVLDBussinessLayer
{
    public class clsTest {

        public enum enMode { AddNew = 1, Update = 2}
        public enMode Mode = enMode.AddNew;

        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public clsTestAppointments TestAppointmentInfo { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }


        public clsTest() { 
            
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = false;
            Notes = "";
            CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }
        private clsTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {

            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestAppointmentInfo = clsTestAppointments.FindTestAppointment(TestAppointmentID);
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;
            Mode = enMode.Update;
        }

        public static DataTable GetAllTests()
        {
            return clsTestDataAccess.GetTestsTable();

        }
        private bool _AddNewTest()
        {
            //call DataAccess Layer 

            this.TestID = clsTestDataAccess.AddNewTest(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
            return (this.TestID != -1);
        }
        private bool _UpdateTest()
        {
            //call DataAccess Layer 

            return clsTestDataAccess.UpdateTest(this.TestID, this.TestAppointmentID,
                this.TestResult, this.Notes, this.CreatedByUserID);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTest();

            }

            return false;
        }

        static public clsTest Find(int TestID) {

            int TestAppointmentID = 0, CreatedByUserID = 0;
            string Notes = "";
            bool TestResult = false;
            bool IsFound = clsTestDataAccess.GetTestInfoByID(TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID);

            if (IsFound)
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            else
                return null;
        }
        static public clsTest FindTestByTestAppointmentID(int TestAppointmentID) {

            int TestID = 0, CreatedByUserID = 0;
            string Notes = "";
            bool TestResult = false;
            bool IsFound = clsTestDataAccess.GetTestInfoByTestAppointmentID(TestAppointmentID, ref TestID, ref TestResult, ref Notes, ref CreatedByUserID);

            if (IsFound)
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            else
                return null;
        }



        static public clsTest FindLastTestPerPersonAndLicenseClass(int PersonID, int LicenseClassID, clsTestType.enTestType TestTypeID)
        {

            int TestID = 0, CreatedByUserID = 0, TestAppointmentID = 0;
            string Notes = "";
            bool TestResult = false;
            bool IsFound = clsTestDataAccess.GetLastTestByPersonAndTestTypeAndLicenseClass(PersonID, LicenseClassID, (int)TestTypeID, ref TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID);

            if (IsFound)
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);
            else
                return null;
        }
        static public byte GetPassedTestCount(int LocalDrivingLicenseApplicationID) {

            return clsTestDataAccess.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }
        static public bool IsPassedAllTests(int LocalDrivingLicenseApplicationID) {

            return (GetPassedTestCount(LocalDrivingLicenseApplicationID) == 3);
        }
    }
}



