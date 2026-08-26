using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsUsersDataAccess {

        static public DataTable GetAllUsers() { 
        
            DataTable UsersTable = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM Users;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {

                    UsersTable.Load(Reader);
                }
                Reader.Close();
            }
            catch (Exception Ex)
            {

                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally {

                Connection.Close();
            }
            return UsersTable;
        }
        static public bool GetUserInfoByID(int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool IsActive) {

            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM Users
            WHERE UserID = @UserID;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserID", UserID);

            try {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {

                    IsFound = true;
                    PersonID = (int)Reader["PersonID"];
                    UserName = (string)Reader["UserName"];
                    Password = (string)Reader["Password"];
                    IsActive = (bool)Reader["IsActive"];
                }
                else { 
                    
                    IsFound = false;
                }
                Reader.Close();
            }
            catch (Exception Ex)
            {
                IsFound = false;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        static public bool GetUserInfoByUserName(string UserName, ref int UserID, ref int PersonID, ref string Password, ref bool IsActive)
        {

            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM Users
            WHERE Users.UserName = @UserName;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserName", UserName);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {

                    IsFound = true;
                    PersonID = (int)Reader["PersonID"];
                    UserID = (int)Reader["UserID"];
                    Password = (string)Reader["Password"];
                    IsActive = (bool)Reader["IsActive"];
                }
                else
                {

                    IsFound = false;
                }
                Reader.Close();
            }
            catch (Exception Ex)
            {
                IsFound = false;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        static public bool GetUserInfoByUserNameAndPassword(string UserName, string Password, ref int UserID, ref int PersonID, ref bool IsActive)
        {

            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM Users WHERE (Users.UserName = @UserName AND Users.Password = @Password);";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {

                    IsFound = true;
                    PersonID = (int)Reader["PersonID"];
                    UserID = (int)Reader["UserID"];
                    IsActive = (bool)Reader["IsActive"];
                }
                else
                {

                    IsFound = false;
                }
                Reader.Close();
            }
            catch (Exception Ex)
            {
                IsFound = false;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
        static public bool IsUserExist(int UserID) {

            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT Found = 1 FROM Users WHERE UserID = @UserID;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserID", UserID);

            try { 
            
                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null)
                    IsFound = true;
                else
                    IsFound = false;
            }
            catch (Exception Ex) { 
            
                IsFound = false;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally { 
                
                Connection.Close();
            }
            return IsFound;
        }
        static public bool IsUserExist(string UserName)
        {

            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT Found = 1 FROM Users WHERE UserName = @UserName;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserName", UserName);

            try
            {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null)
                    IsFound = true;
                else
                    IsFound = false;
            }
            catch (Exception Ex)
            {

                IsFound = false;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }
            return IsFound;
        }
        static public bool IsUserExist(string UserName, string Password)
        {

            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT Found = 1 FROM Users WHERE (UserName = @UserName AND Users.Password = @Password);";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);

            try
            {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null)
                    IsFound = true;
                else
                    IsFound = false;
            }
            catch (Exception Ex)
            {

                IsFound = false;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }
            return IsFound;
        }
        static public bool IsUserExistByPersonID(int PersonID)
        {

            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT Found = 1 FROM Users WHERE (PersonID = @PersonID);";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null)
                    IsFound = true;
                else
                    IsFound = false;
            }
            catch (Exception Ex)
            {

                IsFound = false;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }
            return IsFound;
        }
        static public int AddNewUser(int PersonID, string UserName, string Password, bool IsActive) {


            int _UserID = -1;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"INSERT INTO Users (PersonID, UserName, Password, IsActive)
            VALUES (@PersonID, @UserName, @Password, @IsActive)
            SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);
            Command.Parameters.AddWithValue("@IsActive", IsActive);

            try { 
                
                Connection.Open();
                object result = Command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int ID))
                    _UserID = ID;
                else
                    _UserID = -1;


            }
            catch (Exception Ex) {

                _UserID = -1;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally { 
                
                Connection.Close();
            }
            return _UserID;
        }
        static public bool UpdateUser(int UserID, string UserName, string Password, bool IsActive) {

            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"UPDATE Users
                            SET UserName = @UserName
                               ,Password = @Password
                               ,IsActive = @IsActive
                            WHERE UserID = @UserID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserID", UserID);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);
            Command.Parameters.AddWithValue("@IsActive", IsActive);


            try { 
                
                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();
            }
            catch (Exception Ex) {

                Console.WriteLine($"Error: {Ex.Message}.");
                return false;
            }
            finally { 
            
                Connection.Close();
            }
            return (RowAffected > 0);
        }
        static public bool DeleteUser(int UserID) {

            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"DELETE FROM Users WHERE UserID = @UserID;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserID", UserID);

            try {

                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();
            }
            catch (Exception Ex) {

                Console.WriteLine($"Error: {Ex.Message}.");
                return false;
            }
            finally { 
            
                Connection.Close();
            }
            return (RowAffected > 0);
        }
        static public bool ChangePasswordOfUser(int UserID, string Password) {

            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"UPDATE Users SET Password = @Password WHERE UserID = @UserID;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@UserID", UserID);
            Command.Parameters.AddWithValue("@Password", Password);

            try
            {

                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {

                Console.WriteLine($"Error: {Ex.Message}.");
                return false;
            }
            finally
            {

                Connection.Close();
            }
            return (RowAffected > 0);
        }


        // GetUserInfoByPersonID
        // in GetAllUsers Hadhoud retrive the full name from the data base, in the other hand i make it in the bussiness layer using the property fullname from clsPeople
    }
}
