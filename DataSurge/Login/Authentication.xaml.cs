using DataSurge.Classes;
using DataSurge.User_controls;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace DataSurge.Login
{
    /// <summary>
    /// Interaction logic for Authentication.xaml
    /// </summary>
    public partial class Authentication : Window
    {
        public Authentication()
        {
            InitializeComponent();
            Utility.ListUsers = new Registration();
            Utility.rmbPassword = new RememberPassword();
            Utility.OnLoad();

            /*get remember password value from file*/
            XmlSerializer xs = new XmlSerializer(typeof(RememberPassword));
            FileStream fsin = new FileStream("RememberPassword.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            try
            {
                Utility.rmbPassword = (RememberPassword)xs.Deserialize(fsin);
            }

            catch
            {
                if(fsin.Length != 0)
                    _ = MessageBox.Show("Error occurred when trying to get value from file", "Error loading", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            fsin.Close();

            chkBoxRememberPass.IsChecked = Utility.rmbPassword.RememberPass;

            if(chkBoxRememberPass.IsChecked.Value == true)
            {
                XmlSerializer xsUser = new XmlSerializer(typeof(Registration));
                FileStream fsout = new FileStream(Environment.CurrentDirectory + "\\Users.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                try
                {
                    using (fsout)
                    {
                        Utility.ListUsers = (Registration)xsUser.Deserialize(fsout);

                        //Decrypt
                        Utility.ListUsers.password = Utility.Decrypt(Utility.ListUsers.password);
                    }
                }

                catch
                {
                    //Incorrect_pass.Text = "An error has occured121212";
                    //Incorrect_pass.Visibility = Visibility.Visible;
                    chkBoxRememberPass.IsChecked = false;
                }

                MasterPasswordBox.Password = Utility.ListUsers.password;
            }
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            string password = MasterPasswordBox.Password; // kar uporabnik vnese 

            XmlSerializer xs = new XmlSerializer(typeof(Registration));

            if (File.Exists(Environment.CurrentDirectory + "\\Users.xml"))
            {
                FileStream fsout = new FileStream(Environment.CurrentDirectory + "\\Users.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                try
                {
                    using (fsout)
                    {
                        Utility.ListUsers = (Registration)xs.Deserialize(fsout);

                        //Decrypt
                        Utility.ListUsers.password = Utility.Decrypt(Utility.ListUsers.password);
                    }
                }

                catch
                {
                    Incorrect_pass.Text = "An error has occured";
                    Incorrect_pass.Visibility = Visibility.Visible;
                }

                if (password == Utility.ListUsers.password)
                {
                    MainWindow main = new MainWindow();
                    main.Show();
                    this.Close();
                }

                else
                {
                    Incorrect_pass.Text = "Incorrect password!";
                    Incorrect_pass.Visibility = Visibility.Visible;

                    btnForgottenPassword.Visibility = Visibility.Visible;
                }
            }

            else
            {
                Incorrect_pass.Text = "Incorrect password!";
                Incorrect_pass.Visibility = Visibility.Visible;
            }
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            FileStream fsout = new FileStream(Environment.CurrentDirectory + "\\Users.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

            if (fsout.Length > 0)
            {
                status_error.Text = "User already exists!";
                status_error.Visibility = Visibility.Visible;
                status_success.Visibility = Visibility.Hidden;
            }

            else
            {
                if (password_box.Password == password_repeat.Password && email_box.Text != "" && password_box.Password != "")
                {
                    pass_error.Visibility = Visibility.Hidden;
                    email_error.Visibility = Visibility.Hidden;
                    pass1_error.Visibility = Visibility.Hidden;
                    status_error.Visibility = Visibility.Hidden;

                    //create encryption key with GUID
                    Guid g = Guid.NewGuid();
                    string guidString = Convert.ToBase64String(g.ToByteArray());
                    guidString = guidString.Replace("=", "");
                    guidString = guidString.Replace("+", "");
                    guidString = guidString.Remove(6);
                    Properties.Settings.Default.EncryptionKey = guidString;
                    Properties.Settings.Default.Save();

                    //Encryption
                    password_box.Password = Utility.Encrypt(password_box.Password);
                    email_box.Text = Utility.Encrypt(email_box.Text);

                    Utility.ListUsers.password = password_box.Password;
                    Utility.ListUsers.email = email_box.Text;

                    XmlSerializer xs = new XmlSerializer(typeof(Registration));

                    try
                    {
                        using (fsout)
                        {
                            xs.Serialize(fsout, Utility.ListUsers);
                            status_success.Visibility = Visibility.Visible;
                            //show encryption key warning
                            encryptionKeyWarningUserControl.Visibility = Visibility.Visible;
                            encryptionKeyWarningUserControl.encryptionKeyBox.Text = Properties.Settings.Default.EncryptionKey;
                            clearFields();
                        }
                    }
                    catch
                    {
                        status_error.Visibility = Visibility.Visible;
                    }
                }

                else
                {
                    if (password_box.Password != password_repeat.Password)
                    {
                        pass_error.Visibility = Visibility.Visible;
                    }

                    if (email_box.Text == "")
                    {
                        email_error.Visibility = Visibility.Visible;
                    }

                    if (password_box.Password == "")
                    {
                        pass1_error.Visibility = Visibility.Visible;
                    }

                    if (email_box.Text == "" && password_box.Password == "")
                    {
                        email_error.Visibility = Visibility.Visible;
                        pass1_error.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void EnterLogin(object sender, KeyEventArgs e)
        {
            if (File.Exists(Environment.CurrentDirectory + "\\Users.xml") && e.Key == Key.Enter)
            {
                if (e.Key == Key.Enter)
                {
                    string password = MasterPasswordBox.Password; // kar uporabnik vnese 

                    XmlSerializer xs = new XmlSerializer(typeof(Registration));
                    FileStream fsout = new FileStream(Environment.CurrentDirectory + "\\Users.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                    try
                    {
                        using (fsout)
                        {
                            Utility.ListUsers = (Registration)xs.Deserialize(fsout);

                            //Decrypt
                            Utility.ListUsers.password = Utility.Decrypt(Utility.ListUsers.password);
                        }
                    }

                    catch
                    {
                        Incorrect_pass.Text = "An error has occured";
                        Incorrect_pass.Visibility = Visibility.Visible;
                    }

                    if (password == Utility.ListUsers.password)
                    {
                        MainWindow main = new MainWindow();

                        main.Show();
                        this.Close();
                    }

                    else
                    {
                        Incorrect_pass.Text = "Incorrect password!";
                        Incorrect_pass.Visibility = Visibility.Visible;

                        btnForgottenPassword.Visibility = Visibility.Visible;
                    }
                }
            }

            else if (!File.Exists(Environment.CurrentDirectory + "\\Users.xml") && e.Key == Key.Enter)
            {
                Incorrect_pass.Text = "Incorrect password!";
                Incorrect_pass.Visibility = Visibility.Visible;
            }
        }

        private void EnterRegistration(object sender, KeyEventArgs e)
        {
            FileStream fsout = new FileStream(Environment.CurrentDirectory + "\\Users.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            if ((fsout.Length > 0) && (e.Key == Key.Enter))
            {
                status_error.Text = "User already exists!";
                status_error.Visibility = Visibility.Visible;
                status_success.Visibility = Visibility.Hidden;
            }

            else if ((fsout.Length == 0) && (e.Key == Key.Enter))
            {
                if (password_box.Password == password_repeat.Password && email_box.Text != "" && password_box.Password != "")
                {
                    pass_error.Visibility = Visibility.Hidden;
                    email_error.Visibility = Visibility.Hidden;
                    pass1_error.Visibility = Visibility.Hidden;
                    status_error.Visibility = Visibility.Hidden;

                    //Encryption
                    password_box.Password = Utility.Encrypt(password_box.Password);
                    email_box.Text = Utility.Encrypt(email_box.Text);

                    Utility.ListUsers.password = password_box.Password;
                    Utility.ListUsers.email = email_box.Text;

                    XmlSerializer xs = new XmlSerializer(typeof(Registration));

                    try
                    {
                        using (fsout)
                        {
                            xs.Serialize(fsout, Utility.ListUsers);
                            status_success.Visibility = Visibility.Visible;
                            //show encryption key warning
                            encryptionKeyWarningUserControl.Visibility = Visibility.Visible;
                            encryptionKeyWarningUserControl.encryptionKeyBox.Text = Properties.Settings.Default.EncryptionKey;
                            clearFields();
                        }
                    }
                    catch
                    {
                        status_error.Visibility = Visibility.Visible;
                    }
                }

                else
                {
                    if (password_box.Password != password_repeat.Password)
                    {
                        pass_error.Visibility = Visibility.Visible;
                    }

                    if (email_box.Text == "")
                    {
                        email_error.Visibility = Visibility.Visible;
                    }

                    if (password_box.Password == "")
                    {
                        pass1_error.Visibility = Visibility.Visible;
                    }

                    if (email_box.Text == "" && password_box.Password == "")
                    {
                        email_error.Visibility = Visibility.Visible;
                        pass1_error.Visibility = Visibility.Visible;
                    }
                }
            }

        }

        private void clearFields()
        {
            password_box.Clear();
            password_repeat.Clear();
            email_box.Clear();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ForgotPass(object sender, RoutedEventArgs e)
        {
            HelpContact help_Contact = new HelpContact();
            help_Contact.Show();
        }

        private void RememberPassword(object sender, RoutedEventArgs e)
        {
            Utility.rmbPassword.RememberPass = chkBoxRememberPass.IsChecked.Value;

            File.WriteAllText(Environment.CurrentDirectory + "\\RememberPassword.xml", string.Empty);
            XmlSerializer xs = new XmlSerializer(typeof(RememberPassword));
            FileStream fsout = new FileStream("RememberPassword.xml", FileMode.Open, FileAccess.Write, FileShare.None);

            try
            {
                using (fsout)
                {
                    xs.Serialize(fsout, Utility.rmbPassword);
                }
            }
            catch
            {
                _ = MessageBox.Show("Error occurred when trying to serialize data", "Error serializing", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
