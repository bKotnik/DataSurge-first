using DataSurge.Classes;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace DataSurge.Side_menu
{
    /// <summary>
    /// Interaction logic for PasswordMagician.xaml
    /// </summary>
    public partial class PasswordMagician : Window
    {
        bool isPressed = false;

        public PasswordMagician()
        {
            InitializeComponent();
            Utility.PMClass = new ObservableCollection<PasswordMagicianClass>();
        }

        private async void AnalyzeAsync(object sender, RoutedEventArgs e)
        {
            // show loading
            LoadingGrid.Visibility = Visibility.Visible;
            SetOpacity(0.5);

            int i = 1;
            int k = 0; // pomozni za dobit podvojena gesla
            string warning = "";

            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));
            Stream stream = File.OpenRead(Environment.CurrentDirectory + "\\Data.xml");

            //warnings
            string tooShort = "Password is too short";
            string repetitive = "Password is used for more than 1 account";
            string noNumbers = "Password should contain numbers";
            string noCaps = "Password does not contain a single capital letter";

            if (isPressed == false)
            {
                try
                {
                    using (stream)
                    {
                        Utility.ListData = (ObservableCollection<DataClass>)xs.Deserialize(stream);

                        //decrypt if it's encrypted
                        try
                        {
                            await RunDecryptionAsync();
                        }
                        catch { }

                        foreach (DataClass data in Utility.ListData)
                        {
                            bool isRepeated = false;
                            bool containsInt = data.Password.Any(char.IsDigit); // if it cointains numbers (true/false)
                            bool containsUpper = data.Password.Any(char.IsUpper);
                            bool oneOfThem = false;

                            for (int j = 0; j < Utility.ListData.Count() - 1; j++)
                            {
                                if (j != k)
                                {
                                    if (Utility.ListData[j].Password == Utility.ListData[k].Password)
                                    {
                                        isRepeated = true;
                                    }
                                }
                            }

                            if (isRepeated == true && data.Password != "/")
                            {
                                warning = repetitive + " | ";
                                oneOfThem = true;
                            }

                            if (data.Password.Length <= 6 && data.Password != "/")
                            {
                                warning += tooShort + " | ";
                                oneOfThem = true;
                            }

                            if (containsInt == false && data.Password != "/")
                            {
                                warning += noNumbers + " | ";
                                oneOfThem = true;
                            }

                            if (containsUpper == false && data.Password != "/")
                            {
                                warning += noCaps;
                                oneOfThem = true;
                            }

                            if (oneOfThem == true)
                            {
                                PasswordMagicianClass pmc = new PasswordMagicianClass();
                                pmc.Number = i;
                                pmc.Password = data.Password;
                                pmc.Warning = warning;
                                Utility.PMClass.Add(pmc);
                            }

                            // reset values
                            i++;
                            k++;
                            isPressed = false;
                            oneOfThem = false;
                            warning = "";
                        }
                    }
                }

                catch
                {
                    MessageBoxResult error_loading_data = MessageBox.Show("There are no passwords to analyze!", "Error loading data", MessageBoxButton.OK);
                }
            }

            lvListPM.ItemsSource = Utility.PMClass;
            isPressed = true;
            LoadingGrid.Visibility = Visibility.Collapsed;
            SetOpacity(1);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenWarningDetails(object sender, RoutedEventArgs e)
        {
            if (lvListPM.SelectedItems.Count <= 0)
            {
                _ = MessageBox.Show("Item not selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (lvListPM.SelectedItems.Count > 1)
            {
                _ = MessageBox.Show("Multiple items selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                Utility.indexOfSelectedItem = Utility.PMClass.IndexOf((PasswordMagicianClass)lvListPM.SelectedItem); // get index of selected item

                WarningDetails warningDetails = new WarningDetails();
                warningDetails.Show();
            }
        }

        private async Task RunDecryptionAsync()
        {
            ObservableCollection<DataClass> tmp = new ObservableCollection<DataClass>();

            foreach (DataClass data in Utility.ListData)
            {
                tmp.Add(await Task.Run(() => Utility.DecryptDataClassAsync(data)));
            }

            Utility.ListData = tmp;
        }

        private void SetOpacity(double opacity)
        {
            lvListPM.Opacity = opacity;
            btnExit.Opacity = opacity;
            btnAnalyze.Opacity = opacity;
        }
    }
}
