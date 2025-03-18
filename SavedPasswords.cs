using System;
using System.IO;
using System.Windows.Forms;

namespace Kursach
{
    public static class SavedPasswords
    {
        private static readonly string FilePath = "saved_passwords.txt"; // Путь к файлу

        // Сохранение данных в файл
        public static void SaveCredentials(string username, string password)
        {
            try
            {
                // Записываем данные в файл
                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    writer.WriteLine(username);
                    writer.WriteLine(password);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Загрузка данных из файла
        public static (string Username, string Password) LoadCredentials()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    // Читаем данные из файла
                    using (StreamReader reader = new StreamReader(FilePath))
                    {
                        string username = reader.ReadLine();
                        string password = reader.ReadLine();
                        return (username, password);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return (null, null); // Если файл не существует или произошла ошибка
        }

        // Очистка данных
        public static void ClearCredentials()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath); // Удаляем файл
            }
        }
    }
}