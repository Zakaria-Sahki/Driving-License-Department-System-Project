using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsApplicationTypesDataAccess {

        static public DataTable GetAllApplicationTypes() { 
            
            DataTable AppTypesDt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM ApplicationTypes;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            try {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows) {

                    AppTypesDt.Load(Reader);
                }
                Reader.Close();
            }
            catch (Exception Ex) {

                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally {

                Connection.Close();
            }
            return AppTypesDt;
        }
        static public bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, float ApplicationFees) {

            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"UPDATE ApplicationTypes SET ApplicationTypeTitle = @ApplicationTypeTitle
                ,ApplicationFees = @ApplicationFees
                WHERE ApplicationTypeID = @ApplicationTypeID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);
            Command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);

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
        static public bool GetApplicationTypeInfo(int ApplicationTypeID, ref string ApplicationTypeTitle, ref float ApplicationFees) {

            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    isFound = true;
                    ApplicationTypeTitle = (string)Reader["ApplicationTypeTitle"];
                    ApplicationFees = Convert.ToSingle(Reader["ApplicationFees"]);
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
        static public int AddNewApplicationType(string ApplicationTypeTitle, float ApplicationFees)
        {

            int ID = -1;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"INSERT INTO ApplicationTypes (ApplicationTypeTitle, ApplicationFees)
            VALUES (@ApplicationTypeTitle, @ApplicationFees)
            SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);
            Command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);

            try
            {
                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int AppTypeID))
                {

                    ID = AppTypeID;
                }
                else { 
                    
                    ID = -1;
                }
            }
            catch (Exception Ex)
            {

                ID = -1;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }

            return ID;
        }

    }
}


