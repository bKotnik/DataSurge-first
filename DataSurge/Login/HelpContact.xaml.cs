using System;
using System.Windows;
using System.Windows.Media.Imaging;

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
