using DataSurge.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
            //string decrypted = Utility.Decrypt(Utility.ListData[Utility.indexOfSelectedItem].NoteDetails);
            //noteEditBox.Text = decrypted;
            //Utility.Encrypt(Utility.ListData[Utility.indexOfSelectedItem].NoteDetails);
            noteEditBox.Text = Utility.ListData[Utility.indexOfSelectedItem].NoteDetails;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Done(object sender, RoutedEventArgs e)
        {
            helperFunction();
        }

        private void enterDone(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                helperFunction();
            }
        }
        private void helperFunction()
        {
            // update text
            if (noteEditBox.Text != "")
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
