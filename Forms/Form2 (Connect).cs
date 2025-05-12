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

        // Флаг, указывающий, подключены ли мы к базе данных
        private bool isConnected = false;

        private NpgsqlConnection connection; // Поле для хранения соединения

        public Form2()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form2_FormClosing);
        }

        // Обработчик клика на кнопку "Test Connection"
        private void TestConnectionButton_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                ConnectToDatabase();
            }
            else
            {
                DisconnectFromDatabase();
            }
        }

        private void ConnectToDatabase()
        {
            UpdateConnectionString();

            if (string.IsNullOrEmpty(ConnectionString))
            {
                MessageBox.Show("Строка подключения не может быть пустой. Заполните параметры подключения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                connection = new NpgsqlConnection(ConnectionString);
                connection.Open();
                MessageBox.Show("Connection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateTableList(connection);

                TestConnectionButton.Text = "Disconnect";
                isConnected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isConnected = false;
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
            }
        }

        private void DisconnectFromDatabase()
        {
            if (connection != null)
            {
                try
                {
                    connection.Close();
                    MessageBox.Show("Disconnected from database.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error during disconnection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection = null;
                    isConnected = false;
                    TestConnectionButton.Text = "Connect";
                    comboBox1.Items.Clear();
                    CloseDataTabsInForm1();
                }
            }
            else
            {
                MessageBox.Show("Not connected to any database.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CloseDataTabsInForm1()
        {
            if (Form1.Instance != null && !Form1.Instance.IsDisposed)
            {
                Form1.Instance.CloseAllDataTabs();
            }
        }

        // Обновление строки подключения на основе введённых данных
        private void UpdateConnectionString()
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder
            {
                Host = HostTextBox.Text,
                Port = int.TryParse(PortTextBox.Text, out int port) ? port : 5432,
                Database = DatabaseTextBox.Text,
                Username = UserTextBox.Text,
                Password = PasswordTextBox.Text,
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };

            ConnectionString = builder.ConnectionString;
        }

        // Обновление списка таблиц в базе данных
        private void UpdateTableList(NpgsqlConnection conn)
        {
            string query = checkBox1.Checked
                ? @"
                    SELECT table_schema, table_name
                    FROM information_schema.tables
                    WHERE table_type = 'BASE TABLE' AND table_schema = 'public'
                    ORDER BY table_schema, table_name;"
                : @"
                    SELECT table_schema, table_name
                    FROM information_schema.tables
                    WHERE table_type = 'BASE TABLE'
                    ORDER BY table_schema, table_name;";

            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                comboBox1.Items.Clear();
                while (reader.Read())
                {
                    string tableSchema = reader.GetString(0);
                    string tableName = reader.GetString(1);
                    comboBox1

.Items.Add($"{tableSchema}.{tableName}");
                }
            }

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        // Обработчик изменения выбранной таблицы
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                SelectedTable = comboBox1.SelectedItem.ToString();
            }
        }

        // Обработчик клика на кнопку "Send data"
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedTable))
            {
                MessageBox.Show("Выберите таблицу из списка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tableName = SelectedTable;
            DataTable dataTable = Form1.Instance.GetTableData(tableName);
            Form1.Instance.LoadData(dataTable, tableName);
        }

        // Обработчик изменения состояния чекбокса "Only Public schemes"
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            UpdateTableListBasedOnConnection();
        }

        // Обновление списка таблиц на основе текущего соединения
        private void UpdateTableListBasedOnConnection()
        {
            if (!string.IsNullOrEmpty(ConnectionString) && isConnected && connection != null)
            {
                try
                {
                    UpdateTableList(connection);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении списка таблиц: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (!isConnected)
            {
                comboBox1.Items.Clear();
            }
        }

        // Обработчик изменения текста в поле "User"
        private void UserTextBox_TextChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        // Обработчик изменения состояния чекбокса "Remember settings"
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        // Сохранение настроек
        private void SaveSettings()
        {
            if (checkBox2.Checked)
            {
                Properties.Settings.Default.UserName = UserTextBox.Text;
                Properties.Settings.Default.DatabaseName = DatabaseTextBox.Text;
            }
            else
            {
                Properties.Settings.Default.UserName = string.Empty;
                Properties.Settings.Default.DatabaseName = string.Empty;
            }
            Properties.Settings.Default.Save();
        }

        // Обработчик загрузки формы
        private void Form2_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            UserTextBox.Text = Properties.Settings.Default.UserName;
            DatabaseTextBox.Text = Properties.Settings.Default.DatabaseName;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.UserName))
            {
                checkBox2.Checked = true;
            }

            TestConnectionButton.Text = "Connect";
        }

        // Обработчик закрытия формы
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkBox2.Checked)
            {
                Properties.Settings.Default.UserName = UserTextBox.Text;
                Properties.Settings.Default.DatabaseName = DatabaseTextBox.Text;
            }
            else
            {
                Properties.Settings.Default.UserName = string.Empty;
                Properties.Settings.Default.DatabaseName = string.Empty;
            }
            Properties.Settings.Default.Save();
            // Не разрываем соединение с базой данных
        }

        // Обработчик клика на кнопку закрытия формы
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close(); // Закрываем форму
        }

        private void tabPage1_Click(object sender, EventArgs e) { }
    }
}