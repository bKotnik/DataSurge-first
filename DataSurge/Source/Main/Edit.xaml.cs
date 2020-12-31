using DataSurge.Classes;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace DataSurge.Main
{
    /// <summary>
    /// Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        private readonly MainWindow _mainWindow;

        public Edit(MainWindow mainWindow)
        {
            InitializeComponent();

            _mainWindow = mainWindow;
            Init();
        }

        private void Init()
        {
            EmailForm.Text = Utility.ListData[Utility.indexOfSelectedItem].Email;
            UsernameForm.Text = Utility.ListData[Utility.indexOfSelectedItem].Username;
            PassForm.Text = Utility.ListData[Utility.indexOfSelectedItem].Password;
            OtherForm.Text = Utility.ListData[Utility.indexOfSelectedItem].Other;
            NoteForm.Text = Utility.ListData[Utility.indexOfSelectedItem].Note;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void SaveChanges(object sender, RoutedEventArgs e)
        {
            if (Utility.preferences.MaximumSecurity == true)
            {
                WhichButton.buttonContent = "confirmEdit";
                Confirmation confirmation = new Confirmation();
                confirmation.ShowDialog();

                if (Utility.confirm == true)
                {
                    await helperFunction();
                }

                Utility.confirm = false;
            }

            else
                await helperFunction();
        }

        private async void enterSaveChanges(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Utility.preferences.MaximumSecurity == true)
                {
                    WhichButton.buttonContent = "confirmEdit";
                    Confirmation confirmation = new Confirmation();
                    confirmation.ShowDialog();

                    if (Utility.confirm == true)
                    {
                        await helperFunction();
                    }

                    Utility.confirm = false;
                }

                else
                    await helperFunction();
            }
        }

        private async Task helperFunction()
        {
            // change values of elements of selected item with values from form
            if (EmailForm.Text != "")
                Utility.ListData[Utility.indexOfSelectedItem].Email = EmailForm.Text;

            if (UsernameForm.Text != "")
                Utility.ListData[Utility.indexOfSelectedItem].Username = UsernameForm.Text;

            if (PassForm.Text != "")
                Utility.ListData[Utility.indexOfSelectedItem].Password = PassForm.Text;

            if (OtherForm.Text != "")
                Utility.ListData[Utility.indexOfSelectedItem].Other = OtherForm.Text;

            if (NoteForm.Text != "")
                Utility.ListData[Utility.indexOfSelectedItem].Note = NoteForm.Text;

            //check if user deleted / character
            if (EmailForm.Text == "")
                Utility.ListData[Utility.indexOfSelectedItem].Email = "/";

            if (UsernameForm.Text == "")
                Utility.ListData[Utility.indexOfSelectedItem].Username = "/";

            if (PassForm.Text == "")
                Utility.ListData[Utility.indexOfSelectedItem].Password = "/";

            if (OtherForm.Text == "")
                Utility.ListData[Utility.indexOfSelectedItem].Other = "/";

            if (NoteForm.Text == "")
                Utility.ListData[Utility.indexOfSelectedItem].Note = "/";

            // write the changes to file (overwrite)
            File.WriteAllText(Environment.CurrentDirectory + "\\Data.xml", string.Empty);

            Stream stream = File.OpenWrite(Environment.CurrentDirectory + "\\Data.xml");
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));

            try
            {
                using (stream)
                {
                    if (Properties.Settings.Default.ToolbarWarning == false) // encryption needed
                    {
                        LoadingState("Encrypting");

                        await RunEncryptionAsync();
                        xs.Serialize(stream, Utility.ListData);

                        // decrypt - otherwise encryption stacks
                        await RunDecryptionAsync();

                        StateGrid.Visibility = Visibility.Collapsed;
                        Status.Visibility = Visibility.Visible;
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

        private void OpenNoteDetails(object sender, RoutedEventArgs e)
        {
            NoteEditor noteEditor = new NoteEditor();
            noteEditor.Show();
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
