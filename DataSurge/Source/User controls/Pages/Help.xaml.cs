using DataSurge.Source.Classes.Pages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
