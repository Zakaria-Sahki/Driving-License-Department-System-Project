using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBussinessLayer
{
    public class clsUsers {

        enum enMode { AddUser, UpdateUser};
        enMode _Mode = enMode.AddUser;

        public int _UserID { get; set;}
        public int _PersonID { get; set;}
        public string _UserName { get; set;}
        public string _Password { get; set;}
        public bool _IsActive { get; set;}

        public clsPeople _PersonInfo;


        public clsUsers() {

            _UserID = -1;
            _PersonID = -1;
            _UserName = "";
            _Password = "";
            _IsActive = false;

            _Mode = enMode.AddUser;
        }
        private clsUsers(int UserID, int PersonID, string UserName, string Password, bool IsActive) { 
        
            _UserID = UserID;
            _PersonID = PersonID;
            _UserName = UserName;
            _Password = Password;
            _IsActive = IsActive;
            _PersonInfo = clsPeople.FindPerson(PersonID);


            _Mode = enMode.UpdateUser;
        }

        static public DataTable GetAllUsers() {

            DataTable UsersTable = clsUsersDataAccess.GetAllUsers();

            UsersTable.Columns.Add("FullName", typeof(string));

            foreach (DataRow Row in UsersTable.Rows)
            {
                Row["FullName"] = clsPeople.FindPerson((int)Row["PersonID"]).FullName;
            }
            return UsersTable;
        }
        static public clsUsers FindUser(int UserID) {

            int PersonID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;

            if (clsUsersDataAccess.GetUserInfoByID(UserID, ref PersonID, ref UserName, ref Password, ref IsActive))
            {

                return new clsUsers(UserID, PersonID, UserName, Password, IsActive);
            }
            else
                return null;
        }
        static public clsUsers FindUser(string UserName)
        {

            int PersonID = -1, UserID = -1;
            string Password = "";
            bool IsActive = false;

            if (clsUsersDataAccess.GetUserInfoByUserName(UserName, ref UserID, ref PersonID, ref Password, ref IsActive))
            {

                return new clsUsers(UserID, PersonID, UserName, Password, IsActive);
            }
            else
                return null;
        }
        static public clsUsers FindUser(string UserName, string Password)
        {

            int PersonID = -1, UserID = -1;
            bool IsActive = false;

            if (clsUsersDataAccess.GetUserInfoByUserNameAndPassword(UserName, clsHashingPassword.ComputeHash(Password), ref UserID, ref PersonID, ref IsActive))
            {

                return new clsUsers(UserID, PersonID, UserName, Password, IsActive);
            }
            else
                return null;
        }
        static public bool IsExistUser(int UserID) {

            return clsUsersDataAccess.IsUserExist(UserID);
        }
        static public bool IsExistUser(string UserName)
        {

            return clsUsersDataAccess.IsUserExist(UserName);
        }
        static public bool IsExistUser(string UserName, string Password)
        {

            return clsUsersDataAccess.IsUserExist(UserName, clsHashingPassword.ComputeHash(Password));
        }
        static public bool IsExistUserByPersonID(int PersonID)
        {

            return clsUsersDataAccess.IsUserExistByPersonID(PersonID);
        }
        private bool AddNewUser() {

            this._UserID = clsUsersDataAccess.AddNewUser(this._PersonID, this._UserName, clsHashingPassword.ComputeHash(this._Password), this._IsActive);
            return (this._UserID != -1);
        }
        private bool UpdateUser() {

            return clsUsersDataAccess.UpdateUser(this._UserID, this._UserName, clsHashingPassword.ComputeHash(this._Password), this._IsActive);
        }
        public bool Save() {

            switch (_Mode) {

                case enMode.AddUser:
                    if (AddNewUser()) {

                        _Mode = enMode.UpdateUser;
                        return true;
                    }
                    break;
                case enMode.UpdateUser:
                    return UpdateUser();
            }
            return false;
        }
        static public bool DeleteUser(int UserID) {

            return clsUsersDataAccess.DeleteUser(UserID);
        }
        
        
        
        // Affected methods
        public bool ChangingPassword() {

            return clsUsersDataAccess.ChangePasswordOfUser(this._UserID, this._Password);
        }
    
    
        // find user by personId not implemented
    }
}
