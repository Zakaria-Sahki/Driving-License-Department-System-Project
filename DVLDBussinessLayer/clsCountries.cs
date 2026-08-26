using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBussinessLayer
{
    public class clsCountries {

        public int _CountryID { get; set; }
        public string _CountryName { get; set; }
        public clsCountries() { 
            
            _CountryID = 0;
            _CountryName = "";
        }
        private clsCountries(int CountryID, string CountryName) { 
            
            _CountryID = CountryID;
            _CountryName = CountryName;
        }
        static public DataTable GetAllCountries() {

            return clsCountriesDataAccess.GetAllCountries();
        }
        static public clsCountries Find(int CountryID) {

            string CountryName = "";
            if (clsCountriesDataAccess.GetCountryInfoByID(CountryID, ref CountryName))
            {

                return new clsCountries(CountryID, CountryName);
            }
            else
                return null;
        }
        static public clsCountries Find(string CountryName)
        {

            int CountryID = -1;
            if (clsCountriesDataAccess.GetCountryInfoByName(CountryName, ref CountryID))
            {

                return new clsCountries(CountryID, CountryName);
            }
            else
                return null;
        }
    }
}
