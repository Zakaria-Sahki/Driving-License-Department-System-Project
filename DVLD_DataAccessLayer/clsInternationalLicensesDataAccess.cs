using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsInternationalLicensesDataAccess {

        static public DataTable GetAllInternationalLicenses()
        {

            DataTable InternationaLicensesTable = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive FROM InternationalLicenses;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {

                    InternationaLicensesTable.Load(Reader);
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

            return InternationaLicensesTable;
        }
        static public DataTable GetDriverInternationalLicenses(int DriverID)
        {

            DataTable InternationaLicensesTable = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT InternationalLicenseID, ApplicationID, DriverID,
            IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive 
            FROM InternationalLicenses WHERE DriverID = @DriverID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {

                    InternationaLicensesTable.Load(Reader);
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

            return InternationaLicensesTable;
        }
        public static bool GetInternationalLicenseInfoByID(int InternationalLicenseID, ref int ApplicationID, ref int DriverID, ref int LocalLicenseID, ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
        {

            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"
                SELECT * FROM InternationalLicenses
                WHERE InternationalLicenseID = @InternationalLicenseID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    isFound = true;

                    ApplicationID = (int)Reader["ApplicationID"];
                    DriverID = (int)Reader["DriverID"];
                    LocalLicenseID = (int)Reader["IssuedUsingLocalLicenseID"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];
                    IsActive = (bool)Reader["IsActive"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];

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
        static public bool IsInternationalLicenseExist(int InternationalLicenseID)
        {

            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT Found = 1 FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

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
        static public int AddNewInternationalLicense(int ApplicationID, int DriverID, int LocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {

            int InternationalLicenseID = -1;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"
            
            Update InternationalLicenses 
                set IsActive = 0
            where DriverID = @DriverID;

            INSERT INTO InternationalLicenses
                (ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID)
            VALUES (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID, @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID);
            SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", LocalLicenseID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


            try
            {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                    InternationalLicenseID = ID;
                else
                    InternationalLicenseID = -1;
            }
            catch (Exception Ex)
            {

                InternationalLicenseID = -1;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }

            return InternationalLicenseID;
        }
        static public bool UpdateInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID, int LocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {

            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"UPDATE InternationalLicenses
                            SET ApplicationID = @ApplicationID
                                ,DriverID = @DriverID
                                ,IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID
                                ,IssueDate = @IssueDate
                                ,ExpirationDate = @ExpirationDate
                                ,IsActive = @IsActive
                                ,CreatedByUserID = @CreatedByUserID
                            WHERE InternationalLicenseID = @InternationalLicenseID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", LocalLicenseID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);


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
        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString);

            string query = @"  
                            SELECT Top 1 InternationalLicenseID
                            FROM InternationalLicenses 
                            where DriverID=@DriverID and GetDate() between IssueDate and ExpirationDate 
                            order by ExpirationDate Desc;";

            /*
             
                SELECT InternationalLicenseID FROM InternationalLicenses 
                WHERE DriverID = 9 AND IsActive = 1 AND (GETDATE() BETWEEN IssueDate AND ExpirationDate); 

             */

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return InternationalLicenseID;
        }
    }
}
