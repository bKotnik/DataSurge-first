using DataSurgeLite.Classes;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace DataSurgeLite.Toolbar
{
    /// <summary>
    /// Interaction logic for FindReplace.xaml
    /// </summary>
    public partial class FindReplace : Window
    {
        public FindReplace()
        {
            InitializeComponent();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Replace(object sender, RoutedEventArgs e)
        {
            nothingFound.Text = "*Nothing found in: ";
            string findW = findWhat.Text;
            string replaceW = replaceWith.Text;

            bool emails = chkBoxEmails.IsChecked.Value;
            bool passwords = chkBoxPass.IsChecked.Value;
            bool usernames = chkBoxUsername.IsChecked.Value;
            bool other = chkBoxOther.IsChecked.Value;
            bool notes = chkBoxNotes.IsChecked.Value;

            bool found = false; //if found anything

            if (emails == true || passwords == true || usernames == true || other == true || notes == true)
            {
                nothingSelected.Visibility = Visibility.Hidden;

                /*REPLACE in listdata*/
                if (emails == true) // if it's checked, go through emails and replace
                {
                    foreach (DataClass tmp in Utility.ListData)
                    {
                        if (tmp.Email == findW)
                        {
                            tmp.Email = replaceW;
                            found = true;
                        }
                    }

                    if (found == false)
                        nothingFound.Text += "Emails, ";

                    found = false;
                }
                if (passwords == true)
                {
                    foreach (DataClass tmp in Utility.ListData)
                    {
                        if (tmp.Password == findW)
                        {
                            tmp.Password = replaceW;
                            found = true;
                        }
                    }

                    if (found == false)
                        nothingFound.Text += "Passwords\n";

                    found = false;
                }
                if (usernames == true)
                {
                    foreach (DataClass tmp in Utility.ListData)
                    {
                        if (tmp.Username == findW)
                        {
                            tmp.Username = replaceW;
                            found = true;
                        }
                    }

                    if (found == false)
                        nothingFound.Text += "Usernames, ";

                    found = false;
                }
                if (other == true)
                {
                    foreach (DataClass tmp in Utility.ListData)
                    {
                        if (tmp.Other == findW)
                        {
                            tmp.Other = replaceW;
                            found = true;
                        }
                    }

                    if (found == false)
                        nothingFound.Text += "Other, ";

                    found = false;
                }
                if (notes == true)
                {
                    foreach (DataClass tmp in Utility.ListData)
                    {
                        if (tmp.Note == findW)
                        {
                            tmp.Note = replaceW;
                            found = true;
                        }
                    }

                    if (found == false)
                        nothingFound.Text += "Notes";
                }

                /*REPLACE IN FILE*/
                File.WriteAllText(Environment.CurrentDirectory + "\\Data.xml", string.Empty);

                Stream stream = File.OpenWrite(Environment.CurrentDirectory + "\\Data.xml");
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));

                try
                {
                    using (stream)
                    {
                        xs.Serialize(stream, Utility.ListData);
                        status.Visibility = Visibility.Visible;
                    }
                }

                catch
                {
                    status.Text = "Error replacing!";
                    status.Visibility = Visibility.Visible;
                }
            }

            // if none of the checkboxes are selected
            else if (emails == false && passwords == false && usernames == false && other == false && notes == false)
            {
                nothingSelected.Visibility = Visibility.Visible;
            }

            if (nothingFound.Text.Length > 19) // 19 is length of *nothing found in:,
            {
                nothingFound.Visibility = Visibility.Visible;
                status.Text = "";
            }
        }
    }
}
