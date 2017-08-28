using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Windows;

namespace Hide_Your_Files_Inside_a_Picture
{
    public static class Gtools
    {

        public static string compressFile(string dirPath, string filePath)
        {
            string zipPath = Path.Combine(dirPath, "dummy.zip");

            try
            {
                using (ZipFile zip = new ZipFile(Encoding.UTF8))
                {
                   
                    zip.AddFile(filePath, "");
                    
                    zip.Save(@zipPath);
                }
                return @zipPath;
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message + " Compress IO.File Error");
                return null;
            }

        }

        public static string extractFile(string zipPath)
        {
            try
            {
                
                string tempDirectory = System.IO.Path.Combine(MainWindow.savePath, string.Format("{0:dd-MM-yyyy HH_mm_ss}", DateTime.Now));

                Directory.CreateDirectory(tempDirectory);
                if (Path.GetExtension(zipPath) == ".jpg" || Path.GetExtension(zipPath) == ".JPG")
                {
                    File.Copy(zipPath, Path.Combine(tempDirectory, "dummy.zip"));
                    zipPath = Path.Combine(tempDirectory, "dummy.zip");
                }
                using (ZipFile zip = ZipFile.Read(zipPath))
                {
                   
                    foreach (ZipEntry entry in zip)
                    {
                        if(Path.GetExtension(entry.FileName) == ".txt" || Path.GetExtension(entry.FileName) == ".TXT")
                        {
                            entry.Extract(tempDirectory, ExtractExistingFileAction.OverwriteSilently);
                            return Path.Combine(tempDirectory, entry.FileName);
                        }
                            
                    }
                }
                return string.Empty;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message + " Compress IO.File Error");
                return string.Empty;
            }
            
        }

        public static string compressFile(string dirPath, List<FileIO> fileList)
        {
            string zipPath = Path.Combine(dirPath, "dummy.zip");

            try
            {
                using (ZipFile zip = new ZipFile(Encoding.UTF8))
                {
                    foreach(FileIO file in fileList)
                    {
                        zip.AddFile(file.path, "");
                    }
                    zip.Save(@zipPath);
                }
                return @zipPath;
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message + " Compress IO.File Error");
                return null;
            }

        }

        public static bool createFile(string saveFile, string contentToWrite)
        {
            try
            {
                using (System.IO.FileStream fs = System.IO.File.Create(saveFile))
                {
                    for (byte i = 0; i < 100; i++)
                    {
                        fs.WriteByte(i);
                    }
                }

                System.IO.File.WriteAllText(saveFile, contentToWrite);
                return true;
            }

            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message +" Create IO.File Error");
                return false;
            }
        }

        public static bool writeToFile(string saveFile, string contentToWrite)
        {
            try
            {
                using (StreamWriter w = File.AppendText(saveFile))
                {
                    w.WriteLine(contentToWrite);
                }
                return true;
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message + " Write to IO.File Error");
                return false;
            }
        }


        public static string hashGenerator(string filePath)
        {
            if (Path.HasExtension(filePath))
            {
                try
                {
                    using (var md5 = System.Security.Cryptography.MD5.Create())
                    {
                        using (var stream = System.IO.File.OpenRead(filePath))
                        {
                            return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", null).ToLower();
                            // "" is the 8203 ascii character and the total lenght of the string doesnt change 
                            //return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                        }
                    }
                }
                catch (Exception exc)
                {
                    System.Diagnostics.Debug.WriteLine(exc.Message + " Hash Generator Error");
                    return null;
                }

            }
            else
            {
                return null;
            }

        }

        public static string encodeMix(string data, string str1, string str2)
        {
            try
            {
                data = data.Insert(data.Length - 2, str1.Substring(0, 5));
                data = Base64Encode(data);
                data = data.Insert(2, str2.Substring(4, 10));
                return Base64Encode(data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string decodeMix(string data, string str1, string str2)
        {
            try
            {
                string tempData = data;
                tempData = Base64Decode(tempData);
                tempData = tempData.Remove(2, str2.Substring(4, 10).Count());
                tempData = Base64Decode(tempData);
                tempData = tempData.Remove(tempData.Length - str1.Substring(0, 5).Count() - 2, str1.Substring(0, 5).Count());
                return tempData;
            }
            catch (Exception)
            {
                MessageBox.Show("Decoding error!","Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return data;
            }
        }

        public static string readTextFromFile(string filePath)
        {
            try
            {  
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadToEnd();
                    return line;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("The file could not be read:" + Environment.NewLine+
                    e.Message);
                return string.Empty;
            }
        }

        public static string Base64Encode(string plainText)
        {
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return System.Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string Base64Decode(string base64EncodedData)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string hashFromString(string text)
        {
            try
            {

                // byte array representation of that string
                byte[] encodedPassword = new UTF8Encoding().GetBytes(text);

                // need MD5 to calculate the hash
                byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

                // string representation (similar to UNIX format)
                string encoded = BitConverter.ToString(hash)
                   // without dashes
                   .Replace("-", string.Empty)
                   // make lowercase
                   .ToLower();
                return encoded;
            }
            catch (Exception)
            {

                throw;
            }
        }

        

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            try
            {
                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Defining type of data column gives proper data table 
                    var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name, type);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }

            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message + " Convert to DataTable Error");
                return null;
            }

            
        }
        
        public static  string getTimeStamp()
        {
            return DateTime.Now.ToString("yyyyMMdd_HHmmss", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }

        public static string ExecuteCMDCommands(List<string> strCommands)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;

            Process process = Process.Start(processStartInfo);

            try
            {
                if (process != null)
                {
                    foreach (string command in strCommands)
                    {
                        process.StandardInput.WriteLine(command);
                    }
                    process.StandardInput.WriteLine("exit");

                    process.StandardInput.Close(); // line added to stop process from hanging on ReadToEnd()

                    string outputString = process.StandardOutput.ReadToEnd();
                    return outputString;
                }
                return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }


    
}
