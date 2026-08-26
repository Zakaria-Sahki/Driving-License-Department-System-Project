using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBussinessLayer
{
    public class clsLicenseClass {

        public enum enMode { AddNew, Update}
        public enMode Mode = enMode.AddNew;

        public int LicenseClassID {  get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinmumAllowdAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public float ClassFees { get; set; }

        public clsLicenseClass() { 
            
            LicenseClassID = -1;
            ClassName = "";
            ClassDescription = "";
            MinmumAllowdAge = 18;
            DefaultValidityLength = 10;
            ClassFees = 0;

            Mode = enMode.AddNew;
        }
        private clsLicenseClass(int LicenseClassID, string ClassName, string ClassDescription, byte MinAllowdAge, byte DefaultValidityLength, float Fees)
        {

            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinmumAllowdAge = MinAllowdAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = Fees;

            Mode = enMode.Update;
        }

        private bool AddNewLicenseClass() {

            this.LicenseClassID = clsLicenseClassDataAccess.AddNewLicenseClass(this.ClassName, this.ClassDescription, this.MinmumAllowdAge, this.DefaultValidityLength, this.ClassFees);
            return (this.LicenseClassID != -1);
        }
        private bool UpdateLicenseClass() {

            return clsLicenseClassDataAccess.UpdateLicenseClass(this.LicenseClassID, this.ClassName, this.ClassDescription, this.MinmumAllowdAge, this.DefaultValidityLength, this.ClassFees);
        }
        public bool Save() {

            switch (Mode) {

                case enMode.AddNew:

                    if (AddNewLicenseClass()) {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return UpdateLicenseClass();
            }
            return false;
        }
        static public DataTable GetAllLicenseClasses() {
            
            return clsLicenseClassDataAccess.GetAllLicenseClasses();
        }
        static public clsLicenseClass FindLicenseClass(int LicenseClassID) {

            byte MinmumAge = 0, DefaultValidLength = 0;
            float ClassFees = 0;
            string ClassName = "", ClassDescription = "";

            if (clsLicenseClassDataAccess.GetLicenseClassByID(LicenseClassID, ref ClassName, ref ClassDescription, ref MinmumAge, ref DefaultValidLength, ref ClassFees)) {

                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription, MinmumAge, DefaultValidLength, ClassFees);
            }
            else
                return null;
        }
        static public clsLicenseClass FindLicenseClass(string ClassName)
        {
            int LicenseClassID = 0;
            byte MinmumAge = 0, DefaultValidLength = 0;
            float ClassFees = 0;
            string ClassDescription = "";

            if (clsLicenseClassDataAccess.GetLicenseClassByClassName(ClassName, ref LicenseClassID, ref ClassDescription, ref MinmumAge, ref DefaultValidLength, ref ClassFees))
            {

                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription, MinmumAge, DefaultValidLength, ClassFees);
            }
            else
                return null;
        }
    }
}
