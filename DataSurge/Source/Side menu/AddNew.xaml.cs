using DataSurge.Classes;
using DataSurge.Classes.Utils;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
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

        private async Task function()
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

                //serialization of ListData
                Stream stream = File.OpenWrite(Environment.CurrentDirectory + "\\Data.xml");

                try
                {
                    using (stream)
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));

                        if (Properties.Settings.Default.ToolbarWarning == false) // encryption needed
                        {
                            LoadingState("Encrypting");

                            await RunEncryptionAsync();
                            xs.Serialize(stream, Utility.ListData);

                            // decrypt - otherwise encryption stacks
                            await RunDecryptionAsync();

                            StateGrid.Visibility = Visibility.Collapsed;
                            GetData();
                        }

                        else
                            xs.Serialize(stream, Utility.ListData);
                    }
                }

                catch
                {
                    _ = MessageBox.Show("Error occurred when trying to serialize data", "Error serializing", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            Close();
        }

        private async void AddEntry(object sender, RoutedEventArgs e)
        {
            await function();
        }

        private async void EnterEntry(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await function();
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

        // ENCRYPTION & DECRYPTION - ASYNC
        private async Task RunDecryptionAsync()
        {
            ObservableCollection<DataClass> tmp = new ObservableCollection<DataClass>();

            foreach (DataClass data in Utility.ListData)
            {
                tmp.Add(await Task.Run(() => Utility.DecryptDataClassAsync(data)));
            }

            Utility.ListData = tmp;
        }
        private async Task RunEncryptionAsync()
        {
            ObservableCollection<DataClass> tmp = new ObservableCollection<DataClass>();

            foreach (DataClass data in Utility.ListData)
            {
                tmp.Add(await Task.Run(() => Utility.EncryptDataClassAsync(data)));
            }

            Utility.ListData = tmp;
        }
        /* LOADING INDICATOR */
        private void LoadingState(string state)
        {
            StateGrid.Visibility = Visibility.Visible;

            StateLbl.Content = state;
        }

        private void GetData() // loads edit/delete icons and assigns ItemsSource of listview
        {
            //assign icons to objects
            foreach (DataClass data in Utility.ListData)
            {
                data.EditPath = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\edit_icon.ico", UriKind.Absolute)).ToString();
                data.DeletePath = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\delete_icon_red.ico", UriKind.Absolute)).ToString();
            }
        }
    }
}
