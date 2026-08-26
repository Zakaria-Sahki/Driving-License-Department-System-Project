using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBussinessLayer
{
    public class clsTestAppointments {

        public enum enMode { AddNew = 1, Update = 2};
        public enMode Mode;
        
        public clsTestType.enTestType TestTypeID { get; set; }
        public int TestAppointmentID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public float PaidFees { get; set; }
        public bool IsLocked { get; set; }
        public int LDL_AppID { get; set; }
        public int CreatedByUserID { get; set; }
        public int RetakeTestAppID { get; set; }

        public clsApplication RetakeTestAppInfo { get; set; }

        public int TestID
        {

            get
            {

                return _GetTestID();
            }
        }

        public clsTestAppointments() {

            TestAppointmentID = -1;
            TestTypeID = clsTestType.enTestType.VisionTest;
            AppointmentDate = DateTime.Now;
            PaidFees = 0;
            IsLocked = false;
            LDL_AppID = -1;
            CreatedByUserID = -1;
            RetakeTestAppID = -1;
            Mode = enMode.AddNew;
        }
        private clsTestAppointments(int TestAppointmentID, clsTestType.enTestType TestType, DateTime AppointmentDate, float PaidFees, bool IsLocked, int LDL_AppID, int CreatedByUserID, int RetakeTestAppID)
        {

            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestType;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.IsLocked = IsLocked;
            this.LDL_AppID = LDL_AppID;
            this.CreatedByUserID = CreatedByUserID;
            this.RetakeTestAppID = RetakeTestAppID;
            RetakeTestAppInfo = clsApplication.GetApplicationInfoByID(RetakeTestAppID);
            Mode = enMode.Update;
        }



        public DataTable GetAllAppointmentsByLDLAppID_And_TestType(clsTestType.enTestType TestTypeID)
        {


            return clsTestAppointmentsDataAccess.GetTestAppointmentsInfoByLocalDLAppID_And_TestType(this.LDL_AppID, (int)TestTypeID);
        }
        static public DataTable GetAllAppointmentsByLDLAppID_And_TestType(int LDL_AppID, clsTestType.enTestType TestTypeID)
        {


            return clsTestAppointmentsDataAccess.GetTestAppointmentsInfoByLocalDLAppID_And_TestType(LDL_AppID, (int)TestTypeID);
        }
        private bool AddNewTestAppointment() {

            this.TestAppointmentID = clsTestAppointmentsDataAccess.AddNewTestAppointment((int)this.TestTypeID, this.LDL_AppID, this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestAppID);
            return (this.TestAppointmentID != -1);
        }
        private bool UpdateTestAppointment() {

            return clsTestAppointmentsDataAccess.UpdateTestAppointment(this.TestAppointmentID, this.AppointmentDate, this.IsLocked);
        }
        public bool Save() {

            switch (Mode) {

                case enMode.AddNew:
                    if (AddNewTestAppointment())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return UpdateTestAppointment();
            }
            return false;
        }
        static public clsTestAppointments FindTestAppointment(int TestAppointmentID) {

            int _LDL_AppID = 0, _CreatedByUserID = 0, _RetakeTestAppID = 0, _TestTypeID = 0;
            bool _IsLocked = false;
            DateTime _AppointmentDate = DateTime.Now;
            float _PaidFees = 0;
            bool isFound = clsTestAppointmentsDataAccess.GetTestAppointmentInfoByID(TestAppointmentID, ref _TestTypeID, ref _LDL_AppID, ref _AppointmentDate, ref _PaidFees, ref _CreatedByUserID, ref _IsLocked, ref _RetakeTestAppID);

            if (isFound)
                return new clsTestAppointments(TestAppointmentID, (clsTestType.enTestType)_TestTypeID, _AppointmentDate, _PaidFees, _IsLocked, _LDL_AppID, _CreatedByUserID, _RetakeTestAppID);
            else
                return null;
        }
        static public clsTestAppointments GetLastTestAppointment(int LDL_AppID, clsTestType.enTestType TestTypeID) {

            int _CreatedByUserID = 0, _RetakeTestAppID = 0, TestAppointmentID = 0;
            bool _IsLocked = false;
            DateTime _AppointmentDate = DateTime.Now;
            float _PaidFees = 0;
            bool isFound = clsTestAppointmentsDataAccess.GetLastTestAppointment(LDL_AppID, (int)TestTypeID, ref TestAppointmentID, ref _AppointmentDate, ref _PaidFees, ref _CreatedByUserID, ref _IsLocked, ref _RetakeTestAppID);

            if (isFound)
                return new clsTestAppointments(TestAppointmentID, (clsTestType.enTestType)TestTypeID, _AppointmentDate, _PaidFees, _IsLocked, LDL_AppID, _CreatedByUserID, _RetakeTestAppID);
            else
                return null;
        }
        static public bool IsActiveAppointmentExist(int LDL_AppID, int TestTypeID) {

            return clsTestAppointmentsDataAccess.IsActiveAppointmentExist(LDL_AppID, TestTypeID);
        }
        static public DataTable GetAllTestAppointments() {

            return clsTestAppointmentsDataAccess.GetAllTestAppointments();
        }
        private int _GetTestID() {

            return clsTestAppointmentsDataAccess.GetTestID(this.TestAppointmentID);
        }
    }

}
