using DVLDBussinessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace DVLD_Project
{
    public class clsGlobal {

        static public clsUsers CurrentUserInfo;
        static string KeyPath = @"HKEY_CURRENT_USER\Software\DVLD_System";
        static string ValueUsername = "Username";
        static string ValuePassword = "Password";

        public static bool RememberUsernameAndPassword(string Username, string Password)
        {

            //string KeyPath = @"HKEY_CURRENT_USER\Software\DVLD_System";
            //string ValueUsername = "Username";
            //string ValuePassword = "Password";

            try {

                Registry.SetValue(KeyPath, ValueUsername, Username, RegistryValueKind.String);
                Registry.SetValue(KeyPath, ValuePassword, Password, RegistryValueKind.String);
                return true;
            }
            catch (Exception ex) {

                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }


            //try
            //{
            //    //this will get the current project directory folder.
            //    string currentDirectory = System.IO.Directory.GetCurrentDirectory();


            //    // Define the path to the text file where you want to save the data
            //    string filePath = currentDirectory + "\\data.txt";

            //    //incase the username is empty, delete the file
            //    if (Username == "" && File.Exists(filePath))
            //    {
            //        File.Delete(filePath);
            //        return true;

            //    }

            //    // concatonate username and passwrod withe seperator.
            //    string dataToSave = Username + "#//#" + Password;

            //    // Create a StreamWriter to write to the file
            //    using (StreamWriter writer = new StreamWriter(filePath))
            //    {
            //        // Write the data to the file
            //        writer.WriteLine(dataToSave);

            //        return true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"An error occurred: {ex.Message}");
            //    return false;
            //}

        }
        public static bool GetStoredCredential(ref string Username, ref string Password) {

            string KeyPath = @"HKEY_CURRENT_USER\Software\DVLD_System";
            try {

                string TestUsername = Registry.GetValue(KeyPath, ValueUsername, null) as string;
                if (TestUsername != null) {

                    Username = TestUsername;
                }
                else
                {

                    return false;
                }


                string TestPassword = Registry.GetValue(KeyPath, ValuePassword, null) as string;
                if (TestPassword != null)
                {

                    Password = TestPassword;
                }
                else {

                    return false;
                }
                    
                return true;
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }




            ////this will get the stored username and password and will return true if found and false if not found.
            //try
            //{
            //    //gets the current project's directory
            //    string currentDirectory = System.IO.Directory.GetCurrentDirectory();

            //    // Path for the file that contains the credential.
            //    string filePath = currentDirectory + "\\data.txt";

            //    // Check if the file exists before attempting to read it
            //    if (File.Exists(filePath))
            //    {
            //        // Create a StreamReader to read from the file
            //        using (StreamReader reader = new StreamReader(filePath))
            //        {
            //            // Read data line by line until the end of the file
            //            string line;
            //            while ((line = reader.ReadLine()) != null)
            //            {
            //                Console.WriteLine(line); // Output each line of data to the console
            //                string[] result = line.Split(new string[] { "#//#" }, StringSplitOptions.None);

            //                Username = result[0];
            //                Password = result[1];
            //            }
            //            return true;
            //        }
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"An error occurred: {ex.Message}");
            //    return false;
            //}
        }
    }
}
