namespace DataSurge.Classes
{
    class ListDataClass
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

        // data (email, password,...)
        private string _data;
        public string Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        public ListDataClass(int number, string data)
        {
            Number = number;
            Data = data;
        }
        public ListDataClass() { }
    }
}
