using System;
using System.Linq;
using System.Windows;

namespace DataSurge.Side_menu
{
    /// <summary>
    /// Interaction logic for PasswordGenerator.xaml
    /// </summary>
    public partial class PasswordGenerator : Window
    {
        string password_length;

        public PasswordGenerator()
        {
            InitializeComponent();
        }

        private void generate(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            password_length = pass_length.Text;
            int num;

            if (int.TryParse(password_length, out num) && num >= 6)
            {
                const string chars = "abcdefghijklmnopqrstuvwxyABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_/@.!?";

                string gen_password = new string(Enumerable.Repeat(chars, num).Select(s => s[random.Next(s.Length)]).ToArray());

                GeneratedPassword.Text = gen_password;

                GeneratedPassword.Visibility = Visibility.Visible;

                error_Passlen2.Visibility = Visibility.Hidden;
                error_PassLen.Visibility = Visibility.Hidden;
            }

            else if (int.TryParse(password_length, out num) && num < 6) // if it is a number, but the length is invalid
            {
                error_Passlen2.Visibility = Visibility.Visible;
                error_PassLen.Visibility = Visibility.Hidden;
            }

            else if (!int.TryParse(password_length, out num)) // if it's not a number
            {
                error_PassLen.Visibility = Visibility.Visible;
                error_Passlen2.Visibility = Visibility.Hidden;
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
