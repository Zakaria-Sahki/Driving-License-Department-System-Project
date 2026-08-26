using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project
{
    public class clsUtil {

        public static string GenerateGUID() {

            Guid newGuid = Guid.NewGuid();
            return newGuid.ToString();
        }
        public static bool CreateFolderIfDoesNotExist(string FolderPath) {

            if (!Directory.Exists(FolderPath))
            {
                try {

                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch (Exception Ex){

                    MessageBox.Show("Error creating folder: " + Ex.Message);
                    return false;
                }
                
            }
            return true;
        }
        public static string ReplaceFileNameWithGUID(string sourceFile) { 
            
            string FileName = sourceFile;
            FileInfo file = new FileInfo(FileName);
            string Extenction = file.Extension;
            return GenerateGUID() + Extenction;
        }
        public static bool CopyImageToProjectImagesFolder(ref string SourceFile) {

            // this funciton will copy the image to the
            // project images foldr after renaming it
            // with GUID with the same extention, then it will update the sourceFileName with the new name.

            string DestinationFolder = @"C:\DVLD-People-Images\";

            if (!CreateFolderIfDoesNotExist(DestinationFolder))
                return false;

            string DestinationFile = DestinationFolder + ReplaceFileNameWithGUID(SourceFile);
            try {

                File.Copy(SourceFile, DestinationFile, true);
            }
            catch (IOException iox) {

                MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SourceFile = DestinationFile;
            return true;
        }
    }
}
