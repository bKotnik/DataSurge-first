using DataSurge.Classes;
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

            if (password == Utility.ListUsers.password)
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
