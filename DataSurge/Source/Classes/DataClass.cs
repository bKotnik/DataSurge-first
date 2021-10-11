using DataSurge.Classes.Utils;
using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace DataSurge.Classes
{
    [Serializable]
    public class DataClass : INotifyPropertyChanged
    {
        // email
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyRaised("Email");
            }
        }

        // password
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyRaised("Password");
            }
        }

        // username
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyRaised("Username");
            }
        }

        // other
        private string _other;
        public string Other
        {
            get => _other;
            set
            {
                _other = value;
                OnPropertyRaised("Other");
            }
        }

        // note - header
        private string _note;
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyRaised("Note");
            }
        }

        // note - details
        private string _noteDetails;
        public string NoteDetails
        {
            get => _noteDetails;
            set
            {
                _noteDetails = value;
                OnPropertyRaised("NoteDetails");
            }
        }

        public string DeletePath { get; set; }
        public string EditPath { get; set; }

        public DataClass(string email = "/", string password = "/", string username = "/", string other = "/", string note = "/", string noteDetails = "")
        {
            Email = email;
            Password = password;
            Username = username;
            Other = other;
            Note = note;
            NoteDetails = noteDetails;
            DeletePath = new BitmapImage(Assets.DELETE_ICON).ToString();
            EditPath = new BitmapImage(Assets.EDIT_ICON).ToString();
        }

        public DataClass() { }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname)); // simplified
        }
    }
}
