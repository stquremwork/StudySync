using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
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
            this.FormClosing += new FormClosingEventHandler(Form2_FormClosing);


            // Устанавливаем фон окна
            this.BackColor = Color.FromArgb(33, 42, 57);


            tabPage3.BackColor = Color.FromArgb(45, 55, 70); // немного светлее
            tabPage4.BackColor = Color.FromArgb(45, 55, 70); // немного светлее





        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            var (host, userName, databaseName) = UserSettingsManager.LoadUserSettings();
            HostTextBox.Text = host;
            UserTextBox.Text = userName;
            DatabaseTextBox.Text = databaseName;

            if (!string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(databaseName) || !string.IsNullOrEmpty(host))
            {
                guna2CheckBoxRemSet.Checked = true;
            }

            guna2ButtonConnect.Text = "Connect";
        }

        // Event handlers for controls
        private void HostTextBox_TextChanged(object sender, EventArgs e)
        {
            SettingsTextBox_TextChanged(sender, e);
        }

        private void UserTextBox_TextChanged(object sender, EventArgs e)
        {
            SettingsTextBox_TextChanged(sender, e);
        }

        private void DatabaseTextBox_TextChanged(object sender, EventArgs e)
        {
            SettingsTextBox_TextChanged(sender, e);
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            SettingsTextBox_TextChanged(sender, e);
        }

        private void PortTextBox_TextChanged(object sender, EventArgs e)
        {
            SettingsTextBox_TextChanged(sender, e);
        }

        private void HostLabel_Click(object sender, EventArgs e)
        {
            // Optional: Add any specific behavior when host label is clicked
        }

        private void PortLabel_Click(object sender, EventArgs e)
        {
            // Optional: Add any specific behavior when port label is clicked
        }

        private void DatabaseLabel_Click(object sender, EventArgs e)
        {
            // Optional: Add any specific behavior when database label is clicked
        }

        private void UserLabel_Click(object sender, EventArgs e)
        {
            // Optional: Add any specific behavior when user label is clicked
        }

        private void PasswordLabel_Click(object sender, EventArgs e)
        {
            // Optional: Add any specific behavior when password label is clicked
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Optional: Add any specific behavior when label1 is clicked
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {
            // Optional: Add any specific behavior when tabPage4 is clicked
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            checkBox1_CheckedChanged(sender, e);
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            checkBox2_CheckedChanged(sender, e);
        }

        private void RememberUrlCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // This appears to be the same as checkBox2_CheckedChanged
            checkBox2_CheckedChanged(sender, e);
        }

        private void UrlTextBox_TextChanged(object sender, EventArgs e)
        {
            // If you have a UrlTextBox, handle its text changed event here
            SettingsTextBox_TextChanged(sender, e);
        }

        // Rest of your existing methods...
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

                guna2ButtonConnect.Text = "Disconnect";
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
                    guna2ComboBox1.Items.Clear();
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

        private void UpdateTableList(NpgsqlConnection conn)
        {
            string query = checkBox1.Checked
                ? "SELECT table_schema, table_name FROM information_schema.tables WHERE table_type = 'BASE TABLE' AND table_schema = 'public' ORDER BY table_schema, table_name;"
                : "SELECT table_schema, table_name FROM information_schema.tables WHERE table_type = 'BASE TABLE' ORDER BY table_schema, table_name;";

            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                guna2ComboBox1.Items.Clear();
                while (reader.Read())
                {
                    string tableSchema = reader.GetString(0);
                    string tableName = reader.GetString(1);
                    guna2ComboBox1.Items.Add($"{tableSchema}.{tableName}");
                }
            }

            if (guna2ComboBox1.Items.Count > 0)
            {
                guna2ComboBox1.SelectedIndex = 0;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedItem != null)
            {
                SelectedTable = guna2ComboBox1.SelectedItem.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTableListBasedOnConnection();
        }

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
                guna2ComboBox1.Items.Clear();
            }
        }

        private void SaveSettings()
        {
            if (guna2CheckBoxRemSet.Checked)
            {
                UserSettingsManager.SaveUserSettings(
                    HostTextBox.Text,
                    UserTextBox.Text,
                    DatabaseTextBox.Text
                );
            }
            else
            {
                UserSettingsManager.ClearUserSettings();
            }
        }

        private void SettingsTextBox_TextChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void guna2ButtonSendData_Click(object sender, EventArgs e)
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

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedItem != null)
            {
                SelectedTable = guna2ComboBox1.SelectedItem.ToString();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
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

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
    }
}