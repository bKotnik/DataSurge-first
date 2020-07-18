using System;
using System.Collections.Generic;
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

namespace DataSurge.Login
{
    /// <summary>
    /// Interaction logic for HelpContact.xaml
    /// </summary>
    public partial class HelpContact : Window
    {
        public HelpContact()
        {
            InitializeComponent();
            cardIcon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\logo_icon.ico", UriKind.Absolute));
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
