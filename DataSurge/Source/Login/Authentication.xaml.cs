using DataSurge.Classes;
using DataSurge.Login;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace DataSurge.Source.Login
{
    /// <summary>
    /// Interaction logic for Authentication.xaml
    /// </summary>
    public partial class Authentication : Window
    {
        public Authentication()
        {
            InitializeComponent();

            Utility.ListData = new ObservableCollection<DataClass>();

            Utility.User = new Registration();
            Utility.rmbPassword = new RememberPassword();
            Utility.OnLoad();

            /*get remember password value from file*/
            RememberPass();
        }

        private void RememberPass()
        {
            XmlSerializer xs = new XmlSerializer(typeof(RememberPassword));
            FileStream fsin = new FileStream("RememberPassword.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            try
            {
                using (fsin)
                {
                    Utility.rmbPassword = (RememberPassword)xs.Deserialize(fsin);
                }
            }

            catch
            {
                if (fsin.Length != 0)
                    _ = MessageBox.Show("Error occurred when trying to get value from file", "Error loading", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            chkBoxRememberPass.IsChecked = Utility.rmbPassword.RememberPass;

            if (chkBoxRememberPass.IsChecked.Value == true)
            {
                XmlSerializer xsUser = new XmlSerializer(typeof(Registration));
                FileStream fsout = new FileStream(Environment.CurrentDirectory + "\\Users.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                try
                {
                    using (fsout)
                    {
                        Utility.User = (Registration)xsUser.Deserialize(fsout);

                        //Decrypt
                        Utility.User.password = Utility.Decrypt(Utility.User.password);
                    }
                }

                catch
                {
                    chkBoxRememberPass.IsChecked = false;
                }

                MasterPasswordBox.Password = Utility.User.password;
            }
        }

        private async void LoginAsync(object sender, RoutedEventArgs e)
        {
            GetData();

            string password = MasterPasswordBox.Password; // kar uporabnik vnese 

            XmlSerializer xs = new XmlSerializer(typeof(Registration));

            if (File.Exists(Environment.CurrentDirectory + "\\Users.xml"))
            {
                FileStream fsout = new FileStream(Environment.CurrentDirectory + "\\Users.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                try
                {
                    using (fsout)
                    {
                        Utility.User = (Registration)xs.Deserialize(fsout);

                        //Decrypt
                        Utility.User.password = Utility.Decrypt(Utility.User.password);
                    }
                }

                catch
                {
                    Incorrect_pass.Text = "An error has occured";
                    Incorrect_pass.Visibility = Visibility.Visible;
                }

                if (password == Utility.User.password)
                {
                    await IsDecryptNeededAsync();
                    MainWindow main = new MainWindow();
                    main.Show();
                    Close();
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
            RegistrationHelper();
        }

        private async void EnterLoginAsync(object sender, KeyEventArgs e)
        {
            GetData();

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
                            Utility.User = (Registration)xs.Deserialize(fsout);

                            //Decrypt
                            Utility.User.password = Utility.Decrypt(Utility.User.password);
                        }
                    }

                    catch
                    {
                        Incorrect_pass.Text = "An error has occured";
                        Incorrect_pass.Visibility = Visibility.Visible;
                    }

                    if (password == Utility.User.password)
                    {
                        await IsDecryptNeededAsync();
                        MainWindow main = new MainWindow();
                        main.Show();
                        Close();
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
            if (e.Key == Key.Enter)
            {
                RegistrationHelper();
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

        //check if email is valid
        private bool IsValid(string email)
        {
            try
            {
                MailAddress mailAddress = new MailAddress(email);

                return true;
            }

            catch (Exception e)
            {
                if (e is FormatException || e is ArgumentException)
                    return false;

                return false;
            }
        }

        //send email upon successful registration
        private void SendEmail()
        {
            string bodyMessage = "Hello,\nThank you for registering to DataSurge.\n\n" +
                                 "Your encryption key used for encrypting your data: ";

            SmtpClient client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = "zaboloj@gmail.com",
                    Password = "vncipzdlvwswudki"
                }
            };

            MailAddress fromEmail = new MailAddress("zaboloj@gmail.com", "DataSurge");
            MailAddress toEmail = new MailAddress(email_box.Text, email_box.Text);
            MailMessage message = new MailMessage()
            {
                From = fromEmail,
                Subject = "DataSurge - encryption key",
                Body = bodyMessage + Properties.Settings.Default.EncryptionKey +
                       "\n\n*We recommend that you save this key*\n\n" + "Best regards,\nKecktz Solutions"
            };

            message.To.Add(toEmail);
            client.SendCompleted += Client_SendCompleted;
            client.SendMailAsync(message);
        }

        private void Client_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error occurred - couldn't send email\n" + e.Error.Message, "Error");
                return;
            }
        }

        private void CreateKey()
        {
            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");
            guidString = guidString.Remove(6);
            Properties.Settings.Default.EncryptionKey = guidString;
            Properties.Settings.Default.Save();
        }

        private void RegistrationHelper()
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
                bool isValid = IsValid(email_box.Text);

                if (password_box.Password == password_repeat.Password && email_box.Text != "" && password_box.Password != "" && isValid == true)
                {
                    pass_error.Visibility = Visibility.Hidden;
                    email_error.Visibility = Visibility.Hidden;
                    pass1_error.Visibility = Visibility.Hidden;
                    status_error.Visibility = Visibility.Hidden;

                    //create encryption key with GUID
                    CreateKey();

                    //send email
                    SendEmail();

                    //Encryption
                    password_box.Password = Utility.Encrypt(password_box.Password);
                    email_box.Text = Utility.Encrypt(email_box.Text);

                    Utility.User.password = password_box.Password;
                    Utility.User.email = email_box.Text;

                    XmlSerializer xs = new XmlSerializer(typeof(Registration));

                    try
                    {
                        using (fsout)
                        {
                            xs.Serialize(fsout, Utility.User);
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

                    if (isValid == false && email_box.Text != "")
                    {
                        email_error.Text = "Email not valid";
                        email_error.Visibility = Visibility.Visible;
                    }

                    if (password_box.Password == password_repeat.Password && password_box.Password != "")
                    {
                        pass1_error.Visibility = Visibility.Hidden;
                        pass_error.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        private void GetData()
        {
            FileStream stream;
            string filePath = Environment.CurrentDirectory + "\\Data.xml";

            if (!File.Exists(filePath))
                stream = File.Create(filePath);
            else
                stream = File.OpenRead(filePath);

            XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<DataClass>));

            try
            {
                Utility.ListData = (ObservableCollection<DataClass>)xs.Deserialize(stream);
            }

            catch
            {
                if (stream.Length != 0)
                    _ = MessageBox.Show("Error occurred when trying to load data", "Error loading data", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            stream.Close();
        }

        // check if decrypt is needed
        private async Task IsDecryptNeededAsync()
        {
            if (Properties.Settings.Default.ToolbarWarning == false) // decrypt needed -> data is encrypted
            {
                try
                {
                    SetWindowOpacity(0.3);
                    LoadingUC.Visibility = Visibility.Visible;

                    await RunDecryptionAsync();
                }
                catch { }
            }
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

        private void SetWindowOpacity(double opacity)
        {
            btnExit.Opacity = opacity;
            btnForgottenPassword.Opacity = opacity;
            btnLogin.Opacity = opacity;
            btnRegister.Opacity = opacity;

            LoginLabel.Opacity = opacity;
            RegisterLabel.Opacity = opacity;

            VortexUC.Opacity = opacity;

            email_box.Opacity = opacity;
            password_box.Opacity = opacity;
            chkBoxRememberPass.Opacity = opacity;
            MasterPasswordBox.Opacity = opacity;
            password_repeat.Opacity = opacity;

            RmbPassBlock.Opacity = opacity;
            MasterPassBlock.Opacity = opacity;
            EnterEmailTxtBlock.Opacity = opacity;
            EnterPassBlock.Opacity = opacity;
            RepeatPassTxtblock.Opacity = opacity;

            LogBotLine.Opacity = opacity;
            LogLeftLine.Opacity = opacity;
            LogRightLine.Opacity = opacity;
            LogTopLine.Opacity = opacity;
            RegBotLine.Opacity = opacity;
            RegleftLine.Opacity = opacity;
            RegRightLine.Opacity = opacity;
            RegTopLine.Opacity = opacity;
        }
    }
}
