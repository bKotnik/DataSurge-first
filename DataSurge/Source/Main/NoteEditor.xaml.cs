﻿using DataSurge.Classes;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace DataSurge.Main
{
    /// <summary>
    /// Interaction logic for NoteEditor.xaml
    /// </summary>
    public partial class NoteEditor : Window
    {
        public NoteEditor()
        {
            InitializeComponent();

            header.Text += Utility.ListData[Utility.indexOfSelectedItem].Note;
            noteEditBox.Text = Utility.ListData[Utility.indexOfSelectedItem].NoteDetails;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Done(object sender, RoutedEventArgs e)
        {
            if (Utility.preferences.MaximumSecurity == true)
            {
                Helper.buttonContent = "confirmEdit";
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

        private void enterDone(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (Utility.preferences.MaximumSecurity == true)
                {
                    Helper.buttonContent = "confirmEdit";
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
            // update text
            Utility.ListData[Utility.indexOfSelectedItem].NoteDetails = noteEditBox.Text;

            // write the changes to file (overwrite)
            File.WriteAllText(Environment.CurrentDirectory + "\\Data.xml", string.Empty);

            Stream stream = File.OpenWrite(Environment.CurrentDirectory + "\\Data.xml");
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));

            try
            {
                using (stream)
                {
                    xs.Serialize(stream, Utility.ListData);
                }
            }

            catch
            {
                _ = MessageBox.Show("Error occurred when trying to serialize data", "Error serializing", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Close();
        }
    }
}
