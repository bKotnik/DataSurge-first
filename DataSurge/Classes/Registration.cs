using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
