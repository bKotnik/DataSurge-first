using System;

namespace DataSurge.Classes
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
