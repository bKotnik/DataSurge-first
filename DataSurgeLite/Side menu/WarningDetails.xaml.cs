using DataSurgeLite.Classes;
using System.Windows;

namespace DataSurgeLite.Side_menu
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

            //warningDetails.Text = Utility.PMClass[Utility.indexOfSelectedItem].Warning;

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
