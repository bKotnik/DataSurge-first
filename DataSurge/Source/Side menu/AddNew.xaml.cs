using DataSurge.Classes;
using DataSurge.Classes.Utils;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace DataSurge.Side_menu
{
    /// <summary>
    /// Interaction logic for AddNew.xaml
    /// </summary>
    public partial class AddNew : Window
    {
        private readonly MainWindow _mainWindow;

        public string PasswordText
        {
            get; set;
        }

        public AddNew(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            showPassIcon.Source = new BitmapImage(Assets.SHOW_PASS_ICON);
        }

        private void function()
        {
            //get data from AddNew form
            string email;
            string password;
            string username;
            string other;
            string note;

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
                Close();
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

                        // show ! in toolbar
                        _mainWindow.SetToolbarWarningVisibility(Visibility.Visible);
                        Properties.Settings.Default.ToolbarWarning = true;
                        Properties.Settings.Default.Save();
                    }
                }

                catch
                {
                    _ = MessageBox.Show("Error occurred when trying to serialize data", "Error serializing", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            Close();
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
            Console.WriteLine("enterpasstxtbox: " + enterPassTxtBox.Text);
            Console.WriteLine("password box: " + enterPass.Password);

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
