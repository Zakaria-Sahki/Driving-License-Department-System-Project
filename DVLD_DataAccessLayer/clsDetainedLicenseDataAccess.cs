using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsDetainedLicenseDataAccess {


        static public DataTable GetAllDetainedLicense() {

            DataTable dtDetainedLicenses = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM DetainedLicenses_View;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {

                    dtDetainedLicenses.Load(Reader);
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

            return dtDetainedLicenses;
        }
        static public bool IsDetainedLicense(int LicenseID) {

            bool IsDetained = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT Found = 1 FROM DetainedLicenses WHERE LicenseID = @LicenseID AND IsReleased = 0;";
            SqlCommand Command = new SqlCommand(Query, Connection);Command.Parameters.AddWithValue("@LicenseID", LicenseID);


            try
            {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null)
                    IsDetained = true;
                else
                    IsDetained = false;
                
            }
            catch (Exception Ex)
            {
                IsDetained = false;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }

            return IsDetained;
        }
        static public int AddNewDetainedLicense(int LicenseID, DateTime DetainDate, float FineFees, int CreatedByUserID) {

            int DetainID = -1;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"INSERT INTO DetainedLicenses
                                (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased)
                                VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, 0);
                                SELECT SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@DetainDate", DetainDate);
            Command.Parameters.AddWithValue("@FineFees", FineFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


            try
            {

                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                    DetainID = ID;
                else
                    DetainID = -1;
            }
            catch (Exception Ex)
            {

                DetainID = -1;
                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally
            {

                Connection.Close();
            }

            return DetainID;
        }
        static public bool UpdateDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, float FineFees, int CreatedByUserID) {

            int RowAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"UPDATE DetainedLicenses
                            SET  LicenseID = @LicenseID
                                  , DetainDate = @DetainDate
                                  , FineFees = @FineFees
                                  , CreatedByUserID = @CreatedByUserID
                            WHERE DetainID = @DetainID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DetainID", DetainID);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@DetainDate", DetainDate);
            Command.Parameters.AddWithValue("@FineFees", FineFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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
        static public bool ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID) {

            int RowAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"UPDATE DetainedLicenses
                            SET   IsReleased = 1
                                  , ReleaseDate = @ReleaseDate
                                  , ReleasedByUserID = @ReleasedByUserID
                                  , ReleaseApplicationID = @ReleaseApplicationID
                            WHERE DetainID = @DetainID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DetainID", DetainID);
            Command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            Command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            Command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);

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
        static public bool FindDetainedLicenseByDetainID(int DetainID, ref int LicenseID, ref DateTime DetainDate, ref float FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID) {


            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID;";
            SqlCommand Command = new SqlCommand(Query, Connection); Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@DetainID", DetainID);


            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    IsFound = true;

                    LicenseID = (int)Reader["LicenseID"];
                    DetainDate = (DateTime)Reader["DetainDate"];
                    FineFees = Convert.ToSingle(Reader["FineFees"]);
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsReleased = (bool)Reader["IsReleased"];

                    if (IsReleased == false)
                    {

                        ReleaseDate = DateTime.MaxValue;
                        ReleasedByUserID = -1;
                        ReleaseApplicationID = -1;
                    }
                    else {

                        ReleaseDate = (DateTime)Reader["ReleaseDate"];
                        ReleasedByUserID = (int)Reader["ReleasedByUserID"];
                        ReleaseApplicationID = (int)Reader["ReleaseApplicationID"];
                    }
                        

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
        static public bool FindDetainedLicenseByLicenseID(int LicenseID, ref int DetainID, ref DateTime DetainDate, ref float FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {


            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT top 1 * FROM DetainedLicenses WHERE LicenseID = @LicenseID order by DetainID desc";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);


            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    IsFound = true;

                    DetainID = (int)Reader["DetainID"];
                    DetainDate = (DateTime)Reader["DetainDate"];
                    FineFees = Convert.ToSingle(Reader["FineFees"]);
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsReleased = (bool)Reader["IsReleased"];

                    if (IsReleased == false)
                    {

                        ReleaseDate = DateTime.MaxValue;
                        ReleasedByUserID = -1;
                        ReleaseApplicationID = -1;
                    }
                    else {

                        ReleaseDate = (DateTime)Reader["ReleaseDate"];
                        ReleasedByUserID = (int)Reader["ReleasedByUserID"];
                        ReleaseApplicationID = (int)Reader["ReleaseApplicationID"];
                    }
                        

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
    }
}


