using System;

namespace DataSurge.Classes.Utils
{
    public static class Assets
    {
        public static Uri SHOW_PASS_ICON = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\show_password_icon.ico", UriKind.Absolute);

        // edit and delete icons in edit column
        public static Uri EDIT_ICON = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\edit_icon.ico", UriKind.Absolute);
        public static Uri DELETE_ICON = new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\delete_icon_red.ico", UriKind.Absolute);

        public static bool done = false;
    }
}
