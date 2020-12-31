using DataSurge.Source.Classes.Pages;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DataSurge.Source.User_controls.Pages
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : UserControl, ISwitchable
    {
        public Help()
        {
            InitializeComponent();

            //HelpTxtBox.Text = ""
        }

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            //this.Visibility = Visibility.Hidden;
        }
    }
}
