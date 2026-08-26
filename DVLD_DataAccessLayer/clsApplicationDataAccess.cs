using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccessLayer
{
    public class clsApplicationDataAccess {

        static public DataTable GetAllApplication() {


            DataTable ApplicationTable = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM Applications;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {

                    ApplicationTable.Load(Reader);
                }
                Reader.Close();
            }
            catch (Exception Ex)
            {
                clsEventLogger.LogError(Ex, "GetAllApplication Method Error");
                //Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }

            return ApplicationTable;
        }
        public static bool GetApplicationInfoByID(int ApplicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID, ref byte ApplicationStatus, ref DateTime LastStatusDate, ref float PaidFees, ref int CreatedByUserID)
        {

            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM Applications WHERE ApplicationID = @ApplicationID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    isFound = true;

                    ApplicantPersonID = (int)Reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)Reader["ApplicationDate"];
                    ApplicationTypeID = (int)Reader["ApplicationTypeID"];
                    ApplicationStatus = (byte)Reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)Reader["LastStatusDate"];
                    PaidFees = Convert.ToSingle(Reader["PaidFees"]);
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
                clsEventLogger.LogError(Ex, "GetApplicationInfoByID Method Error");
                //Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }

            return isFound;
        }
        static public int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, float PaidFees,  int CreatedByUserID) {

            int ApplicationID = -1;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"INSERT INTO Applications (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
            VALUES (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID)
            SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


            try
            {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                    ApplicationID = ID;
                else
                    ApplicationID = -1;
            }
            catch (Exception Ex)
            {

                ApplicationID = -1;
                clsEventLogger.LogError(Ex, "AddNewApplication");
                //Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }

            return ApplicationID;
        }
        static public bool UpdateApplication(int ApplicationID, byte ApplicationStatus, DateTime LastStatusDate) {

            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"UPDATE Applications
                            SET ApplicationStatus = @ApplicationStatus
                               ,LastStatusDate = @LastStatusDate
                             WHERE ApplicationID = @ApplicationID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);


            try
            {

                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {

                clsEventLogger.LogError(Ex, "UpdateApplication Method error.");
                // Console.WriteLine($"Error: {Ex.Message}.");
                return false;
            }
            finally
            {

                Connection.Close();
            }
            return (RowAffected > 0);
        }
        static public bool DeleteApplication(int ApplicationID)
        {

            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"DELETE FROM Applications WHERE ApplicationID = @ApplicationID;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {

                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {

                clsEventLogger.LogError(Ex, "DeleteApplication Method error.");
                //Console.WriteLine($"Error: {Ex.Message}.");
                return false;
            }
            finally
            {

                Connection.Close();
            }
            return (RowAffected > 0);
        }
        static public bool IsExistApplication(int ApplicationID)
        {

            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT Found = 1 FROM Applications WHERE ApplicationID = @ApplicationID;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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
                clsEventLogger.LogError(Ex, "IsExistApplication Method error.");
                //Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }
            return IsFound;
        }
        
        // --------------------------------------------------------
        static public int GetActiveApplicationID(int ApplicantPersonID, int ApplicationTypeID) {

            int ApplicationID = -1;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT ActiveApplicationID = ApplicationID FROM Applications
            WHERE ApplicantPersonID = @ApplicantPersonID AND ApplicationStatus = 1 AND ApplicationTypeID = @ApplicationTypeID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                    ApplicationID = ID;
                else
                    ApplicationID = -1;
            }
            catch (Exception Ex)
            {

                ApplicationID = -1;
                clsEventLogger.LogError(Ex, "GetActiveApplicationID Method error.");
                //Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }

            return ApplicationID;
        }
        static public int GetActiveApplicationIDForLicenseClass(int ApplicantPersonID, int ApplicationTypeID, int LicenseClassID)
        {

            int ApplicationID = -1;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT ActiveApplicationID = Applications.ApplicationID From Applications 
            INNER JOIN LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
            WHERE ApplicantPersonID = @ApplicantPersonID and ApplicationTypeID = @ApplicationTypeID 
            and LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID and ApplicationStatus = 1;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                    ApplicationID = ID;
                else
                    ApplicationID = -1;
            }
            catch (Exception Ex)
            {

                ApplicationID = -1;
                clsEventLogger.LogError(Ex, "GetActiveApplicationIDForLicenseClass Method error.");
                //Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }

            return ApplicationID;
        }
        static public bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID) {

            return (GetActiveApplicationID(PersonID, ApplicationTypeID) != -1);
        }
    }
}




// i don't know why the instructor create this method.

//public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
//             byte ApplicationStatus, DateTime LastStatusDate,
//             float PaidFees, int CreatedByUserID)
//{

//    int rowsAffected = 0;
//    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

//    string query = @"Update  Applications  
//                            set ApplicantPersonID = @ApplicantPersonID,
//                                ApplicationDate = @ApplicationDate,
//                                ApplicationTypeID = @ApplicationTypeID,
//                                ApplicationStatus = @ApplicationStatus, 
//                                LastStatusDate = @LastStatusDate,
//                                PaidFees = @PaidFees,
//                                CreatedByUserID=@CreatedByUserID
//                            where ApplicationID=@ApplicationID";

//    SqlCommand command = new SqlCommand(query, connection);

//    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
//    command.Parameters.AddWithValue("ApplicantPersonID", @ApplicantPersonID);
//    command.Parameters.AddWithValue("ApplicationDate", @ApplicationDate);
//    command.Parameters.AddWithValue("ApplicationTypeID", @ApplicationTypeID);
//    command.Parameters.AddWithValue("ApplicationStatus", @ApplicationStatus);
//    command.Parameters.AddWithValue("LastStatusDate", @LastStatusDate);
//    command.Parameters.AddWithValue("PaidFees", @PaidFees);
//    command.Parameters.AddWithValue("CreatedByUserID", @CreatedByUserID);


//    try
//    {
//        connection.Open();
//        rowsAffected = command.ExecuteNonQuery();

//    }
//    catch (Exception ex)
//    {
//        //Console.WriteLine("Error: " + ex.Message);
//        return false;
//    }

//    finally
//    {
//        connection.Close();
//    }

//    return (rowsAffected > 0);
//}


