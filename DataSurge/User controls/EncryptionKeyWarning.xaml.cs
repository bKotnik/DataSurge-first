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

namespace DataSurge.User_controls
{
    /// <summary>
    /// Interaction logic for EncryptionKeyWarning.xaml
    /// </summary>
    public partial class EncryptionKeyWarning : UserControl
    {
        public EncryptionKeyWarning()
        {
            InitializeComponent();
        }

        private void EncryptionKeyWarningOk(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }
    }
}
