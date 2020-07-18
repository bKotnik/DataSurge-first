using DataSurgeLite.Classes;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace DataSurgeLite.Main
{
    /// <summary>
    /// Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        public Edit()
        {
            InitializeComponent();

            EmailForm.Text = Utility.ListData[Utility.indexOfSelectedItem].Email;
            UsernameForm.Text = Utility.ListData[Utility.indexOfSelectedItem].Username;
            PassForm.Text = Utility.ListData[Utility.indexOfSelectedItem].Password;
            OtherForm.Text = Utility.ListData[Utility.indexOfSelectedItem].Other;
            NoteForm.Text = Utility.ListData[Utility.indexOfSelectedItem].Note;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            if (Utility.preferences.MaximumSecurity == true)
            {
                WhichButton.buttonContent = "confirmEdit";
                Confirmation confirmation = new Confirmation();
                confirmation.ShowDialog();

                if (Utility.confirm == true)
                {
                    helperFunction();
                }

                Utility.confirm = false;
            }

            else
                helperFunction();
        }

        private void enterSaveChanges(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Utility.preferences.MaximumSecurity == true)
                {
                    WhichButton.buttonContent = "confirmEdit";
                    Confirmation confirmation = new Confirmation();
                    confirmation.ShowDialog();

                    if (Utility.confirm == true)
                    {
                        helperFunction();
                    }

                    Utility.confirm = false;
                }

                else
                    helperFunction();
            }
        }

        private void helperFunction()
        {
            // change values of elements of selected item with values from form
            if (EmailForm.Text != "")
                Utility.ListData[Utility.indexOfSelectedItem].Email = EmailForm.Text;

            if (UsernameForm.Text != "")
                Utility.ListData[Utility.indexOfSelectedItem].Username = UsernameForm.Text;

            if (PassForm.Text != "")
                Utility.ListData[Utility.indexOfSelectedItem].Password = PassForm.Text;

            if (OtherForm.Text != "")
                Utility.ListData[Utility.indexOfSelectedItem].Other = OtherForm.Text;

            if (NoteForm.Text != "")
                Utility.ListData[Utility.indexOfSelectedItem].Note = NoteForm.Text;

            // write the changes to file (overwrite)
            File.WriteAllText(Environment.CurrentDirectory + "\\Data.xml", string.Empty);

            Stream stream = File.OpenWrite(Environment.CurrentDirectory + "\\Data.xml");
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));

            try
            {
                using (stream)
                {
                    xs.Serialize(stream, Utility.ListData);
                    Status.Visibility = Visibility.Visible;
                }
            }

            catch
            {
                _ = MessageBox.Show("Error occurred when trying to serialize data", "Error serializing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
