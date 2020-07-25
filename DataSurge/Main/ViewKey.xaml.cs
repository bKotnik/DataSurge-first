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
