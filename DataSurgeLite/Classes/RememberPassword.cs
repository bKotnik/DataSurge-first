using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSurgeLite.Classes
{
    [Serializable]
    public class RememberPassword
    {
        private bool _rememberPass;
        public bool RememberPass
        {
            get
            {
                return _rememberPass;
            }
            set
            {
                _rememberPass = value;
            }
        }

        public RememberPassword(bool rmbPass)
        {
            RememberPass = rmbPass;
        }

        public RememberPassword() { }
    }
}
