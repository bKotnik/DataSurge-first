using DataSurge.Classes;
using DataSurge.Login;
using DataSurge.Main;
using DataSurge.Side_menu;
using DataSurge.Source.Login;
using DataSurge.Toolbar;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using WPFCustomMessageBox;

namespace DataSurge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // when clicking on hide columns (e - emails, u - usernames,...)
        //bool clickedAgainE = false; 
        //bool clickedAgainU = false;
        //bool clickedAgainP = false;
        //bool clickedAgainO = false;
        //bool clickedAgainN = false;
        bool spaceAgain = false;
        bool logout_pressed = false;

        // for drag and drop
        private int startIndex = -1;
        private Point startPoint = new Point();

        public MainWindow()
        {
            InitializeComponent();

            // Initialize list
            Utility.preferences = new PreferencesClass();

            // quick casts
            PreviewKeyDown += new KeyEventHandler(HandleEsc);
            PreviewKeyDown += new KeyEventHandler(HandleSpace);
            PreviewKeyDown += new KeyEventHandler(HandleDelete);
            PreviewKeyDown += new KeyEventHandler(HandleAddNew);

            // check toolbar warning!
            if (Properties.Settings.Default.ToolbarWarning == false)
                toolbarWarning.Visibility = Visibility.Hidden;
            else
                toolbarWarning.Visibility = Visibility.Visible;

            // get preferences
            GetPreferences();

            // get data
            GetData();
        }

        private void GetPreferences()
        {
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
        }

        private void GetData() // loads edit/delete icons and assigns ItemsSource of listview
        {
            //assign icons to objects
            foreach (DataClass data in Utility.ListData)
            {
                data.EditPath = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\edit_icon.ico", UriKind.Absolute)).ToString();
                data.DeletePath = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\delete_icon_red.ico", UriKind.Absolute)).ToString();
            }

            lvDataMain.ItemsSource = Utility.ListData;
        }

        /* LISTVIEW BUTTONS */
        private void Click_emails(object sender, RoutedEventArgs e)
        {
            WhichButton.buttonContent = (sender as Button).Content.ToString();
            WhichButton.listLabel = "Email";
            ListData emails = new ListData();

            emails.ShowDialog();
        }

        private void Click_usernames(object sender, RoutedEventArgs e)
        {
            WhichButton.buttonContent = (sender as Button).Content.ToString();
            WhichButton.listLabel = "Username";
            ListData usernames = new ListData();

            usernames.ShowDialog();
        }

        private void Click_passwords(object sender, RoutedEventArgs e)
        {
            WhichButton.buttonContent = (sender as Button).Content.ToString();
            WhichButton.listLabel = "Password";
            ListData passwords = new ListData();

            passwords.ShowDialog();
        }

        private void Click_other(object sender, RoutedEventArgs e)
        {
            WhichButton.buttonContent = (sender as Button).Content.ToString();
            WhichButton.listLabel = "Other";
            ListData other = new ListData();

            other.ShowDialog();
        }

        private void Click_note(object sender, RoutedEventArgs e)
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

        private void Btn_import(object sender, RoutedEventArgs e)
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

                            //show ! in toolbar
                            toolbarWarning.Visibility = Visibility.Visible;
                            Properties.Settings.Default.ToolbarWarning = true;
                            Properties.Settings.Default.Save();
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

        private void Btn_export(object sender, RoutedEventArgs e)
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

        private void AddNew(object sender, RoutedEventArgs e)
        {
            AddNew add_new_window = new AddNew(this);

            add_new_window.ShowDialog();

            lvDataMain.ItemsSource = Utility.ListData;
        }
        private void Logout(object sender, RoutedEventArgs e)
        {
            logout_pressed = true;
            CloseAllWindows();
            Authentication login = new Authentication();
            login.Show();
            Close();
        }

        /* EDIT COLUMN BUTTONS */
        private async void DeleteItem(object sender, RoutedEventArgs e)
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
                        await HelperDeleteItem();
                    }

                    Utility.confirm = false;
                }

                else
                {
                    await HelperDeleteItem();
                }
            }
        }

        private async Task HelperDeleteItem()
        {
            while (lvDataMain.SelectedItems.Count > 0)
            {
                Utility.ListData.Remove((DataClass)lvDataMain.SelectedItem);
            }

            File.WriteAllText(Environment.CurrentDirectory + "\\Data.xml", string.Empty);

            Stream stream = File.OpenWrite(Environment.CurrentDirectory + "\\Data.xml");
            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));

            try
            {
                using (stream)
                {
                    if(Properties.Settings.Default.ToolbarWarning == false) // data needs to be encrypted
                    {
                        await RunEncryptionAsync();
                        xs.Serialize(stream, Utility.ListData);

                        // decrypt - otherwise encryption stacks
                        await RunDecryptionAsync();
                    }

                    else
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

                Edit editWindow = new Edit(this);
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


        /*FILE*/
        private void OpenAddNew(object sender, RoutedEventArgs e)
        {
            AddNew add_new_window = new AddNew(this);

            add_new_window.ShowDialog();
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
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

                Edit editWindow = new Edit(this);
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

        // decrypt data.xml file (from toolbar)
        private async void DecryptToolbarAsync(object sender, RoutedEventArgs e)
        {
            LoadingState("Decrypting");

            File.WriteAllText(Environment.CurrentDirectory + "\\Data.xml", string.Empty); // clear file

            try
            {
                await RunDecryptionAsync();
                lvDataMain.ItemsSource = Utility.ListData;
                GetData();
            }

            catch
            {
                // it always catches exception but still does the decryption... idk
                //_ = MessageBox.Show("Cannot decrypt data that is encrypted incorrectly\nOR\nIs already decrypted", "Decryption", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            try
            {
                using (Stream stream = File.OpenWrite(Environment.CurrentDirectory + "\\Data.xml"))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));
                    xs.Serialize(stream, Utility.ListData);

                    //show ! in toolbar
                    toolbarWarning.Visibility = Visibility.Visible;
                    Properties.Settings.Default.ToolbarWarning = true;
                    Properties.Settings.Default.Save();
                }
            }

            catch
            {
                _ = MessageBox.Show("Error occurred when trying to serialize data", "Error serializing", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            StateGrid.Visibility = Visibility.Collapsed;
        }

        // encrypt data.xml file (from toolbar)
        private async void EncryptToolbarAsync(object sender, RoutedEventArgs e)
        {
            LoadingState("Encrypting");

            if (Properties.Settings.Default.ToolbarWarning == true)
            {
                // overwrite data.xml with empty string
                File.WriteAllText(Environment.CurrentDirectory + "\\Data.xml", string.Empty);

                // first decryption so that encrypted string doesnt stack
                try
                {
                    await RunDecryptionAsync();
                }
                catch { }

                //encryption
                await RunEncryptionAsync();

                //serialization of ListData
                Stream stream = File.OpenWrite(Environment.CurrentDirectory + "\\Data.xml");

                try
                {
                    using (stream)
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));
                        xs.Serialize(stream, Utility.ListData);

                        //hide ! in toolbar
                        toolbarWarning.Visibility = Visibility.Hidden;
                        Properties.Settings.Default.ToolbarWarning = false;
                        Properties.Settings.Default.Save();
                    }
                }

                catch
                {
                    _ = MessageBox.Show("Error occurred when trying to serialize data", "Error serializing", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //decrypt observable collection
                try
                {
                    await RunDecryptionAsync();
                    GetData(); // get source for edit/delete icons
                }
                catch
                {
                    _ = MessageBox.Show("Error occurred when trying to decrypt data", "Error decrypting", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            StateGrid.Visibility = Visibility.Collapsed;
        }

        /*HELP*/
        private void ViewHelp(object sender, RoutedEventArgs e)
        {
            string path = new Uri(AppDomain.CurrentDomain.BaseDirectory, UriKind.Absolute).ToString();
            var file = Path.Combine(path, "Help.txt");
            Process.Start(file);
        }

        private void ViewKey(object sender, RoutedEventArgs e)
        {
            ViewKey viewKey = new ViewKey();
            viewKey.Show();
        }

        // to change ! visibility from other windows
        public void SetToolbarWarningVisibility(Visibility visibility)
        {
            toolbarWarning.Visibility = visibility;
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
        private void HandleDelete(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
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
        }
        private void HandleAddNew(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A && Keyboard.Modifiers == ModifierKeys.Control) // ctrl + a
            {
                AddNew addNew = new AddNew(this);
                addNew.ShowDialog();
            }
        }

        private void  CloseAllWindows()
        {
            // save order of ListView data
            LoadingState("Saving State");
            File.WriteAllText(Environment.CurrentDirectory + "\\Data.xml", string.Empty);

            if(Properties.Settings.Default.ToolbarWarning == false) // data needs to be encrypted before writing to file
            {
                using(Stream stream = File.OpenWrite(Environment.CurrentDirectory + "\\Data.xml"))
                {
                    try
                    {
                        //await RunEncryptionAsync();
                        Utility.ListData = Utility.encryptListData(Utility.ListData);

                        XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));
                        xs.Serialize(stream, Utility.ListData);
                    }

                    catch
                    {
                        _ = MessageBox.Show("Error occurred when trying to save state", "Error saving state", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            else
            {
                using (Stream stream = File.OpenWrite(Environment.CurrentDirectory + "\\Data.xml"))
                {
                    try
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));
                        xs.Serialize(stream, Utility.ListData);
                    }
                    catch 
                    {
                        _ = MessageBox.Show("Error occurred when trying to serialize data", "Error serializing", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            for (int i = Application.Current.Windows.Count - 1; i > 0; i--)
                Application.Current.Windows[i].Close();

            StateGrid.Visibility = Visibility.Collapsed;
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

                startIndex = -1;
            }
        }

        // when pressed on X close all other windows
        protected override void OnClosed(EventArgs e)
        {
            if (logout_pressed == false)
                CloseAllWindows();
        }

        private async Task RunDecryptionAsync()
        {
            ObservableCollection<DataClass> tmp = new ObservableCollection<DataClass>();

            foreach (DataClass data in Utility.ListData)
            {
                tmp.Add(await Task.Run(() => Utility.DecryptDataClassAsync(data)));
            }

            Utility.ListData = tmp;
        }

        private async Task RunEncryptionAsync()
        {
            ObservableCollection<DataClass> tmp = new ObservableCollection<DataClass>();

            foreach (DataClass data in Utility.ListData)
            {
                tmp.Add(await Task.Run(() => Utility.EncryptDataClassAsync(data)));
            }

            Utility.ListData = tmp;
        }

        private void LoadingState(string state)
        {
            StateGrid.Visibility = Visibility.Visible;

            StateLbl.Content = state;
        }
    }
}