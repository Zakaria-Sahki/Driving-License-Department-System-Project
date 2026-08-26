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
    public class clsApplicationType {

        public enum enMode { AddNew, Update};
        public enMode _Mode = enMode.AddNew;

        public int ID { get; set; }
        public string Title { get; set; }
        public float Fees { get; set; }


        public clsApplicationType() {

            ID = -1;
            Title = "";
            Fees = 0;
            _Mode = enMode.AddNew;
        }
        private clsApplicationType(int ID, string AppTitle, float AppFees) {

            this.ID = ID;
            this.Title = AppTitle;
            this.Fees = AppFees;
            _Mode = enMode.Update;
        }

        static public clsApplicationType GetApplicationTypeInfo(int ID) {

            string AppTypeTitle = "";
            float AppFees = 0;

            if (clsApplicationTypesDataAccess.GetApplicationTypeInfo(ID, ref AppTypeTitle, ref AppFees))
            {

                return new clsApplicationType(ID, AppTypeTitle, AppFees);
            }
            else {
                return null;
            }
        }
        static public DataTable GetAllApplicationTypes() {

            return clsApplicationTypesDataAccess.GetAllApplicationTypes();
        }
        private bool UpdateApplicationType() {

            return clsApplicationTypesDataAccess.UpdateApplicationType(this.ID, this.Title, this.Fees);
        }
        private bool AddNewApplicationType() {

            this.ID = clsApplicationTypesDataAccess.AddNewApplicationType(this.Title, this.Fees);
            return (this.ID != -1);
        }
        public bool Save() {

            switch (_Mode) {

                case enMode.AddNew:

                    if (AddNewApplicationType())
                    {

                        _Mode = enMode.Update;
                        return true;
                    }
                    else {

                        return false;
                    }

                case enMode.Update:
                    return UpdateApplicationType();
                    
            }
            return false;
        }
    }
}
