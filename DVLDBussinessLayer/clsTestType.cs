using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBussinessLayer
{
    public class clsTestType {

        public enum enMode { AddNew, Update};
        public enMode Mode = enMode.AddNew;

        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3};
        //public clsTestType.enTestType ID { get; set; }
        
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Fees { get; set; }


        public clsTestType()
        {

            ID = -1;
            Title = "";
            Description = "";
            Fees = 0;
            Mode = enMode.AddNew;
        }
        private clsTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, float TestTypeFees) { 
            
            ID = TestTypeID;
            Title = TestTypeTitle;
            Description = TestTypeDescription;
            Fees = TestTypeFees;
            Mode = enMode.Update;
        }

        static public DataTable GetAllTestTypes() { 
            
            return clsTestTypeDataAccess.GetAllTestTypes();
        }
        static public clsTestType FindTestByID(int ID) {

            string TestTypeTitle = "", TestTypeDescription = "";
            float TestTypeFees = 0;
            if (clsTestTypeDataAccess.GetTestTypeInfoByID(ID, ref TestTypeTitle, ref TestTypeDescription, ref TestTypeFees))
            {

                return new clsTestType(ID, TestTypeTitle, TestTypeDescription, TestTypeFees);
            }
            else { 
                
                return null;
            }
        }
        private bool UpdateTestTypeInfo() {

            return clsTestTypeDataAccess.UpdateTestType(this.ID, this.Title, this.Description, this.Fees);
        }
        private bool AddNewTestType() {

            this.ID = clsTestTypeDataAccess.AddNewTestType(this.Title, this.Description, this.Fees);
            return (this.ID != -1);
        }
        public bool Save() {

            switch (Mode) {

                case enMode.AddNew:
                    if (AddNewTestType())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else { 
                        
                        return false;
                    }
                case enMode.Update:
                    return UpdateTestTypeInfo();
            }

            return false;
        }
    }
}
