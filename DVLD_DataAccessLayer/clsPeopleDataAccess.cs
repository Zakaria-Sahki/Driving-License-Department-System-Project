using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsPeopleDataAccess {

        public static DataTable GetAllPeople() { 
            
            DataTable PeopleTable = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, People.ThirdName, People.LastName,
            People.DateOfBirth, People.Gendor,  
            CASE
	            WHEN People.Gendor = 0 THEN 'Male'
	            ELSE 'Female'
            END as GendorCaption ,People.Address, People.Phone, People.Email, 
            People.NationalityCountryID, Countries.CountryName, People.ImagePath
            FROM People INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID
            ORDER BY People.FirstName";
            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {

                    PeopleTable.Load(Reader);
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

            return PeopleTable;
        }
        public static bool GetPersonInfoByPersonID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref byte Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath) { 
            
            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM People WHERE PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    isFound = true;
                    NationalNo = (string)Reader["NationalNo"];
                    FirstName = (string)Reader["FirstName"];
                    LastName = (string)Reader["LastName"];
                    DateOfBirth = (DateTime)Reader["DateOfBirth"];
                    Gendor = (byte)Reader["Gendor"];
                    Address = (string)Reader["Address"];
                    Phone = (string)Reader["Phone"];
                    NationalityCountryID = (int)Reader["NationalityCountryID"];

                    if (Reader["SecondName"] == System.DBNull.Value)
                        SecondName = "";
                    else
                        SecondName = (string)Reader["SecondName"];

                    if (Reader["ThirdName"] == System.DBNull.Value)
                        ThirdName = "";
                    else
                        ThirdName = (string)Reader["ThirdName"];


                    if (Reader["Email"] == System.DBNull.Value)
                        Email = "";
                    else
                        Email = (string)Reader["Email"];



                    if (Reader["ImagePath"] == System.DBNull.Value)
                        ImagePath = "";
                    else
                        ImagePath = (string)Reader["ImagePath"];

                }
                else { 
                    
                    isFound = false;
                }
                Reader.Close();
            }
            catch (Exception Ex)
            {
                isFound = false;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally {

                Connection.Close();
            }

            return isFound;
        }
        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref DateTime DateOfBirth, ref byte Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {

            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM People WHERE NationalNo = @NationalNo";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    isFound = true;
                    PersonID = (int)Reader["PersonID"];
                    FirstName = (string)Reader["FirstName"];
                    LastName = (string)Reader["LastName"];
                    DateOfBirth = (DateTime)Reader["DateOfBirth"];
                    Gendor = (byte)Reader["Gendor"];
                    Address = (string)Reader["Address"];
                    Phone = (string)Reader["Phone"];
                    NationalityCountryID = (int)Reader["NationalityCountryID"];

                    if (Reader["SecondName"] == System.DBNull.Value)
                        SecondName = "";
                    else
                        SecondName = (string)Reader["SecondName"];

                    if (Reader["ThirdName"] == System.DBNull.Value)
                        ThirdName = "";
                    else
                        ThirdName = (string)Reader["ThirdName"];


                    if (Reader["Email"] == System.DBNull.Value)
                        Email = "";
                    else
                        Email = (string)Reader["Email"];



                    if (Reader["ImagePath"] == System.DBNull.Value)
                        ImagePath = "";
                    else
                        ImagePath = (string)Reader["ImagePath"];

                }
                else
                {

                    isFound = false;
                }
                Reader.Close();
            }
            catch (Exception Ex)
            {
                isFound = false;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }

            return isFound;
        }
        static public int AddNewPerson(string NationalNo, string FirstName, string SecondName,
            string ThirdName, string LastName, DateTime DateOfBirth, byte Gendor,
            string Address, string Phone, int NationalIDCountry, string Email, string ImagePath) {

            int PersonID = -1;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"INSERT INTO People(NationalNo, FirstName, SecondName, ThirdName,
            LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath)
            VALUES(@NationalNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth, @Gendor, @Address, @Phone, @Email, @Nationality, @ImagePath)
            Select SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gendor", Gendor);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);
            Command.Parameters.AddWithValue("@Nationality", NationalIDCountry);

            // SecondName, ThirdName, Email, ImagePath allow null so: 

            if (SecondName != "" && SecondName != null)
                Command.Parameters.AddWithValue("@SecondName", SecondName);
            else
                Command.Parameters.AddWithValue("@SecondName", System.DBNull.Value);

            if (ThirdName != "" && ThirdName != null)
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                Command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            if (Email != "" && Email != null)
                Command.Parameters.AddWithValue("@Email", Email);
            else
                Command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            if (ImagePath != "" && ImagePath != null)
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                Command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);



            try {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                    PersonID = ID;
                else
                    PersonID = -1;
            }
            catch (Exception Ex) {

                PersonID = -1;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally { 
            
                Connection.Close();
            }

            return PersonID;
        }
        static public bool UpdatePerson(int PersonID, string NationalNo, string FirstName,
            string SecondName, string ThirdName, string LastName, DateTime DateOfBirth,
            byte Gendor, string Address, string Phone,
            string Email, int NationalityCountryID, string ImagePath) {

            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"UPDATE People
            SET NationalNo = @NationalNo, FirstName = @FirstName,
            SecondName = @SecondName, ThirdName = @ThirdName,
            LastName = @LastName, DateOfBirth = @DateOfBirth,
            Gendor = @Gendor, Address = @Address, Phone = @Phone,
            Email = @Email, NationalityCountryID = @NationalityCountryID,
            ImagePath = @ImagePath
            WHERE PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gendor", Gendor);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);
            Command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);


            if (SecondName != "" && SecondName != null)
                Command.Parameters.AddWithValue("@SecondName", SecondName);
            else
                Command.Parameters.AddWithValue("@SecondName", System.DBNull.Value);

            if (ThirdName != "" && ThirdName != null)
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                Command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            if (Email != "" && Email != null)
                Command.Parameters.AddWithValue("@Email", Email);
            else
                Command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            if (ImagePath != "" && ImagePath != null)
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            else
                Command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);



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
        static public bool DeletePerson(int PersonID) {

            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"DELETE FROM People WHERE PersonID = @PersonID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try { 
            
                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {

                Console.WriteLine($"Error: {Ex.Message}.");
                return false;
            }
            finally { 
                
                Connection.Close();
            }
            return (RowAffected > 0);
        }
        static public bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT Found = 1 FROM People WHERE NationalNo = @NationalNo;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null)
                {
                    isFound = true;
                }
                else
                {

                    isFound = false;
                }

            }
            catch (Exception Ex)
            {

                isFound = false;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }
            return isFound;
        }
        static public bool IsPersonExist(int PersonID)
        {
            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT Found = 1 FROM People WHERE PersonID = @PersonID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null)
                {
                    isFound = true;
                }
                else
                {

                    isFound = false;
                }

            }
            catch (Exception Ex)
            {

                isFound = false;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }
            return isFound;
        }
    }
}
