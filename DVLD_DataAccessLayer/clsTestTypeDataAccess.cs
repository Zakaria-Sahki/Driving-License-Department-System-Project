using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsTestTypeDataAccess {

        static public DataTable GetAllTestTypes()
        {

            DataTable dt = new DataTable();
            
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM TestTypes;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            
            
            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {

                    dt.Load(Reader);
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
        
            return dt;
        }
        static public bool GetTestTypeInfoByID(int TestTypeID, ref string TestTypeTitle, ref string TestTypeDescription, ref float TestTypeFees) {

            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    isFound = true;

                    TestTypeTitle = (string)Reader["TestTypeTitle"];
                    TestTypeDescription = (string)Reader["TestTypeDescription"];
                    TestTypeFees = Convert.ToSingle(Reader["TestTypeFees"]);
                }
                else { 
                    
                    isFound = false;
                }
            }
            catch (Exception Ex) {

                isFound = false;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally { 
                
                Connection.Close();
            }
            return isFound;
        }
        static public bool UpdateTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, float TestTypeFees) {

            int RowAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"UPDATE TestTypes
                             SET TestTypeTitle = @TestTypeTitle
                                ,TestTypeDescription = @TestTypeDescription
                                ,TestTypeFees = @TestTypeFees
                              WHERE TestTypeID = @TestTypeID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            Command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            Command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);

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
        static public int AddNewTestType(string TestTypeTitle, string TestTypeDescription, float TestTypeFees) {

            int TestTypeID = -1;

            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"INSERT INTO TestTypes (TestTypeTitle, TestTypeDescription, TestTypeFees)
            VALUES (@TestTypeTitle, @TestTypeDescription, @TestTypeFees);
            SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            Command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            Command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);

            try {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                {

                    TestTypeID = ID;
                }
                else {

                    TestTypeID = -1;
                }
            }
            catch (Exception Ex) {

                TestTypeID = -1;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally { 
                
                Connection.Close();
            }

            return TestTypeID;
        }
    }
}
