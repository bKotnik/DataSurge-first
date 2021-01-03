using DataSurge.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace DataSurge.Source.Nuggets
{
    /// <summary>
    /// Interaction logic for ChangeKey.xaml
    /// </summary>
    public partial class ChangeKey : Window
    {
        public ChangeKey()
        {
            InitializeComponent();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            WhichButton.buttonContent = "confirmChangeKey";

            Confirmation confirmation = new Confirmation();
            confirmation.ShowDialog();

            if (Utility.confirm == true)
            {
                // get user credentials
                XmlSerializer xs = new XmlSerializer(typeof(Registration));

                if (File.Exists(Environment.CurrentDirectory + "\\Users.xml"))
                {
                    FileStream fsout = new FileStream(Environment.CurrentDirectory + "\\Users.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                    try
                    {
                        using (fsout)
                        {
                            Utility.User = (Registration)xs.Deserialize(fsout);

                            //Decrypt
                            Utility.User.password = Utility.Decrypt(Utility.User.password);
                            Utility.User.email = Utility.Decrypt(Utility.User.email);
                        }
                    }
                    catch
                    {
                    }
                }

                // change encryption key
                string key = ChangeKeyTxtBox.Text.ToString();
                Properties.Settings.Default.EncryptionKey = key;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();

                // encrypt user with new key
                File.WriteAllText(Environment.CurrentDirectory + "\\Users.xml", string.Empty);
                using(FileStream fsout = new FileStream(Environment.CurrentDirectory + "\\Users.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    // encrypt
                    Utility.User.password = Utility.Encrypt(Utility.User.password);
                    Utility.User.email = Utility.Encrypt(Utility.User.email);
                    xs.Serialize(fsout, Utility.User);
                }
            }

            Utility.confirm = false;
            Close();
        }
    }
}
