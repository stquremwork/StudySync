using System;

namespace StudySync
{
    public static class UserSettingsManager
    {
        // Сохранение имени пользователя и названия базы данных
        public static void SaveUserSettings(string host, string userName, string databaseName)
        {
            Properties.Settings.Default.Host = host;
            Properties.Settings.Default.UserName = userName;
            Properties.Settings.Default.DatabaseName = databaseName;
            Properties.Settings.Default.Save();
        }

        // Загрузка сохраненных данных пользователя
        public static (string Host, string UserName, string DatabaseName) LoadUserSettings()
        {
            string host = Properties.Settings.Default.Host;
            string userName = Properties.Settings.Default.UserName;
            string databaseName = Properties.Settings.Default.DatabaseName;
            return (host, userName, databaseName);
        }

        // Очистка сохраненных данных пользователя
        public static void ClearUserSettings()
        {
            Properties.Settings.Default.Host = string.Empty;
            Properties.Settings.Default.UserName = string.Empty;
            Properties.Settings.Default.DatabaseName = string.Empty;
            Properties.Settings.Default.Save();
        }
    }
}