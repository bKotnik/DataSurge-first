using DataSurgeLite.Classes;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Xml.Serialization;

namespace DataSurgeLite.Side_menu
{
    /// <summary>
    /// Interaction logic for ChangeMasterPassword.xaml
    /// </summary>
    public partial class ChangeMasterPassword : Window
    {
        public ChangeMasterPassword()
        {
            InitializeComponent();
        }

        private void SaveCMP(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();

            string password;

            XmlSerializer xs = new XmlSerializer(typeof(Registration));

            FileStream fsin = new FileStream("Users.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            try
            {
                using (fsin)
                {
                    registration = (Registration)xs.Deserialize(fsin);
                }
            }

            catch
            {
                statusSaving.Visibility = Visibility.Visible;
            }

            password = Utility.Decrypt(registration.password);

            //error checking
            if (password == enterCurrentMP.Password && enterNewMP.Password == repeatNewMP.Password)
            {
                Incorrect_pass.Visibility = Visibility.Hidden;
                repeat_error.Visibility = Visibility.Hidden;
                // change password
                Utility.ListUsers.password = enterNewMP.Password;
                //Utility.ListUsers.email = registration.email;

                //Encrypt
                Utility.ListUsers.password = Utility.Encrypt(Utility.ListUsers.password);
                //Utility.ListUsers.email = Utility.Encrypt(Utility.ListUsers.email);

                XmlSerializer xs2 = new XmlSerializer(typeof(Registration));

                FileStream fsout = new FileStream("Users.xml", FileMode.Create, FileAccess.Write, FileShare.None);

                try
                {
                    using (fsout)
                    {
                        xs2.Serialize(fsout, Utility.ListUsers);

                    }
                }
                catch
                {
                    _ = MessageBox.Show("Error occurred when trying to serialize data", "Error serializing", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                var bc = new BrushConverter();
                statusSaving.Visibility = Visibility.Visible;
                statusSaving.Text = "Password changed successfully!";
                statusSaving.Foreground = (Brush)bc.ConvertFrom("#4db5a7");
            }

            else if (password == enterCurrentMP.Password && enterNewMP.Password != repeatNewMP.Password)
            {
                Incorrect_pass.Visibility = Visibility.Hidden;
                repeat_error.Visibility = Visibility.Visible;
            }

            else if (password != enterCurrentMP.Password && enterNewMP.Password == repeatNewMP.Password)
            {
                repeat_error.Visibility = Visibility.Hidden;
                Incorrect_pass.Visibility = Visibility.Visible;
            }

            else if (password != enterCurrentMP.Password && enterNewMP.Password != repeatNewMP.Password)
            {
                repeat_error.Visibility = Visibility.Visible;
                Incorrect_pass.Visibility = Visibility.Visible;
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
