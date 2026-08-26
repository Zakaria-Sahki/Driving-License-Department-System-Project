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
    public class clsTestAppointmentsDataAccess {

        static public DataTable GetTestAppointmentsInfoByLocalDLAppID_And_TestType(int LDL_AppID, int TestTypeID) {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT TestAppointments.TestAppointmentID, TestAppointments.AppointmentDate,
            TestAppointments.PaidFees, TestAppointments.IsLocked FROM TestAppointments
            WHERE TestAppointments.LocalDrivingLicenseApplicationID = @LDL_AppID AND TestAppointments.TestTypeID = @TestTypeID
            ORDER BY TestAppointmentID desc;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows) {

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
        static public int AddNewTestAppointment(int TestTypeID, int LDL_AppID, DateTime AppointmentDate, float PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestAppID)
        {

            int TestAppointmentID = -1;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"INSERT INTO TestAppointments 
            (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, 
            PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID)
            VALUES (@TestTypeID, @LDL_AppID, @AppointmentDate, @PaidFees, @CreatedByUserID, @IsLocked, @RetakeTestAppID);
            SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IsLocked", IsLocked);

            if (RetakeTestAppID != -1)
            {

                Command.Parameters.AddWithValue("@RetakeTestAppID", RetakeTestAppID);
            }
            else {

                Command.Parameters.AddWithValue("@RetakeTestAppID", System.DBNull.Value);
            }
                

            try {
                Connection.Open();
                object result = Command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int ID))
                {

                    TestAppointmentID = ID;
                }
                else
                    TestAppointmentID = -1;

            }
            catch (Exception Ex) {

                TestAppointmentID = -1;
                Console.WriteLine($"Error: [{Ex.Message}].");
            }
            finally { 
                
                Connection.Close();
            }

            return TestAppointmentID;
        }
        static public bool UpdateTestAppointment(int TestAppointmentID, DateTime AppointmentDate, bool IsLocked) {

            int RowAffected = -1;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"UPDATE TestAppointments
            SET    AppointmentDate = @AppointmentDate
                  ,IsLocked = @IsLocked
            WHERE TestAppointmentID = @TestAppointmentID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@IsLocked", IsLocked);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {

                Connection.Open();
                RowAffected = Command.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {

                RowAffected = -1;
                Console.WriteLine($"Error: [{Ex.Message}].");
            }
            finally { 
                
                Connection.Close();
            }
            return (RowAffected > 0);
        }
        static public bool GetTestAppointmentInfoByID(int TestAppointmentID, ref int TestTypeID, ref int LDL_AppID, ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestAppID) {

            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT* FROM TestAppointments WHERE TestAppointments.TestAppointmentID = @TestAppointmentID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try { 
                
                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                if (reader.Read()) { 
                    
                    isFound = true;
                    TestTypeID = (int)reader["TestTypeID"];
                    LDL_AppID = (int)reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = Convert.ToSingle(reader["PaidFees"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = (bool)reader["IsLocked"];


                    if (reader["RetakeTestApplicationID"] == System.DBNull.Value)
                        RetakeTestAppID = -1;
                    else
                        RetakeTestAppID = (int)reader["RetakeTestApplicationID"];
                }
                else
                    isFound = false;

                reader.Close();
            }
            catch (Exception Ex) {

                isFound = false;
                Console.WriteLine($"Error: [{Ex.Message}].");
            }
            finally { 
                
                Connection.Close();
            }

            return isFound;
        }
        static public bool IsActiveAppointmentExist(int LDL_AppID, int TestTypeID) {

            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT Found = 1 FROM TestAppointments 
            WHERE TestAppointments.LocalDrivingLicenseApplicationID = @LDL_AppID 
            AND TestAppointments.TestTypeID = @TestTypeID AND TestAppointments.IsLocked = 0;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null)
                    isFound = true;
                else
                    isFound = false;

            }
            catch (Exception Ex)
            {

                isFound = false;
                Console.WriteLine($"Error: [{Ex.Message}].");
            }
            finally
            {

                Connection.Close();
            }

            return isFound;
        }

        // -------------------------------
        static public bool GetLastTestAppointment(int LDL_AppID, int TestTypeID, ref int TestAppointmentID, ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestAppID)
        {

            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT top 1 * FROM TestAppointments
            WHERE TestAppointments.TestTypeID = @TestTypeID AND TestAppointments.LocalDrivingLicenseApplicationID = @LDL_AppID
            ORDER BY TestAppointments.TestAppointmentID DESC;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LDL_AppID", LDL_AppID);

            try
            {

                Connection.Open();
                SqlDataReader reader = Command.ExecuteReader();
                if (reader.Read())
                {

                    isFound = true;

                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = Convert.ToSingle(reader["PaidFees"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = (bool)reader["IsLocked"];


                    if (reader["RetakeTestApplicationID"] == System.DBNull.Value)
                        RetakeTestAppID = -1;
                    else
                        RetakeTestAppID = (int)reader["RetakeTestApplicationID"];
                }
                else
                    isFound = false;

                reader.Close();
            }
            catch (Exception Ex)
            {

                isFound = false;
                Console.WriteLine($"Error: [{Ex.Message}].");
            }
            finally
            {

                Connection.Close();
            }

            return isFound;
        }
        static public DataTable GetAllTestAppointments()
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM TestAppointments_View ORDER BY AppointmentDate DESC;";
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
        static public int GetTestID(int TestAppointmentID) {

            int TestID = -1;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT TestID FROM Tests WHERE Tests.TestAppointmentID = @TestAppointmentID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                    TestID = ID;
                else
                    TestID = -1;
            }
            catch (Exception Ex)
            {
                TestID = -1;
                Console.WriteLine($"Error: [{Ex.Message}].");
            }
            finally
            {

                Connection.Close();
            }
            return TestID;
        }
    }
}
