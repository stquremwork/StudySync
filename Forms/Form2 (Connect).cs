using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Kursach
{
    public partial class Form2 : Form
    {
        // Статическая строка подключения к базе данных
        public static string ConnectionString { get; private set; }

        // Статическая переменная для хранения имени выбранной таблицы
        public static string SelectedTable { get; private set; }

        public Form2()
        {
            // Инициализация компонентов формы
            InitializeComponent();

            // Подписка на событие закрытия формы
            this.FormClosing += new FormClosingEventHandler(Form2_FormClosing);
        }

        // Обработчик события клика на кнопку "Test Connection"
        private void TestConnectionButton_Click(object sender, EventArgs e)
        {
            UpdateConnectionString();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();
                    MessageBox.Show("Connection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UpdateTableList(conn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Метод для обновления строки подключения в зависимости от выбранного способа подключения
        private void UpdateConnectionString()
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();

            if (UseUrlCheckBox.Checked)
            {
                // Обрабатываем URL
                try
                {
                    // Разбираем URL и устанавливаем параметры
                    Uri uri = new Uri(UrlTextBox.Text);
                    builder.Host = uri.Host;
                    builder.Port = uri.Port > 0 ? uri.Port : 5432;
                    builder.Database = uri.AbsolutePath.TrimStart('/');

                    if (!string.IsNullOrEmpty(uri.UserInfo))
                    {
                        string[] userInfoParts = uri.UserInfo.Split(':');
                        if (userInfoParts.Length > 0)
                        {
                            builder.Username = userInfoParts[0];
                            if (userInfoParts.Length > 1)
                            {
                                builder.Password = userInfoParts[1];
                            }
                        }
                    }

                    builder.SslMode = SslMode.Require;
                    builder.TrustServerCertificate = true;
                }
                catch (UriFormatException ex)
                {
                    MessageBox.Show($"Неверный формат URL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обработке URL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                // Используем отдельные поля для подключения
                builder.Host = HostTextBox.Text;
                builder.Port = int.Parse(PortTextBox.Text);
                builder.Database = DatabaseTextBox.Text;
                builder.Username = UserTextBox.Text;
                builder.Password = PasswordTextBox.Text;
                builder.SslMode = SslMode.Require;
                builder.TrustServerCertificate = true;
            }

            ConnectionString = builder.ConnectionString;
            //MessageBox.Show($"Connection string: {ConnectionString}"); // Для отладки - можно раскомментировать для проверки
        }

        // Метод для обновления списка таблиц в базе данных
        private void UpdateTableList(NpgsqlConnection conn)
        {
            string query;

            // Если выбрано только публичные схемы
            if (checkBox1.Checked)
            {
                // Формируем запрос для публичных таблиц
                query = @"
                    SELECT table_schema, table_name
                    FROM information_schema.tables
                    WHERE table_type = 'BASE TABLE' AND table_schema = 'public'
                    ORDER BY table_schema, table_name;";
            }
            else
            {
                // Формируем запрос для всех таблиц
                query = @"
                    SELECT table_schema, table_name
                    FROM information_schema.tables
                    WHERE table_type = 'BASE TABLE'
                    ORDER BY table_schema, table_name;";
            }

            // Создаем команду SQL
            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                // Очищаем список таблиц
                comboBox1.Items.Clear();

                // Читаем результаты запроса
                while (reader.Read())
                {
                    string tableSchema = reader.GetString(0);
                    string tableName = reader.GetString(1);

                    // Добавляем таблицы в список
                    comboBox1.Items.Add($"{tableSchema}.{tableName}");
                }
            }

            // Выбираем первую таблицу по умолчанию
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        // Обработчик события изменения выбранной таблицы в списке
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Если выбрана таблица
            if (comboBox1.SelectedItem != null)
            {
                // Сохраняем имя выбранной таблицы
                SelectedTable = comboBox1.SelectedItem.ToString();
            }
        }

        // Обработчик события клика на кнопку "Send data"
        private void button1_Click(object sender, EventArgs e)
        {
            // Если не выбрана таблица
            if (string.IsNullOrEmpty(SelectedTable))
            {
                // Выводим предупреждение
                MessageBox.Show("Выберите таблицу из списка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Получаем имя выбранной таблицы
            string tableName = SelectedTable;

            // Получаем данные таблицы
            DataTable dataTable = Form1.Instance.GetTableData(tableName);

            // Загружаем данные в новую вкладку
            Form1.Instance.LoadData(dataTable, tableName);
        }


        // Обработчик события изменения состояния чекбокса "Only Public schemes"
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            UpdateTableListBasedOnConnection();
        }

        // Обновление списка таблиц на основе текущего соединения
        private void UpdateTableListBasedOnConnection()
        {
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                try
                {
                    using (var conn = new NpgsqlConnection(ConnectionString))
                    {
                        conn.Open();
                        UpdateTableList(conn);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении списка таблиц: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Обработчик события изменения текста в поле "User"
        private void UserTextBox_TextChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        // Обработчик события изменения состояния чекбокса "Remember settings"
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        // Метод сохранения настроек
        private void SaveSettings()
        {
            if (checkBox2.Checked)
            {
                // Сохраняем имя пользователя и базы данных в настройках
                Properties.Settings.Default.UserName = UserTextBox.Text;
                Properties.Settings.Default.DatabaseName = DatabaseTextBox.Text;
            }
            else
            {
                // Очищаем настройки
                Properties.Settings.Default.UserName = string.Empty;
                Properties.Settings.Default.DatabaseName = string.Empty;
            }
            Properties.Settings.Default.Save();
        }

        // Обработчик события загрузки формы
        private void Form2_Load(object sender, EventArgs e)
        {
            // Загружаем сохраненные настройки
            UserTextBox.Text = Properties.Settings.Default.UserName;
            DatabaseTextBox.Text = Properties.Settings.Default.DatabaseName;

            // Если сохранено имя пользователя
            if (!string.IsNullOrEmpty(Properties.Settings.Default.UserName))
            {
                // Включаем чекбокс "Remember settings"
                checkBox2.Checked = true;
            }

            // Если сохранен URL
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Url))
            {
                // Загружаем URL в текстовое поле
                UrlTextBox.Text = Properties.Settings.Default.Url;
                // Включаем чекбокс "Remember URL"
                RememberUrlCheckBox.Checked = true;
            }
        }

        // Обработчик события закрытия формы
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Если включен чекбокс "Remember settings"
            if (checkBox2.Checked)
            {
                // Сохраняем имя пользователя и базы данных в настройках
                Properties.Settings.Default.UserName = UserTextBox.Text;
                Properties.Settings.Default.DatabaseName = DatabaseTextBox.Text;
            }
            else
            {
                // Очищаем настройки
                Properties.Settings.Default.UserName = string.Empty;
                Properties.Settings.Default.DatabaseName = string.Empty;
            }

            // Если включен чекбокс "Remember URL"
            if (RememberUrlCheckBox.Checked)
            {
                // Сохраняем URL в настройках
                Properties.Settings.Default.Url = UrlTextBox.Text;
            }
            else
            {
                // Очищаем URL в настройках
                Properties.Settings.Default.Url = string.Empty;
            }
            Properties.Settings.Default.Save();
        }

        // Обработчик события изменения состояния чекбокса "Use URL"
        private void UseUrlCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUIElements();
        }

        // Метод для обновления доступности полей ввода в зависимости от состояния чекбокса "Use URL"
        private void UpdateUIElements()
        {
            // Если включен чекбокс "Use URL"
            if (UseUrlCheckBox.Checked)
            {
                // Отключаем поля для ввода данных
                HostTextBox.Enabled = false;
                PortTextBox.Enabled = false;
                DatabaseTextBox.Enabled = false;
                UserTextBox.Enabled = false;
                PasswordTextBox.Enabled = false;

                // Включаем поле для ввода URL
                UrlTextBox.Enabled = true;
            }
            else
            {
                // Включаем поля для ввода данных
                HostTextBox.Enabled = true;
                PortTextBox.Enabled = true;
                DatabaseTextBox.Enabled = true;
                UserTextBox.Enabled = true;
                PasswordTextBox.Enabled = true;

                // Отключаем поле для ввода URL
                UrlTextBox.Enabled = false;
            }
        }

        // Обработчик события изменения состояния чекбокса "Remember URL"
        private void RememberUrlCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // Если включен чекбокс "Remember URL"
            if (RememberUrlCheckBox.Checked)
            {
                // Сохраняем URL в настройках
                Properties.Settings.Default.Url = UrlTextBox.Text;
            }
            else
            {
                // Очищаем URL в настройках
                Properties.Settings.Default.Url = string.Empty;
            }
            Properties.Settings.Default.Save();
        }

        private void UrlTextBox_TextChanged(object sender, EventArgs e)
        {
            // Обновляем строку подключения при изменении текста в UrlTextBox
            if (UseUrlCheckBox.Checked)
            {
                UpdateConnectionString();
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}