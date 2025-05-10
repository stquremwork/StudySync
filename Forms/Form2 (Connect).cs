using System;
using System.Data;
using System.Windows.Forms;
using Kursach.Properties;
using Npgsql;

namespace Kursach
{
    public partial class Form2 : Form
    {
        public static string ConnectionString { get; private set; }
        public static string SelectedTable { get; private set; }
        private bool isConnected = false;
        private NpgsqlConnection connection;

        public Form2()
        {
            InitializeComponent();
            this.FormClosing += Form2_FormClosing;
        }

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
                MessageBox.Show("Заполните параметры подключения", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                connection = new NpgsqlConnection(ConnectionString);
                connection.Open();

                MessageBox.Show("Подключение успешно!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateTableList(connection);

                TestConnectionButton.Text = "Отключиться";
                isConnected = true;
                button1.Enabled = true;
            }
            catch (Npgsql.PostgresException pe)
            {
                MessageBox.Show($"Ошибка PostgreSQL: {pe.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!isConnected && connection != null)
                {
                    connection.Dispose();
                    connection = null;
                }
            }
        }

        private void UpdateConnectionString()
        {
            if (HostTextBox.Text.StartsWith("postgresql://"))
            {
                ConnectionString = HostTextBox.Text;
                return;
            }

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = HostTextBox.Text,
                Port = int.TryParse(PortTextBox.Text, out int port) ? port : 5432,
                Database = DatabaseTextBox.Text,
                Username = UserTextBox.Text,
                Password = PasswordTextBox.Text,
                SslMode = SslMode.Prefer,
                Pooling = true,
                Timeout = 15
            };

            ConnectionString = builder.ToString();
        }

        private void UpdateTableList(NpgsqlConnection conn)
        {
            try
            {
                string query = checkBox1.Checked
                    ? @"SELECT table_schema, table_name 
                        FROM information_schema.tables 
                        WHERE table_type = 'BASE TABLE' AND table_schema = 'public'
                        ORDER BY table_schema, table_name;"
                    : @"SELECT table_schema, table_name 
                        FROM information_schema.tables 
                        WHERE table_type = 'BASE TABLE'
                        ORDER BY table_schema, table_name;";

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    comboBox1.BeginUpdate();
                    comboBox1.Items.Clear();

                    while (reader.Read())
                    {
                        comboBox1.Items.Add($"{reader.GetString(0)}.{reader.GetString(1)}");
                    }

                    comboBox1.EndUpdate();

                    if (comboBox1.Items.Count > 0)
                    {
                        comboBox1.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке таблиц: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisconnectFromDatabase()
        {
            try
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                    connection = null;

                    MessageBox.Show("Отключено от базы данных", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отключении: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isConnected = false;
                TestConnectionButton.Text = "Подключиться";
                comboBox1.Items.Clear();
                button1.Enabled = false;
                CloseDataTabsInForm1();
            }
        }

        private void CloseDataTabsInForm1()
        {
            if (Form1.Instance != null && !Form1.Instance.IsDisposed)
            {
                Form1.Instance.CloseAllDataTabs();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                SelectedTable = comboBox1.SelectedItem.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedTable))
            {
                MessageBox.Show("Выберите таблицу из списка", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string tableName = SelectedTable;
                DataTable dataTable = Form1.Instance.GetTableData(tableName);
                Form1.Instance.LoadData(dataTable, tableName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (isConnected && connection != null)
            {
                UpdateTableList(connection);
            }
        }

        private void SaveSettings()
        {
            if (checkBox2.Checked)
            {
                Properties.Settings.Default.UserName = UserTextBox.Text;
                Properties.Settings.Default.DatabaseName = DatabaseTextBox.Text;
                Properties.Settings.Default.Host = HostTextBox.Text;
                Properties.Settings.Default.Port = PortTextBox.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.UserName = string.Empty;
                Properties.Settings.Default.DatabaseName = string.Empty;
                Properties.Settings.Default.Host = string.Empty;
                Properties.Settings.Default.Port = string.Empty;
                Properties.Settings.Default.Save();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Загрузка сохраненных настроек
            UserTextBox.Text = Settings.Default.UserName;
            DatabaseTextBox.Text = Settings.Default.DatabaseName;
            HostTextBox.Text = Settings.Default.Host;
            PortTextBox.Text = Settings.Default.Port;

            checkBox2.Checked = !string.IsNullOrEmpty(Settings.Default.UserName);
            TestConnectionButton.Text = "Подключиться";
            button1.Enabled = false;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HostTextBox_TextChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void PortTextBox_TextChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void DatabaseTextBox_TextChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}