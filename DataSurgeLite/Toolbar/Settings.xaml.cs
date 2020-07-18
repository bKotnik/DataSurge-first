using DataSurgeLite.Classes;
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

namespace DataSurgeLite.Toolbar
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();

            chkBoxSpaceQC.IsChecked = Utility.preferences.QuickCastSpace;
            chkBoxEscQC.IsChecked = Utility.preferences.QuickCastEsc;
            chkBoxMaximumSecurity.IsChecked = Utility.preferences.MaximumSecurity;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Utility.preferences.QuickCastSpace = chkBoxSpaceQC.IsChecked.Value;
            Utility.preferences.QuickCastEsc = chkBoxEscQC.IsChecked.Value;
            Utility.preferences.MaximumSecurity = chkBoxMaximumSecurity.IsChecked.Value;

            File.WriteAllText(Environment.CurrentDirectory + "\\Preferences.xml", string.Empty);
            XmlSerializer xs = new XmlSerializer(typeof(PreferencesClass));
            FileStream fsout = new FileStream("Preferences.xml", FileMode.Open, FileAccess.Write, FileShare.None);

            try
            {
                using (fsout)
                {
                    xs.Serialize(fsout, Utility.preferences);
                    status.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                _ = MessageBox.Show("Error occurred when trying to serialize data", "Error serializing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
