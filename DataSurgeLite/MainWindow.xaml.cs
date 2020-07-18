using DataSurgeLite.Classes;
using DataSurgeLite.Login;
using DataSurgeLite.Main;
using DataSurgeLite.Side_menu;
using DataSurgeLite.Toolbar;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using WPFCustomMessageBox;

namespace DataSurgeLite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool clickedAgainE = false;
        bool clickedAgainU = false;
        bool clickedAgainP = false;
        bool clickedAgainO = false;
        bool clickedAgainN = false;
        bool spaceAgain = false;

        // for drag and drop
        private int startIndex = -1;
        private Point startPoint = new Point();

        public MainWindow()
        {
            InitializeComponent();

            //quick casts
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.PreviewKeyDown += new KeyEventHandler(HandleSpace);

            // get source for images
            logout_icon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\logout.png", UriKind.Absolute));
            editOrodna_icon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\edit_icon_backup.png", UriKind.Absolute));
            new_icon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\new_icon.png", UriKind.Absolute));
            xOrodna_icon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\x_icon.png", UriKind.Absolute));
            fullScreenOrodna_icon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\fullScreen_icon.png", UriKind.Absolute));
            HelpOrodna_icon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\help_icon.png", UriKind.Absolute));
            language_icon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\language_icon.png", UriKind.Absolute));
            settings_icon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\settings_icon.png", UriKind.Absolute));
            findReplace_icon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\findReplace_icon.png", UriKind.Absolute));
            export_icon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\export_icon.ico", UriKind.Absolute));
            import_icon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\import_icon.ico", UriKind.Absolute));
            decrypt_icon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\decrypt_icon.ico", UriKind.Absolute));
            encrypt_icon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\encrypt_icon.ico", UriKind.Absolute));
            colorPicker_icon.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\color_picker_icon.ico", UriKind.Absolute));
            // 

            Utility.ListData = new ObservableCollection<DataClass>();
            Utility.preferences = new PreferencesClass();

            /*GET PREFERENCES FROM FILE*/
            XmlSerializer xsPref = new XmlSerializer(typeof(PreferencesClass));
            FileStream fsin = new FileStream("Preferences.xml", FileMode.Open, FileAccess.Read, FileShare.None);
            try
            {
                using (fsin)
                {
                    Utility.preferences = (PreferencesClass)xsPref.Deserialize(fsin);
                }
            }

            catch
            {
                if (fsin.Length != 0)
                    _ = MessageBox.Show("Error occurred when trying to load preferences", "Error loading preferences", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            /*GET DATA FROM FILE*/
            Stream stream = File.OpenRead(Environment.CurrentDirectory + "\\Data.xml");
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));
            try
            {
                Utility.ListData = (ObservableCollection<DataClass>)xs.Deserialize(stream);

                //assign icons to objects
                foreach (DataClass data in Utility.ListData)
                {
                    data.EditPath = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\edit_icon.ico", UriKind.Absolute)).ToString();
                    data.DeletePath = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\delete_icon_red.ico", UriKind.Absolute)).ToString();
                }

                lvDataMain.ItemsSource = Utility.ListData;
            }

            catch
            {
                if (stream.Length != 0)
                {
                    _ = MessageBox.Show("Error occurred when trying to load data", "Error loading data", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            stream.Close();
        }

        /* LISTVIEW BUTTONS */
        private void click_emails(object sender, RoutedEventArgs e)
        {
            WhichButton.buttonContent = (sender as Button).Content.ToString();
            WhichButton.listLabel = "Email";
            ListData emails = new ListData();

            emails.ShowDialog();
        }

        private void click_usernames(object sender, RoutedEventArgs e)
        {
            WhichButton.buttonContent = (sender as Button).Content.ToString();
            WhichButton.listLabel = "Username";
            ListData usernames = new ListData();

            usernames.ShowDialog();
        }

        private void click_passwords(object sender, RoutedEventArgs e)
        {
            WhichButton.buttonContent = (sender as Button).Content.ToString();
            WhichButton.listLabel = "Password";
            ListData passwords = new ListData();

            passwords.ShowDialog();
        }

        private void click_other(object sender, RoutedEventArgs e)
        {
            WhichButton.buttonContent = (sender as Button).Content.ToString();
            WhichButton.listLabel = "Other";
            ListData other = new ListData();

            other.ShowDialog();
        }

        private void click_note(object sender, RoutedEventArgs e)
        {
            WhichButton.buttonContent = (sender as Button).Content.ToString();
            WhichButton.listLabel = "Note";
            ListData notes = new ListData();

            notes.ShowDialog();
        }

        /* SIDE MENU BUTTONS */
        private void click_PG(object sender, RoutedEventArgs e)
        {
            PasswordGenerator passGen = new PasswordGenerator();
            passGen.Show();
        }

        private void btn_import(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            fileDialog.Filter = "Xml file (*.xml)|*.xml";

            // ti jih sam nalozi v listview pol pa uprasa ce hocs overwritat data.xml file
            //open file dialog
            if (fileDialog.ShowDialog() == true)
            {
                //if fileDialog is true and data is loaded successfully, it asks the user to overwrite file or not
                MessageBoxResult choice = CustomMessageBox.ShowYesNoCancel("Data has been successfully loaded into the app! \nDo you want to overwrite current data file with this file?",
                                         "Overwrite file", "Overwrite", "Just display loaded data", "Cancel", MessageBoxImage.Question);

                if (choice == MessageBoxResult.Yes)
                {
                    try
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));
                        Utility.ListData = (ObservableCollection<DataClass>)xs.Deserialize(fileDialog.OpenFile());
                    }

                    catch
                    {
                        _ = MessageBox.Show("Error occurred when trying to load data from file\n*Data may be encrypted/formatted incorrectly*",
                            "Error loading data", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    File.WriteAllText(Environment.CurrentDirectory + "\\Data.xml", string.Empty); // clear file

                    Stream stream = File.OpenWrite(Environment.CurrentDirectory + "\\Data.xml");

                    try
                    {
                        using (stream)
                        {
                            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));
                            xs.Serialize(stream, Utility.ListData);
                        }
                    }

                    catch
                    {
                        _ = MessageBox.Show("Error occurred when trying to serialize data", "Error serializing", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    lvDataMain.ItemsSource = Utility.ListData;
                }

                else if (choice == MessageBoxResult.No)
                {
                    try
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));
                        Utility.ListData = (ObservableCollection<DataClass>)xs.Deserialize(fileDialog.OpenFile());
                    }

                    catch
                    {
                        _ = MessageBox.Show("Error occurred when trying to load data from file\n*Data may be encrypted/formatted incorrectly*",
                            "Error loading data", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    lvDataMain.ItemsSource = Utility.ListData;
                }

                else if (choice == MessageBoxResult.Cancel) { }
            }

            else { }
        }

        private void btn_export(object sender, RoutedEventArgs e)
        {
            MessageBoxResult choice = CustomMessageBox.ShowYesNoCancel("How do you want to export data file?\n*Choosing formatted allows you to import the file later",
                "Export data file", "Formatted", "Plain", "Cancel", MessageBoxImage.Question);

            SaveFileDialog saveFile = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (choice == MessageBoxResult.Yes)
                saveFile.Filter = "Xml file (*.xml)|*.xml";

            else
                saveFile.Filter = "Text file (*.txt)|*.txt";

            if (choice == MessageBoxResult.Yes)
            {
                if (saveFile.ShowDialog() == true)
                {
                    //Encrypt
                    Utility.encryptListData(Utility.ListData);

                    try
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));
                        xs.Serialize(saveFile.OpenFile(), Utility.ListData);
                    }

                    catch
                    {
                        _ = MessageBox.Show("Error occurred when trying to save file", "Error saving", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //Decrypt
                    Utility.decryptListData(Utility.ListData);
                }

                else { }
            }

            else if (choice == MessageBoxResult.No)
            {
                if (saveFile.ShowDialog() == true)
                {
                    TextBlock label = new TextBlock();
                    label.Text = "Emails";
                    label.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                    label.FontSize = 16;

                    List<string> lines = new List<string>();
                    lines.Add(label.Text);

                    foreach (DataClass data in Utility.ListData)
                    {
                        lines.Add(data.Email);
                    }
                    label.Text = "Usernames";
                    lines.Add("\n");
                    lines.Add(label.Text);
                    foreach (DataClass data in Utility.ListData)
                    {
                        lines.Add(data.Username);
                    }
                    label.Text = "Passwords";
                    lines.Add("\n");
                    lines.Add(label.Text);
                    foreach (DataClass data in Utility.ListData)
                    {
                        lines.Add(data.Password);
                    }
                    label.Text = "Other";
                    lines.Add("\n");
                    lines.Add(label.Text);
                    foreach (DataClass data in Utility.ListData)
                    {
                        lines.Add(data.Other);
                    }
                    label.Text = "Notes";
                    lines.Add("\n");
                    lines.Add(label.Text);
                    foreach (DataClass data in Utility.ListData)
                    {
                        lines.Add(data.Note);
                    }

                    File.WriteAllLines(saveFile.FileName, lines);
                }

                else { }
            }

            else if (choice == MessageBoxResult.Cancel) { }
        }

        private void CngMasterPassword(object sender, RoutedEventArgs e)
        {
            ChangeMasterPassword cngMP = new ChangeMasterPassword();
            cngMP.ShowDialog();
        }

        private void PasswordMagician(object sender, RoutedEventArgs e)
        {
            PasswordMagician passMag = new PasswordMagician();
            passMag.ShowDialog();
        }

        private void addNew(object sender, RoutedEventArgs e)
        {
            AddNew add_new_window = new AddNew();

            add_new_window.ShowDialog();

            lvDataMain.ItemsSource = Utility.ListData;
        }
        private void Logout(object sender, RoutedEventArgs e)
        {
            CloseAllWindows();

            Authentication login = new Authentication();
            login.Show();
            Close();
        }

        /* EDIT COLUMN BUTTONS */
        private void DeleteItem(object sender, RoutedEventArgs e)
        {
            if (lvDataMain.SelectedItems.Count <= 0)
            {
                _ = MessageBox.Show("0 items(s) selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                if (Utility.preferences.MaximumSecurity == true)
                {
                    WhichButton.buttonContent = "confirmDelete";

                    Confirmation confirmation = new Confirmation();
                    confirmation.ShowDialog();

                    if (Utility.confirm == true)
                    {
                        HelperDeleteItem();
                    }

                    Utility.confirm = false;
                }

                else
                {
                    HelperDeleteItem();
                }
            }

        }

        private void HelperDeleteItem()
        {
            while (lvDataMain.SelectedItems.Count > 0)
            {
                Utility.ListData.Remove((DataClass)lvDataMain.SelectedItem);
            }

            //Utility.ListData.Remove((DataClass)lvDataMain.SelectedItem);
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
        }

        private void EditEntry(object sender, RoutedEventArgs e)
        {
            if (lvDataMain.SelectedItems.Count <= 0)
            {
                _ = MessageBox.Show("Item not selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (lvDataMain.SelectedItems.Count > 1)
            {
                _ = MessageBox.Show("Multiple items selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                Utility.indexOfSelectedItem = Utility.ListData.IndexOf((DataClass)lvDataMain.SelectedItem); // get index of selected item

                Edit editWindow = new Edit();
                editWindow.ShowDialog();
            }
        }

        private void OpenNoteEditor(object sender, RoutedEventArgs e)
        {
            if (lvDataMain.SelectedItems.Count <= 0)
            {
                _ = MessageBox.Show("Item not selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (lvDataMain.SelectedItems.Count > 1)
            {
                _ = MessageBox.Show("Multiple items selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                Utility.indexOfSelectedItem = Utility.ListData.IndexOf((DataClass)lvDataMain.SelectedItem); // get index of selected item

                NoteEditor noteEditor = new NoteEditor();
                noteEditor.ShowDialog();
            }
        }

        /* TOOLBAR MENU */
        /*FILE*/
        private void OpenAddNew(object sender, RoutedEventArgs e)
        {
            AddNew add_new_window = new AddNew();

            add_new_window.ShowDialog();
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            MessageBoxResult youSure = MessageBox.Show("Are you sure you want to exit application?", "exit application", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (youSure == MessageBoxResult.Yes)
                Close();
        }

        /*EDIT*/
        private void OpenEditEntry(object sender, RoutedEventArgs e) // orodna vrstica
        {
            if (lvDataMain.SelectedItems.Count <= 0)
            {
                _ = MessageBox.Show("Item not selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (lvDataMain.SelectedItems.Count > 1)
            {
                _ = MessageBox.Show("Multiple items selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                Utility.indexOfSelectedItem = Utility.ListData.IndexOf((DataClass)lvDataMain.SelectedItem); // get index of selected item

                Edit editWindow = new Edit();
                editWindow.ShowDialog();
            }
        }

        private void OpenFindReplace(object sender, RoutedEventArgs e)
        {
            FindReplace findReplace = new FindReplace();
            findReplace.Show();
        }

        /*VIEW*/
        private void FullScreen(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }

            else
            {
                WindowState = WindowState.Normal;
            }
        }

        private void TranslateSlo(object sender, RoutedEventArgs e)
        {
            // Side Menu
            btnPG.Content = "PASSWORD\nGENERATOR";
            btnImport.Content = "IMPORT";
            btnExport.Content = "EXPORT";
            btnCMP.Content = "CHANGE MASTER\n     PASSWORD";
            btnPM.Content = "PASSWORD\nMAGICIAN";
            btnAddnew.Content = "ADD NEW";

            /* Toolbar Menu */
            //File
            toolbarFile.Header = "_File";
            toolbarNew.Header = "New";
            toolbarImport.Header = "Import";
            toolbarExport.Header = "Export";
            toolbarExit.Header = "Exit";
            //Edit
            toolbarEdit.Header = "_Edit";
            toolbarEditEntry.Header = "Edit entry";
            //View
            toolbarView.Header = "_View";
            toolbarFullscr.Header = "Full Screen";
            toolbarLanguage.Header = "Language";
            //Help
            toolbarHelp.Header = "_Help";
            toolbarViewHelp.Header = "View Help";
            //Listview buttons
            btnEmails.Content = "EMAILS";
            btnUsernames.Content = "USERNAMES";
            btnPasswords.Content = "PASSWORDS";
            btnOther.Content = "OTHER";
            btnNotes.Content = "NOTES";
            editHeader.Content = "EDIT";

            slo.IsChecked = false;
        }

        private void TranslateEng(object sender, RoutedEventArgs e)
        {
            // Side Menu
            btnPG.Content = "GENERATOR\n      GESEL";
            btnImport.Content = "UVOZI";
            btnExport.Content = "IZVOZI";
            btnCMP.Content = "SPREMENI GLAVNO\n            GESLO";
            btnPM.Content = "ČAROVNIK ZA\n       GESLA";
            btnAddnew.Content = "DODAJ";

            /* Toolbar Menu */
            //File
            toolbarFile.Header = "_Datoteka";
            toolbarNew.Header = "Novo";
            toolbarImport.Header = "Uvozi";
            toolbarExport.Header = "Izvozi";
            toolbarExit.Header = "Izhod";
            //Edit
            toolbarEdit.Header = "Uredi";
            toolbarEditEntry.Header = "Uredi vnos";
            //View
            toolbarView.Header = "Pogled";
            toolbarFullscr.Header = "Celozaslonski način";
            toolbarLanguage.Header = "Jezik";
            //Help
            toolbarHelp.Header = "_Pomoč";
            toolbarViewHelp.Header = "Ogled Pomoči";
            //Listview buttons
            btnEmails.Content = "E-NASLOVI";
            btnUsernames.Content = "UPORABNIŠKA IMENA";
            btnPasswords.Content = "GESLA";
            btnOther.Content = "DRUGO";
            btnNotes.Content = "ZAPISKI";
            editHeader.Content = "UREDI";

            //Tooltips
            eng.IsChecked = false;
        }

        private void ColorPicker(object sender, RoutedEventArgs e)
        {
            ColorPicker colorPicker = new ColorPicker();
            colorPicker.Show();
        }

        /*TOOLS*/
        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
        }

        private void DecryptImportedData(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                Utility.decryptListData(Utility.ListData);
            }

            catch
            {
                _ = MessageBox.Show("Cannot decrypt data that is encrypted incorrectly", "Decryption", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Mouse.OverrideCursor = null;
        }

        private void DecryptOverwriteImportedData(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                Utility.decryptListData(Utility.ListData);

                File.WriteAllText(Environment.CurrentDirectory + "\\Data.xml", string.Empty); // clear file

                Stream stream = File.OpenWrite(Environment.CurrentDirectory + "\\Data.xml");

                try
                {
                    using (stream)
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));
                        xs.Serialize(stream, Utility.ListData);
                    }
                }

                catch
                {
                    _ = MessageBox.Show("Error occurred when trying to serialize data", "Error serializing", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            catch
            {
                _ = MessageBox.Show("Cannot decrypt data that is encrypted incorrectly", "Decryption", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Mouse.OverrideCursor = null;
        }

        // encrypt data.xml file (from toolbar)
        private void EncryptToolbar(object sender, RoutedEventArgs e)
        {
            // overwrite data.xml with empty string
            File.WriteAllText(Environment.CurrentDirectory + "\\Data.xml", string.Empty);

            // first decryption so that encrypted string doesnt stack
            try
            {
                Utility.decryptListData(Utility.ListData);
            }
            catch { }

            //encryption
            Utility.encryptListData(Utility.ListData);

            //serialization of ListData
            Stream stream = File.OpenWrite(Environment.CurrentDirectory + "\\Data.xml");

            try
            {
                using (stream)
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));
                    xs.Serialize(stream, Utility.ListData);
                }
            }

            catch
            {
                _ = MessageBox.Show("Error occurred when trying to serialize data", "Error serializing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /*HELP*/
        private void ViewHelp(object sender, RoutedEventArgs e)
        {
            string path = new Uri(AppDomain.CurrentDomain.BaseDirectory, UriKind.Absolute).ToString();
            var file = System.IO.Path.Combine(path, "Help.txt");
            Process.Start(file);
        }

        // HIDE COLUMNS - LISTVIEW MENU
        private void HideColumn(object sender, RoutedEventArgs e)
        {
            string which = (sender as MenuItem).Header.ToString();

            if (which == "Emails")
            {
                HelperHideColumn(emailColumn, clickedAgainE);
                if (clickedAgainE == false)
                    clickedAgainE = true;
                else
                    clickedAgainE = false;
            }

            else if (which == "Usernames")
            {
                HelperHideColumn(usernameColumn, clickedAgainU);
                if (clickedAgainU == false)
                    clickedAgainU = true;
                else
                    clickedAgainU = false;
            }

            else if (which == "Passwords")
            {
                HelperHideColumn(passwordColumn, clickedAgainP);
                if (clickedAgainP == false)
                    clickedAgainP = true;
                else
                    clickedAgainP = false;
            }

            else if (which == "Contacts/other")
            {
                HelperHideColumn(otherColumn, clickedAgainO);
                if (clickedAgainO == false)
                    clickedAgainO = true;
                else
                    clickedAgainO = false;
            }

            else if (which == "Notes")
            {
                HelperHideColumn(noteColumn, clickedAgainN);
                if (clickedAgainN == false)
                    clickedAgainN = true;
                else
                    clickedAgainN = false;
            }
        }

        private void HelperHideColumn(GridViewColumn gridViewColumn, bool clickedAgain)
        {
            if (clickedAgain == false)
            {
                gridViewColumn.Width = 0;
                //clickedAgain = true;
            }

            else if (clickedAgain == true)
            {
                Binding myBinding = new Binding
                {
                    Source = editColumn,
                    Path = new PropertyPath("helperField"),
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                BindingOperations.SetBinding(gridViewColumn, GridViewColumn.WidthProperty, myBinding);

                //clickedAgain = false;
            }
        }

        // QUICK CASTS
        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (Utility.preferences.QuickCastEsc == true)
            {
                if (e.Key == Key.Escape)
                    Close();
            }
        }

        private void HandleSpace(object sender, KeyEventArgs e)
        {
            if (Utility.preferences.QuickCastSpace == true)
            {
                List<string> blankList = new List<string>();

                if (e.Key == Key.Space && spaceAgain == false)
                {
                    lvDataMain.ItemsSource = blankList;
                    spaceAgain = true;
                }

                else
                {
                    lvDataMain.ItemsSource = Utility.ListData;
                    spaceAgain = false;
                }
            }
        }

        private void CloseAllWindows()
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter > 0; intCounter--)
                App.Current.Windows[intCounter].Close();
        }

        /*DRAG AND DROP FUNCTIONALITY - Main Data ListView*/
        private void lv_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Get current mouse position
            startPoint = e.GetPosition(null);
        }

        // Helper to search up the VisualTree
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }

                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);

            return null;
        }

        private void lv_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed
                && (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance
                || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (listViewItem == null)
                    return; // Abort

                // Find the data behind the ListViewItem
                DataClass item = (DataClass)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);

                if (item == null)
                    return; // Abort

                // Initialize the drag & drop operation
                startIndex = lvDataMain.SelectedIndex;
                DataObject dragData = new DataObject("DataClass", item);
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void lv_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("DataClass") || sender != e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void lv_Drop(object sender, DragEventArgs e)
        {
            int index = -1;

            if (e.Data.GetDataPresent("DataClass") && sender == e.Source)
            {
                // Get the drop ListViewItem destination
                ListView listView = sender as ListView;
                ListViewItem listViewItem = FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                if (listViewItem == null)
                {
                    // Abort
                    e.Effects = DragDropEffects.None;
                    return;
                }

                // Find the data behind the ListViewItem
                DataClass item = (DataClass)listView.ItemContainerGenerator.ItemFromContainer(listViewItem);

                // Move item into observable collection 
                // (this will be automatically reflected to lstView.ItemsSource)
                e.Effects = DragDropEffects.Move;
                index = Utility.ListData.IndexOf(item);

                if (startIndex >= 0 && index >= 0)
                {
                    Utility.ListData.Move(startIndex, index);
                }

                startIndex = -1;        // Done!
            }
        }
    }
}

