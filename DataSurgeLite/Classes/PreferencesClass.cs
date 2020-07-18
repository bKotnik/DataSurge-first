using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSurgeLite.Classes
{
    [Serializable]
    public class PreferencesClass
    {
        // space quick cast option
        private bool _quickCastSpace;
        public bool QuickCastSpace
        {
            get
            {
                return _quickCastSpace;
            }
            set
            {
                _quickCastSpace = value;
            }
        }

        // ESC quick cast option
        private bool _quickCastEsc;
        public bool QuickCastEsc
        {
            get
            {
                return _quickCastEsc;
            }
            set
            {
                _quickCastEsc = value;
            }
        }

        // Maximum security option
        private bool _maximumSecurity;
        public bool MaximumSecurity
        {
            get
            {
                return _maximumSecurity;
            }
            set
            {
                _maximumSecurity = value;
            }
        }

        public PreferencesClass(bool space, bool esc, bool maximum)
        {
            QuickCastSpace = space;
            QuickCastEsc = esc;
            MaximumSecurity = maximum;
        }
        public PreferencesClass() { }
    }
}
