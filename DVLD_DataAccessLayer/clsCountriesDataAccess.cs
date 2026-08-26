using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsCountriesDataAccess {

        static public DataTable GetAllCountries() { 
            
            DataTable CountriesTable = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM Countries;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {

                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                    CountriesTable.Load(Reader);

                Reader.Close();
            }
            catch (Exception Ex)
            {

                Console.WriteLine($"Error: {Ex.Message}.");
            }
            finally {

                Connection.Close();
            }
            return CountriesTable;
        }
        static public bool GetCountryInfoByID(int CountryID, ref string CountryName) {

            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM Countries WHERE CountryID = @CountryID;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@CountryID", CountryID);

            try { 
                
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    isFound = true;
                    CountryName = (string)Reader["CountryName"];
                }
                else { 
                
                    isFound = false;
                }
                    Reader.Close();
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
        static public bool GetCountryInfoByName(string CountryName, ref int CountryID)
        {

            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataSettings.ConnectionString);
            string Query = @"SELECT * FROM Countries WHERE CountryName = @CountryName;";
            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {

                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {

                    isFound = true;
                    CountryID = (int)Reader["CountryID"];
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
    }
}
