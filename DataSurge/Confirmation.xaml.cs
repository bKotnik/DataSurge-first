using DataSurge.Classes;
using System.Windows;
using System.Windows.Input;

namespace DataSurge
{
    /// <summary>
    /// Interaction logic for Confirmation.xaml
    /// </summary>
    public partial class Confirmation : Window
    {
        public Confirmation()
        {
            InitializeComponent();

            if (WhichButton.buttonContent == "confirmDelete")
                Title = "Confirm delete";

            else if (WhichButton.buttonContent == "confirmEdit")
                Title = "Confirm edit";

            else
                Title = "Confirm changes";
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            helperFunction();
        }

        private void enterConfirm(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                helperFunction();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void helperFunction()
        {
            string password = confirmPasswordBox.Password;

            if (password == Utility.User.password)
            {
                Utility.confirm = true;
                Close();
            }

            else
            {
                incorrectPass.Visibility = Visibility.Visible;
            }
        }
    }
}
