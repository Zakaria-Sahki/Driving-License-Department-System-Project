using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsDriverDataAccess {

        static public DataTable GetAllDrivers()
        {

            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM Drivers_View;";
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
        static public bool GetDriverInfoByID(int DriverID, ref int PersonID, ref DateTime CreatedDate, ref int CreatedByUserID)
        {

            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM Drivers WHERE DriverID = @DriverID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    IsFound = true;
                    PersonID = (int)Reader["PersonID"];
                    CreatedDate = (DateTime)Reader["CreatedDate"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
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
        static public bool GetDriverInfoByPersonID(int PersonID, ref int DriverID, ref DateTime CreatedDate, ref int CreatedByUserID)
        {

            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM Drivers WHERE PersonID = @PersonID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    IsFound = true;
                    DriverID = (int)Reader["DriverID"];
                    CreatedDate = (DateTime)Reader["CreatedDate"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
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
        static public int AddNewDriver(int PersonID, int CreatedByUserID) {

            int DriverID = -1;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"INSERT INTO Drivers(PersonID, CreatedByUserID, CreatedDate)
            VALUES (@PersonID, @CreatedByUserID, @CreatedDate)
            SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try { 
                
                Connection.Open();
                object result = Command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int ID))
                    DriverID = ID;
                else
                    DriverID = -1;
            }
            catch (Exception Ex) {

                DriverID = -1;
                Console.WriteLine($"Error: [{Ex.Message}].");
            }
            finally{ 
                
                Connection.Close();
            }
            return DriverID;
        }
        static public bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID)
        {

            int RowAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"UPDATE Drivers
            SET PersonID = @PersonID
                ,CreatedByUserID = @CreatedByUserID
            WHERE DriverID = @DriverID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);

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

    }
}
