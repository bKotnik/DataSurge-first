using DataSurgeLite.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace DataSurgeLite.Side_menu
{
    /// <summary>
    /// Interaction logic for AddNew.xaml
    /// </summary>
    public partial class AddNew : Window
    {
        public AddNew()
        {
            InitializeComponent();
            showPassIcon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\show_password_icon.ico", UriKind.Absolute));
        }

        private void function()
        {
            //get data from AddNew form
            string email = "";
            string password = "";
            string username = "";
            string other = "";
            string note = "";

            if (enterEmail.Text == "")
                email = "/";
            else
                email = enterEmail.Text;
            ////////////////////////////////////////////
            if (enterPass.Password == "")
                password = "/";
            else
                password = enterPass.Password;
            ////////////////////////////////////////////
            if (enterUsername.Text == "")
                username = "/";
            else
                username = enterUsername.Text;
            ////////////////////////////////////////////
            if (enterOther.Text == "")
                other = "/";
            else
                other = enterOther.Text;
            ////////////////////////////////////////////
            if (EnterNote.Text == "")
                note = "/";
            else
                note = EnterNote.Text;

            if (email == "/" && username == "/" && password == "/" && other == "/" && note == "/")
            {
                this.Close();
            }

            else
            {
                DataClass data = new DataClass(email, password, username, other, note);
                Utility.ListData.Add(data);

                File.WriteAllText(Environment.CurrentDirectory + "\\Data.xml", string.Empty);

                //File.WriteAllText(Environment.CurrentDirectory + "\\data.txt", string.Empty);

                //serialization of ListData
                Stream stream = File.OpenWrite(Environment.CurrentDirectory + "\\Data.xml");

                try
                {
                    using (stream)
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));
                        xs.Serialize(stream, Utility.ListData);
                    }
                }

                catch
                {
                    _ = MessageBox.Show("Error occurred when trying to serialize data", "Error serializing", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            this.Close();
        }

        private void AddEntry(object sender, RoutedEventArgs e)
        {
            function();
        }

        private void EnterEntry(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                function();
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void showPass(object sender, RoutedEventArgs e)
        {
            enterPassTxtBox.Text = enterPass.Password;

            if (enterPass.Visibility == Visibility.Visible)//if password is masked
            {
                enterPass.Visibility = Visibility.Hidden;
                enterPassTxtBox.Visibility = Visibility.Visible;
            }

            else
            {
                enterPass.Visibility = Visibility.Visible;
                enterPassTxtBox.Visibility = Visibility.Hidden;
            }
        }
    }
}
