using System;

namespace Kursach
{
    public static class UserSettingsManager
    {
        // Сохранение имени пользователя и названия базы данных
        public static void SaveUserSettings(string userName, string databaseName)
        {
            Properties.Settings.Default.UserName = userName;
            Properties.Settings.Default.DatabaseName = databaseName;
            Properties.Settings.Default.Save();
        }

        // Загрузка сохраненных данных пользователя
        public static (string UserName, string DatabaseName) LoadUserSettings()
        {
            string userName = Properties.Settings.Default.UserName;
            string databaseName = Properties.Settings.Default.DatabaseName;
            return (userName, databaseName);
        }

        // Очистка сохраненных данных пользователя
        public static void ClearUserSettings()
        {
            Properties.Settings.Default.UserName = string.Empty;
            Properties.Settings.Default.DatabaseName = string.Empty;
            Properties.Settings.Default.Save();
        }
    }
}