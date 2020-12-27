namespace DataSurge.Classes
{
    class PasswordMagicianClass
    {
        // number
        private int _number;
        public int Number
        {
            get
            {
                return _number;
            }
            set
            {
                _number = value;
            }
        }

        // password
        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        // warning
        private string _warning;
        public string Warning
        {
            get
            {
                return _warning;
            }
            set
            {
                _warning = value;
            }
        }

        public PasswordMagicianClass(int number, string password, string warning)
        {
            Number = number;
            Password = password;
            Warning = warning;
        }
        public PasswordMagicianClass() { }
    }
}
