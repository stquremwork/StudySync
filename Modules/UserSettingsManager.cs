using System;

namespace Kursach
{
    public static class UserSettingsManager
    {
        public static void SaveUserSettings(string userName, string databaseName, string host, string port)
        {
            Properties.Settings.Default.UserName = userName;
            Properties.Settings.Default.DatabaseName = databaseName;
            Properties.Settings.Default.Host = host;
            Properties.Settings.Default.Port = port;
            Properties.Settings.Default.Save();
        }

        public static (string UserName, string DatabaseName, string Host, string Port) LoadUserSettings()
        {
            return (
                Properties.Settings.Default.UserName,
                Properties.Settings.Default.DatabaseName,
                Properties.Settings.Default.Host,
                Properties.Settings.Default.Port
            );
        }

        public static void ClearUserSettings()
        {
            Properties.Settings.Default.UserName = string.Empty;
            Properties.Settings.Default.DatabaseName = string.Empty;
            Properties.Settings.Default.Host = string.Empty;
            Properties.Settings.Default.Port = string.Empty;
            Properties.Settings.Default.Save();
        }
    }
}