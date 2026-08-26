using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsLicenseClassDataAccess {


        static public DataTable GetAllLicenseClasses() {

            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM LicenseClasses;";
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
            finally
            {

                Connection.Close();
            }
            return dt;
        }
        static public bool GetLicenseClassByID(int LicenseClassID, ref string ClassName, ref string ClassDescription, ref byte MinAllowdAge, ref byte DefaultValidityLength, ref float Fees)
        {

            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    IsFound = true;
                    ClassName = (string)Reader["ClassName"];
                    ClassDescription = (string)Reader["ClassDescription"];
                    MinAllowdAge = (byte)Reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)Reader["DefaultValidityLength"];
                    Fees = Convert.ToSingle(Reader["ClassFees"]);

                }
                else
                    IsFound = false;

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
        static public bool GetLicenseClassByClassName(string ClassName, ref int LicenseClassID, ref string ClassDescription, ref byte MinAllowdAge, ref byte DefaultValidityLength, ref float Fees)
        {

            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM LicenseClasses WHERE ClassName = @ClassName;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ClassName", ClassName);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    IsFound = true;
                    LicenseClassID = (int)Reader["LicenseClassID"];
                    ClassDescription = (string)Reader["ClassDescription"];
                    MinAllowdAge = (byte)Reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)Reader["DefaultValidityLength"];
                    Fees = Convert.ToSingle(Reader["ClassFees"]);

                }
                else
                    IsFound = false;

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
        static public int AddNewLicenseClass(string ClassName, string ClassDescription, byte MinAllowdAge, byte DefaultValidityLength, float Fees) {

            int LicenseClassID = -1;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"INSERT INTO LicenseClasses
            (ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees)
            VALUES
            (@ClassName, @ClassDescription, @MinimumAllowedAge, @DefaultValidityLength, @ClassFees)
            SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ClassName", ClassName);
            Command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            Command.Parameters.AddWithValue("@MinimumAllowedAge", MinAllowdAge);
            Command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            Command.Parameters.AddWithValue("@ClassFees", Fees);

            try { 
            
                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                    LicenseClassID = ID;
                else
                    LicenseClassID = -1;
            }
            catch (Exception Ex) {

                LicenseClassID = -1;
                Console.WriteLine($"Error: [{Ex.Message}].");
            }
            finally {

                Connection.Close();
            }

            return LicenseClassID;
        }
        static public bool UpdateLicenseClass(int LicenseClassID, string ClassName, string ClassDescription, byte MinAllowdAge, byte DefaultValidityLength, float Fees) {


            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"UPDATE LicenseClasses
            SET  ClassName = @ClassName
               , ClassDescription = @ClassDescription
               , MinimumAllowedAge = @MinimumAllowedAge
               , DefaultValidityLength = @DefaultValidityLength
               , ClassFees = @ClassFees
            WHERE LicenseClassID = @LicenseClassID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ClassName", ClassName);
            Command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            Command.Parameters.AddWithValue("@MinimumAllowedAge", MinAllowdAge);
            Command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            Command.Parameters.AddWithValue("@ClassFees", Fees);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try { 
            
                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();
            }
            catch (Exception Ex) {

                Console.WriteLine($"Error: [{Ex.Message}].");
                return false;
            }
            finally { 
                
                Connection.Close();
            }

            return (RowAffected > 0);
        }
    }
}
