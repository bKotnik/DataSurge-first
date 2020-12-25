using System;

namespace DataSurge.Classes
{
    [Serializable]
    public class Registration
    {
        private string Password;

        public string password
        {
            get
            {
                return Password;
            }

            set
            {
                Password = value;
            }
        }

        private string Email;

        public string email
        {
            get
            {
                return Email;
            }

            set
            {
                Email = value;
            }
        }
    }
}
