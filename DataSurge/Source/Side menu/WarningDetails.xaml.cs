using DataSurge.Classes;
using System.Windows;

namespace DataSurge.Side_menu
{
    /// <summary>
    /// Interaction logic for WarningDetails.xaml
    /// </summary>
    public partial class WarningDetails : Window
    {
        public WarningDetails()
        {
            InitializeComponent();

            int i = 1;
            string tmp = "";
            tmp += i + ".  ";

            foreach (char character in Utility.PMClass[Utility.indexOfSelectedItem].Warning)
            {
                if (!character.Equals('|'))
                    tmp += character;

                if (character.Equals('|'))
                {
                    tmp += "\n";
                    tmp += i + 1 + ". ";
                    i++;
                }
            }

            warningDetails.Text = tmp;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
