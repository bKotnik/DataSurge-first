using System.Windows;

namespace DataSurge.Main
{
    /// <summary>
    /// Interaction logic for ViewKey.xaml
    /// </summary>
    public partial class ViewKey : Window
    {
        public ViewKey()
        {
            InitializeComponent();

            viewKeyBox.Text = Properties.Settings.Default.EncryptionKey;
        }
    }
}
