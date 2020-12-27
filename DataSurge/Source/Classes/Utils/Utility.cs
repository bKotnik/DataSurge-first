using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace DataSurge.Classes
{
    [Serializable]
    static class Utility
    {
        public static ObservableCollection<DataClass> ListData;
        public static Registration User;
        public static PreferencesClass preferences;
        public static RememberPassword rmbPassword;
        public static ObservableCollection<ListDataClass> LDClass; // collection for list data (listview buttons) -> eg. display all emails
        public static ObservableCollection<PasswordMagicianClass> PMClass; // collection for password magician

        public static bool textIfFileEmpty = false;

        //for edit entry
        public static int indexOfSelectedItem;
        public static bool confirm = false; //confirmation with password for edit/delete entry

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = Properties.Settings.Default.EncryptionKey; // it was abc123
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }

                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = Properties.Settings.Default.EncryptionKey;
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }

                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }

        public static ObservableCollection<DataClass> decryptListData(ObservableCollection<DataClass> data)
        {
            foreach (DataClass tmp in data)
            {
                tmp.Email = Decrypt(tmp.Email);
                tmp.Password = Decrypt(tmp.Password);
                tmp.Username = Decrypt(tmp.Username);
                tmp.Other = Decrypt(tmp.Other);
                tmp.Note = Decrypt(tmp.Note);
                tmp.NoteDetails = Decrypt(tmp.NoteDetails);
            }

            return data;
        }
        public static ObservableCollection<DataClass> encryptListData(ObservableCollection<DataClass> data)
        {
            foreach (DataClass tmp in data)
            {
                tmp.Email = Encrypt(tmp.Email);
                tmp.Password = Encrypt(tmp.Password);
                tmp.Username = Encrypt(tmp.Username);
                tmp.Other = Encrypt(tmp.Other);
                tmp.Note = Encrypt(tmp.Note);
                tmp.NoteDetails = Encrypt(tmp.NoteDetails);
            }

            return data;
        }

        public static void OnLoad()
        {
            var bc = new BrushConverter();
            var cc = new ColorConverter();

            /*main color*/
            Application.Current.Resources["MainColor"] = (Brush)bc.ConvertFromString(Properties.Settings.Default.MainColor);
            Application.Current.Resources["MainColor50"] = (Brush)bc.ConvertFromString(Properties.Settings.Default.MainColor50);
            Application.Current.Resources["MainGradientColor"] = (Color)cc.ConvertFrom(Properties.Settings.Default.MainColor);
            Application.Current.Resources["MainColor35"] = (Color)cc.ConvertFrom(Properties.Settings.Default.MainColor35);
            Application.Current.Resources["MainColor95"] = (Color)cc.ConvertFrom(Properties.Settings.Default.MainColor95);
            /*main color*/

            /*secondary color*/
            Application.Current.Resources["SecondaryColor"] = (Brush)bc.ConvertFromString(Properties.Settings.Default.SecondaryColor);
            Application.Current.Resources["SecondaryGradientColor"] = (Color)cc.ConvertFrom(Properties.Settings.Default.SecondaryColor);
            /*secondary color*/

            /*exit/cancel color*/
            Application.Current.Resources["ExitColor"] = (Brush)bc.ConvertFromString(Properties.Settings.Default.ExitColor);
            /*exit/cancel color*/

            /*title color*/
            Application.Current.Resources["TitleColor"] = (Brush)bc.ConvertFromString(Properties.Settings.Default.TitleColor);
            Application.Current.Resources["TitleGradientColor"] = (Color)cc.ConvertFrom(Properties.Settings.Default.TitleColor);
            /*title color*/

            /*text color*/
            Application.Current.Resources["TextColor"] = (Brush)bc.ConvertFromString(Properties.Settings.Default.TextColor);
            Application.Current.Resources["TextGradientColor"] = (Color)cc.ConvertFrom(Properties.Settings.Default.TextColor);
            /*text color*/

            /*background color*/
            Application.Current.Resources["BackgroundColor"] = (Brush)bc.ConvertFromString(Properties.Settings.Default.BackgroundColor);
            Application.Current.Resources["BackgroundGradientColor"] = (Color)cc.ConvertFrom(Properties.Settings.Default.BackgroundColor);
            /*background color*/

            /*warning color*/
            Application.Current.Resources["WarningColor"] = (Brush)bc.ConvertFromString(Properties.Settings.Default.WarningColor);
            /*warning color*/
        } // load colors
    }
}
