using System.Windows;
using System.Windows.Controls;

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
