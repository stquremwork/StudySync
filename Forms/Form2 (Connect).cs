using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Kursach
{
    public partial class Form2 : Form
    {
        // Ð¡Ñ‚Ð°Ñ‚Ð¸Ñ‡ÐµÑÐºÐ°Ñ ÑÑ‚Ñ€Ð¾ÐºÐ° Ð¿Ð¾Ð´ÐºÐ»ÑŽÑ‡ÐµÐ½Ð¸Ñ Ðº Ð±Ð°Ð·Ðµ Ð´Ð°Ð½Ð½Ñ‹Ñ…
        public static string ConnectionString { get; private set; }

        // Ð¡Ñ‚Ð°Ñ‚Ð¸Ñ‡ÐµÑÐºÐ°Ñ Ð¿ÐµÑ€ÐµÐ¼ÐµÐ½Ð½Ð°Ñ Ð´Ð»Ñ Ñ…Ñ€Ð°Ð½ÐµÐ½Ð¸Ñ Ð¸Ð¼ÐµÐ½Ð¸ Ð²Ñ‹Ð±Ñ€Ð°Ð½Ð½Ð¾Ð¹ Ñ‚Ð°Ð±Ð»Ð¸Ñ†Ñ‹
        public static string SelectedTable { get; private set; }

        // Ð¤Ð»Ð°Ð³, ÑƒÐºÐ°Ð·Ñ‹Ð²Ð°ÑŽÑ‰Ð¸Ð¹, Ð¿Ð¾Ð´ÐºÐ»ÑŽÑ‡ÐµÐ½Ñ‹ Ð»Ð¸ Ð¼Ñ‹ Ðº Ð±Ð°Ð·Ðµ Ð´Ð°Ð½Ð½Ñ‹Ñ…
        private bool isConnected = false;

        private NpgsqlConnection connection; // Ð”Ð¾Ð±Ð°Ð²Ð»ÑÐµÐ¼ Ð¿Ð¾Ð»Ðµ Ð´Ð»Ñ Ñ…Ñ€Ð°Ð½ÐµÐ½Ð¸Ñ ÑÐ¾ÐµÐ´Ð¸Ð½ÐµÐ½Ð¸Ñ

        public Form2()
        {
            // Ð˜Ð½Ð¸Ñ†Ð¸Ð°Ð»Ð¸Ð·Ð°Ñ†Ð¸Ñ ÐºÐ¾Ð¼Ð¿Ð¾Ð½ÐµÐ½Ñ‚Ð¾Ð² Ñ„Ð¾Ñ€Ð¼Ñ‹
            InitializeComponent();

            // ÐŸÐ¾Ð´Ð¿Ð¸ÑÐºÐ° Ð½Ð° ÑÐ¾Ð±Ñ‹Ñ‚Ð¸Ðµ Ð·Ð°ÐºÑ€Ñ‹Ñ‚Ð¸Ñ Ñ„Ð¾Ñ€Ð¼Ñ‹
            this.FormClosing += new FormClosingEventHandler(Form2_FormClosing);
        }

        // ÐžÐ±Ñ€Ð°Ð±Ð¾Ñ‚Ñ‡Ð¸Ðº ÑÐ¾Ð±Ñ‹Ñ‚Ð¸Ñ ÐºÐ»Ð¸ÐºÐ° Ð½Ð° ÐºÐ½Ð¾Ð¿ÐºÑƒ "Test Connection"
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

            if (string.IsNullOrEmpty(Form2.ConnectionString))
            {
                MessageBox.Show("Ð¡Ñ‚Ñ€Ð¾ÐºÐ° Ð¿Ð¾Ð´ÐºÐ»ÑŽÑ‡ÐµÐ½Ð¸Ñ Ð½Ðµ Ð¼Ð¾Ð¶ÐµÑ‚ Ð±Ñ‹Ñ‚ÑŒ Ð¿ÑƒÑÑ‚Ð¾Ð¹. Ð—Ð°Ð¿Ð¾Ð»Ð½Ð¸Ñ‚Ðµ Ð¿Ð°Ñ€Ð°Ð¼ÐµÑ‚Ñ€Ñ‹ Ð¿Ð¾Ð´ÐºÐ»ÑŽÑ‡ÐµÐ½Ð¸Ñ.", "ÐžÑˆÐ¸Ð±ÐºÐ°", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                connection = new NpgsqlConnection(ConnectionString); // Ð˜Ð½Ð¸Ñ†Ð¸Ð°Ð»Ð¸Ð·Ð¸Ñ€ÑƒÐµÐ¼ ÑÐ¾ÐµÐ´Ð¸Ð½ÐµÐ½Ð¸Ðµ
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
                    comboBox1.Items.Clear(); // ÐžÑ‡Ð¸Ñ‰Ð°ÐµÐ¼ ÑÐ¿Ð¸ÑÐ¾Ðº Ñ‚Ð°Ð±Ð»Ð¸Ñ† Ð¿Ñ€Ð¸ Ð¾Ñ‚ÐºÐ»ÑŽÑ‡ÐµÐ½Ð¸Ð¸
                    CloseDataTabsInForm1(); // Ð—Ð°ÐºÑ€Ñ‹Ð²Ð°ÐµÐ¼ Ð²ÐºÐ»Ð°Ð´ÐºÐ¸ Ñ Ð´Ð°Ð½Ð½Ñ‹Ð¼Ð¸ Ð² Form1
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

        // ÐœÐµÑ‚Ð¾Ð´ Ð´Ð»Ñ Ð¾Ð±Ð½Ð¾Ð²Ð»ÐµÐ½Ð¸Ñ ÑÑ‚Ñ€Ð¾ÐºÐ¸ Ð¿Ð¾Ð´ÐºÐ»ÑŽÑ‡ÐµÐ½Ð¸Ñ Ð² Ð·Ð°Ð²Ð¸ÑÐ¸Ð¼Ð¾ÑÑ‚Ð¸ Ð¾Ñ‚ Ð²Ñ‹Ð±Ñ€Ð°Ð½Ð½Ð¾Ð³Ð¾ ÑÐ¿Ð¾ÑÐ¾Ð±Ð° Ð¿Ð¾Ð´ÐºÐ»ÑŽÑ‡ÐµÐ½Ð¸Ñ
        private void UpdateConnectionString()
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();

            if (UseUrlCheckBox.Checked)
            {
                // ÐžÐ±Ñ€Ð°Ð±Ð°Ñ‚Ñ‹Ð²Ð°ÐµÐ¼ URL
                try
                {
                    // Ð Ð°Ð·Ð±Ð¸Ñ€Ð°ÐµÐ¼ URL Ð¸ ÑƒÑÑ‚Ð°Ð½Ð°Ð²Ð»Ð¸Ð²Ð°ÐµÐ¼ Ð¿Ð°Ñ€Ð°Ð¼ÐµÑ‚Ñ€Ñ‹
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
                    MessageBox.Show($"ÐÐµÐ²ÐµÑ€Ð½Ñ‹Ð¹ Ñ„Ð¾Ñ€Ð¼Ð°Ñ‚ URL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"ÐžÑˆÐ¸Ð±ÐºÐ° Ð¿Ñ€Ð¸ Ð¾Ð±Ñ€Ð°Ð±Ð¾Ñ‚ÐºÐµ URL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                // Ð˜ÑÐ¿Ð¾Ð»ÑŒÐ·ÑƒÐµÐ¼ Ð¾Ñ‚Ð´ÐµÐ»ÑŒÐ½Ñ‹Ðµ Ð¿Ð¾Ð»Ñ Ð´Ð»Ñ Ð¿Ð¾Ð´ÐºÐ»ÑŽÑ‡ÐµÐ½Ð¸Ñ
                builder.Host = HostTextBox.Text;
                builder.Port = int.Parse(PortTextBox.Text);
                builder.Database = DatabaseTextBox.Text;
                builder.Username = UserTextBox.Text;
                builder.Password = PasswordTextBox.Text;
                builder.SslMode = SslMode.Require;
                builder.TrustServerCertificate = true;
            }

            ConnectionString = builder.ConnectionString;
            //MessageBox.Show($"Connection string: {ConnectionString}"); // Ð”Ð»Ñ Ð¾Ñ‚Ð»Ð°Ð´ÐºÐ¸ - Ð¼Ð¾Ð¶Ð½Ð¾ Ñ€Ð°ÑÐºÐ¾Ð¼Ð¼ÐµÐ½Ñ‚Ð¸Ñ€Ð¾Ð²Ð°Ñ‚ÑŒ Ð´Ð»Ñ Ð¿Ñ€Ð¾Ð²ÐµÑ€ÐºÐ¸
        }

        // ÐœÐµÑ‚Ð¾Ð´ Ð´Ð»Ñ Ð¾Ð±Ð½Ð¾Ð²Ð»ÐµÐ½Ð¸Ñ ÑÐ¿Ð¸ÑÐºÐ° Ñ‚Ð°Ð±Ð»Ð¸Ñ† Ð² Ð±Ð°Ð·Ðµ Ð´Ð°Ð½Ð½Ñ‹Ñ…
        private void UpdateTableList(NpgsqlConnection conn)
        {
            string query;

            // Ð•ÑÐ»Ð¸ Ð²Ñ‹Ð±Ñ€Ð°Ð½Ð¾ Ñ‚Ð¾Ð»ÑŒÐºÐ¾ Ð¿ÑƒÐ±Ð»Ð¸Ñ‡Ð½Ñ‹Ðµ ÑÑ…ÐµÐ¼Ñ‹
            if (checkBox1.Checked)
            {
                // Ð¤Ð¾Ñ€Ð¼Ð¸Ñ€ÑƒÐµÐ¼ Ð·Ð°Ð¿Ñ€Ð¾Ñ Ð´Ð»Ñ Ð¿ÑƒÐ±Ð»Ð¸Ñ‡Ð½Ñ‹Ñ… Ñ‚Ð°Ð±Ð»Ð¸Ñ†
                query = @"
                    SELECT table_schema, table_name
                    FROM information_schema.tables
                    WHERE table_type = 'BASE TABLE' AND table_schema = 'public'
                    ORDER BY table_schema, table_name;";
            }
            else
            {
                // Ð¤Ð¾Ñ€Ð¼Ð¸Ñ€ÑƒÐµÐ¼ Ð·Ð°Ð¿Ñ€Ð¾Ñ Ð´Ð»Ñ Ð²ÑÐµÑ… Ñ‚Ð°Ð±Ð»Ð¸Ñ†
                query = @"
                    SELECT table_schema, table_name
                    FROM information_schema.tables
                    WHERE table_type = 'BASE TABLE'
                    ORDER BY table_schema, table_name;";
            }

            // Ð¡Ð¾Ð·Ð´Ð°ÐµÐ¼ ÐºÐ¾Ð¼Ð°Ð½Ð´Ñƒ SQL
            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                // ÐžÑ‡Ð¸Ñ‰Ð°ÐµÐ¼ ÑÐ¿Ð¸ÑÐ¾Ðº Ñ‚Ð°Ð±Ð»Ð¸Ñ†
                comboBox1.Items.Clear();

                // Ð§Ð¸Ñ‚Ð°ÐµÐ¼ Ñ€ÐµÐ·ÑƒÐ»ÑŒÑ‚Ð°Ñ‚Ñ‹ Ð·Ð°Ð¿Ñ€Ð¾ÑÐ°
                while (reader.Read())
                {
                    string tableSchema = reader.GetString(0);
                    string tableName = reader.GetString(1);

                    // Ð”Ð¾Ð±Ð°Ð²Ð»ÑÐµÐ¼ Ñ‚Ð°Ð±Ð»Ð¸Ñ†Ñ‹ Ð² ÑÐ¿Ð¸ÑÐ¾Ðº
                    comboBox1.Items.Add($"{tableSchema}.{tableName}");
                }
            }

            // Ð’Ñ‹Ð±Ð¸Ñ€Ð°ÐµÐ¼ Ð¿ÐµÑ€Ð²ÑƒÑŽ Ñ‚Ð°Ð±Ð»Ð¸Ñ†Ñƒ Ð¿Ð¾ ÑƒÐ¼Ð¾Ð»Ñ‡Ð°Ð½Ð¸ÑŽ
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
        }

        // ÐžÐ±Ñ€Ð°Ð±Ð¾Ñ‚Ñ‡Ð¸Ðº ÑÐ¾Ð±Ñ‹Ñ‚Ð¸Ñ Ð¸Ð·Ð¼ÐµÐ½ÐµÐ½Ð¸Ñ Ð²Ñ‹Ð±Ñ€Ð°Ð½Ð½Ð¾Ð¹ Ñ‚Ð°Ð±Ð»Ð¸Ñ†Ñ‹ Ð² ÑÐ¿Ð¸ÑÐºÐµ
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ð•ÑÐ»Ð¸ Ð²Ñ‹Ð±Ñ€Ð°Ð½Ð° Ñ‚Ð°Ð±Ð»Ð¸Ñ†Ð°
            if (comboBox1.SelectedItem != null)
            {
                // Ð¡Ð¾Ñ…Ñ€Ð°Ð½ÑÐµÐ¼ Ð¸Ð¼Ñ Ð²Ñ‹Ð±Ñ€Ð°Ð½Ð½Ð¾Ð¹ Ñ‚Ð°Ð±Ð»Ð¸Ñ†Ñ‹
                SelectedTable = comboBox1.SelectedItem.ToString();
            }
        }

        // ÐžÐ±Ñ€Ð°Ð±Ð¾Ñ‚Ñ‡Ð¸Ðº ÑÐ¾Ð±Ñ‹Ñ‚Ð¸Ñ ÐºÐ»Ð¸ÐºÐ° Ð½Ð° ÐºÐ½Ð¾Ð¿ÐºÑƒ "Send data"
        private void button1_Click(object sender, EventArgs e)
        {
            // Ð•ÑÐ»Ð¸ Ð½Ðµ Ð²Ñ‹Ð±Ñ€Ð°Ð½Ð° Ñ‚Ð°Ð±Ð»Ð¸Ñ†Ð°
            if (string.IsNullOrEmpty(SelectedTable))
            {
                // Ð’Ñ‹Ð²Ð¾Ð´Ð¸Ð¼ Ð¿Ñ€ÐµÐ´ÑƒÐ¿Ñ€ÐµÐ¶Ð´ÐµÐ½Ð¸Ðµ
                MessageBox.Show("Ð’Ñ‹Ð±ÐµÑ€Ð¸Ñ‚Ðµ Ñ‚Ð°Ð±Ð»Ð¸Ñ†Ñƒ Ð¸Ð· ÑÐ¿Ð¸ÑÐºÐ°.", "ÐžÑˆÐ¸Ð±ÐºÐ°", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ÐŸÐ¾Ð»ÑƒÑ‡Ð°ÐµÐ¼ Ð¸Ð¼Ñ Ð²Ñ‹Ð±Ñ€Ð°Ð½Ð½Ð¾Ð¹ Ñ‚Ð°Ð±Ð»Ð¸Ñ†Ñ‹
            string tableName = SelectedTable;

            // ÐŸÐ¾Ð»ÑƒÑ‡Ð°ÐµÐ¼ Ð´Ð°Ð½Ð½Ñ‹Ðµ Ñ‚Ð°Ð±Ð»Ð¸Ñ†Ñ‹
            DataTable dataTable = Form1.Instance.GetTableData(tableName);

            // Ð—Ð°Ð³Ñ€ÑƒÐ¶Ð°ÐµÐ¼ Ð´Ð°Ð½Ð½Ñ‹Ðµ Ð² Ð½Ð¾Ð²ÑƒÑŽ Ð²ÐºÐ»Ð°Ð´ÐºÑƒ
            Form1.Instance.LoadData(dataTable, tableName);
        }

        // ÐžÐ±Ñ€Ð°Ð±Ð¾Ñ‚Ñ‡Ð¸Ðº ÑÐ¾Ð±Ñ‹Ñ‚Ð¸Ñ Ð¸Ð·Ð¼ÐµÐ½ÐµÐ½Ð¸Ñ ÑÐ¾ÑÑ‚Ð¾ÑÐ½Ð¸Ñ Ñ‡ÐµÐºÐ±Ð¾ÐºÑÐ° "Only Public schemes"
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            UpdateTableListBasedOnConnection();
        }

        // ÐžÐ±Ð½Ð¾Ð²Ð»ÐµÐ½Ð¸Ðµ ÑÐ¿Ð¸ÑÐºÐ° Ñ‚Ð°Ð±Ð»Ð¸Ñ† Ð½Ð° Ð¾ÑÐ½Ð¾Ð²Ðµ Ñ‚ÐµÐºÑƒÑ‰ÐµÐ³Ð¾ ÑÐ¾ÐµÐ´Ð¸Ð½ÐµÐ½Ð¸Ñ
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
                    MessageBox.Show($"ÐžÑˆÐ¸Ð±ÐºÐ° Ð¿Ñ€Ð¸ Ð¾Ð±Ð½Ð¾Ð²Ð»ÐµÐ½Ð¸Ð¸ ÑÐ¿Ð¸ÑÐºÐ° Ñ‚Ð°Ð±Ð»Ð¸Ñ†: {ex.Message}", "ÐžÑˆÐ¸Ð±ÐºÐ°", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (!isConnected)
            {
                comboBox1.Items.Clear();
            }
        }

        // ÐžÐ±Ñ€Ð°Ð±Ð¾Ñ‚Ñ‡Ð¸Ðº ÑÐ¾Ð±Ñ‹Ñ‚Ð¸Ñ Ð¸Ð·Ð¼ÐµÐ½ÐµÐ½Ð¸Ñ Ñ‚ÐµÐºÑÑ‚Ð° Ð² Ð¿Ð¾Ð»Ðµ "User"
        private void UserTextBox_TextChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        // ÐžÐ±Ñ€Ð°Ð±Ð¾Ñ‚Ñ‡Ð¸Ðº ÑÐ¾Ð±Ñ‹Ñ‚Ð¸Ñ Ð¸Ð·Ð¼ÐµÐ½ÐµÐ½Ð¸Ñ ÑÐ¾ÑÑ‚Ð¾ÑÐ½Ð¸Ñ Ñ‡ÐµÐºÐ±Ð¾ÐºÑÐ° "Remember settings"
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SaveSettings();
        }

        // ÐœÐµÑ‚Ð¾Ð´ ÑÐ¾Ñ…Ñ€Ð°Ð½ÐµÐ½Ð¸Ñ Ð½Ð°ÑÑ‚Ñ€Ð¾ÐµÐº
        private void SaveSettings()
        {
            if (checkBox2.Checked)
            {
                // Ð¡Ð¾Ñ…Ñ€Ð°Ð½ÑÐµÐ¼ Ð¸Ð¼Ñ Ð¿Ð¾Ð»ÑŒÐ·Ð¾Ð²Ð°Ñ‚ÐµÐ»Ñ Ð¸ Ð±Ð°Ð·Ñ‹ Ð´Ð°Ð½Ð½Ñ‹Ñ… Ð² Ð½Ð°ÑÑ‚Ñ€Ð¾Ð¹ÐºÐ°Ñ…
                Properties.Settings.Default.UserName = UserTextBox.Text;
                Properties.Settings.Default.DatabaseName = DatabaseTextBox.Text;
            }
            else
            {
                // ÐžÑ‡Ð¸Ñ‰Ð°ÐµÐ¼ Ð½Ð°ÑÑ‚Ñ€Ð¾Ð¹ÐºÐ¸
                Properties.Settings.Default.UserName = string.Empty;
                Properties.Settings.Default.DatabaseName = string.Empty;
            }
            Properties.Settings.Default.Save();
        }

        // ÐžÐ±Ñ€Ð°Ð±Ð¾Ñ‚Ñ‡Ð¸Ðº ÑÐ¾Ð±Ñ‹Ñ‚Ð¸Ñ Ð·Ð°Ð³Ñ€ÑƒÐ·ÐºÐ¸ Ñ„Ð¾Ñ€Ð¼Ñ‹
        private void Form2_Load(object sender, EventArgs e)
        {
            // ÐÐµÐ±Ð¾Ð»ÑŒÑˆÐ¸Ðµ Ð½Ð°ÑÑ‚Ñ€Ð¾ÐºÐ¸ Ð¾ÐºÐ½Ð°
            this.FormBorderStyle = FormBorderStyle.FixedDialog;  // Ð—Ð°Ð¿Ñ€ÐµÑ‰Ð°ÐµÐ¼ Ð¸Ð·Ð¼ÐµÐ½ÐµÐ½Ð¸Ðµ Ñ€Ð°Ð·Ð¼ÐµÑ€Ð° Ð¾ÐºÐ½Ð°
            this.MaximizeBox = false;  // Ð—Ð°Ð¿Ñ€ÐµÑ‰Ð°ÐµÐ¼ Ð¼Ð°ÐºÑÐ¸Ð¼Ð¸Ð·Ð°Ñ†Ð¸ÑŽ Ð¾ÐºÐ½Ð°
            this.MinimizeBox = false;  // Ð—Ð°Ð¿Ñ€ÐµÑ‰Ð°ÐµÐ¼ Ð¼Ð¸Ð½Ð¸Ð¼Ð¸Ð·Ð°Ñ†Ð¸ÑŽ Ð¾ÐºÐ½Ð°
            this.StartPosition = FormStartPosition.CenterScreen;

            // Ð—Ð°Ð³Ñ€ÑƒÐ¶Ð°ÐµÐ¼ ÑÐ¾Ñ…Ñ€Ð°Ð½ÐµÐ½Ð½Ñ‹Ðµ Ð½Ð°ÑÑ‚Ñ€Ð¾Ð¹ÐºÐ¸
            UserTextBox.Text = Properties.Settings.Default.UserName;
            DatabaseTextBox.Text = Properties.Settings.Default.DatabaseName;

            // Ð•ÑÐ»Ð¸ ÑÐ¾Ñ…Ñ€Ð°Ð½ÐµÐ½Ð¾ Ð¸Ð¼Ñ Ð¿Ð¾Ð»ÑŒÐ·Ð¾Ð²Ð°Ñ‚ÐµÐ»Ñ
            if (!string.IsNullOrEmpty(Properties.Settings.Default.UserName))
            {
                // Ð’ÐºÐ»ÑŽÑ‡Ð°ÐµÐ¼ Ñ‡ÐµÐºÐ±Ð¾ÐºÑ "Remember settings"
                checkBox2.Checked = true;
            }

            // Ð•ÑÐ»Ð¸ ÑÐ¾Ñ…Ñ€Ð°Ð½ÐµÐ½ URL
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Url))
            {
                // Ð—Ð°Ð³Ñ€ÑƒÐ¶Ð°ÐµÐ¼ URL Ð² Ñ‚ÐµÐºÑÑ‚Ð¾Ð²Ð¾Ðµ Ð¿Ð¾Ð»Ðµ
                UrlTextBox.Text = Properties.Settings.Default.Url;
                // Ð’ÐºÐ»ÑŽÑ‡Ð°ÐµÐ¼ Ñ‡ÐµÐºÐ±Ð¾ÐºÑ "Remember URL"
                RememberUrlCheckBox.Checked = true;
            }

            // Ð£ÑÑ‚Ð°Ð½Ð°Ð²Ð»Ð¸Ð²Ð°ÐµÐ¼ Ð½Ð°Ñ‡Ð°Ð»ÑŒÐ½Ð¾Ðµ ÑÐ¾ÑÑ‚Ð¾ÑÐ½Ð¸Ðµ ÐºÐ½Ð¾Ð¿ÐºÐ¸
            TestConnectionButton.Text = "Connect";
        }

        // ÐžÐ±Ñ€Ð°Ð±Ð¾Ñ‚Ñ‡Ð¸Ðº ÑÐ¾Ð±Ñ‹Ñ‚Ð¸Ñ Ð·Ð°ÐºÑ€Ñ‹Ñ‚Ð¸Ñ Ñ„Ð¾Ñ€Ð¼Ñ‹
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Ð•ÑÐ»Ð¸ Ð²ÐºÐ»ÑŽÑ‡ÐµÐ½ Ñ‡ÐµÐºÐ±Ð¾ÐºÑ "Remember settings"
            if (checkBox2.Checked)
            {
                // Ð¡Ð¾Ñ…Ñ€Ð°Ð½ÑÐµÐ¼ Ð¸Ð¼Ñ Ð¿Ð¾Ð»ÑŒÐ·Ð¾Ð²Ð°Ñ‚ÐµÐ»Ñ Ð¸ Ð±Ð°Ð·Ñ‹ Ð´Ð°Ð½Ð½Ñ‹Ñ… Ð² Ð½Ð°ÑÑ‚Ñ€Ð¾Ð¹ÐºÐ°Ñ…
                Properties.Settings.Default.UserName = UserTextBox.Text;
                Properties.Settings.Default.DatabaseName = DatabaseTextBox.Text;
            }
            else
            {
                // ÐžÑ‡Ð¸Ñ‰Ð°ÐµÐ¼ Ð½Ð°ÑÑ‚Ñ€Ð¾Ð¹ÐºÐ¸
                Properties.Settings.Default.UserName = string.Empty;
                Properties.Settings.Default.DatabaseName = string.Empty;
            }

            // Ð•ÑÐ»Ð¸ Ð²ÐºÐ»ÑŽÑ‡ÐµÐ½ Ñ‡ÐµÐºÐ±Ð¾ÐºÑ "Remember URL"
            if (RememberUrlCheckBox.Checked)
            {
                // Ð¡Ð¾Ñ…Ñ€Ð°Ð½ÑÐµÐ¼ URL Ð² Ð½Ð°ÑÑ‚Ñ€Ð¾Ð¹ÐºÐ°Ñ…
                Properties.Settings.Default.Url = UrlTextBox.Text;
            }
            else
            {
                // ÐžÑ‡Ð¸Ñ‰Ð°ÐµÐ¼ URL Ð² Ð½Ð°ÑÑ‚Ñ€Ð¾Ð¹ÐºÐ°Ñ…
                Properties.Settings.Default.Url = string.Empty;
            }
            Properties.Settings.Default.Save();

            // Ð—Ð°ÐºÑ€Ñ‹Ð²Ð°ÐµÐ¼ ÑÐ¾ÐµÐ´Ð¸Ð½ÐµÐ½Ð¸Ðµ Ð¿Ñ€Ð¸ Ð·Ð°ÐºÑ€Ñ‹Ñ‚Ð¸Ð¸ Ñ„Ð¾Ñ€Ð¼Ñ‹
            DisconnectFromDatabase();
        }

        // ÐžÐ±Ñ€Ð°Ð±Ð¾Ñ‚Ñ‡Ð¸Ðº ÑÐ¾Ð±Ñ‹Ñ‚Ð¸Ñ Ð¸Ð·Ð¼ÐµÐ½ÐµÐ½Ð¸Ñ ÑÐ¾ÑÑ‚Ð¾ÑÐ½Ð¸Ñ Ñ‡ÐµÐºÐ±Ð¾ÐºÑÐ° "Use URL"
        private void UseUrlCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUIElements();
        }

        // ÐœÐµÑ‚Ð¾Ð´ Ð´Ð»Ñ Ð¾Ð±Ð½Ð¾Ð²Ð»ÐµÐ½Ð¸Ñ Ð´Ð¾ÑÑ‚ÑƒÐ¿Ð½Ð¾ÑÑ‚Ð¸ Ð¿Ð¾Ð»ÐµÐ¹ Ð²Ð²Ð¾Ð´Ð° Ð² Ð·Ð°Ð²Ð¸ÑÐ¸Ð¼Ð¾ÑÑ‚Ð¸ Ð¾Ñ‚ ÑÐ¾ÑÑ‚Ð¾ÑÐ½Ð¸Ñ Ñ‡ÐµÐºÐ±Ð¾ÐºÑÐ° "Use URL"
        private void UpdateUIElements()
        {
            // Ð•ÑÐ»Ð¸ Ð²ÐºÐ»ÑŽÑ‡ÐµÐ½ Ñ‡ÐµÐºÐ±Ð¾ÐºÑ "Use URL"
            if (UseUrlCheckBox.Checked)
            {
                // ÐžÑ‚ÐºÐ»ÑŽÑ‡Ð°ÐµÐ¼ Ð¿Ð¾Ð»Ñ Ð´Ð»Ñ Ð²Ð²Ð¾Ð´Ð° Ð´Ð°Ð½Ð½Ñ‹Ñ…
                HostTextBox.Enabled = false;
                PortTextBox.Enabled = false;
                DatabaseTextBox.Enabled = false;
                UserTextBox.Enabled = false;
                PasswordTextBox.Enabled = false;

                // Ð’ÐºÐ»ÑŽÑ‡Ð°ÐµÐ¼ Ð¿Ð¾Ð»Ðµ Ð´Ð»Ñ Ð²Ð²Ð¾Ð´Ð° URL
                UrlTextBox.Enabled = true;
            }
            else
            {
                // Ð’ÐºÐ»ÑŽÑ‡Ð°ÐµÐ¼ Ð¿Ð¾Ð»Ñ Ð´Ð»Ñ Ð²Ð²Ð¾Ð´Ð° Ð´Ð°Ð½Ð½Ñ‹Ñ…
                HostTextBox.Enabled = true;
                PortTextBox.Enabled = true;
                DatabaseTextBox.Enabled = true;
                UserTextBox.Enabled = true;
                PasswordTextBox.Enabled = true;

                // ÐžÑ‚ÐºÐ»ÑŽÑ‡Ð°ÐµÐ¼ Ð¿Ð¾Ð»Ðµ Ð´Ð»Ñ Ð²Ð²Ð¾Ð´Ð° URL
                UrlTextBox.Enabled = false;
            }
        }

        // ÐžÐ±Ñ€Ð°Ð±Ð¾Ñ‚Ñ‡Ð¸Ðº ÑÐ¾Ð±Ñ‹Ñ‚Ð¸Ñ Ð¸Ð·Ð¼ÐµÐ½ÐµÐ½Ð¸Ñ ÑÐ¾ÑÑ‚Ð¾ÑÐ½Ð¸Ñ Ñ‡ÐµÐºÐ±Ð¾ÐºÑÐ° "Remember URL"
        private void RememberUrlCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // Ð•ÑÐ»Ð¸ Ð²ÐºÐ»ÑŽÑ‡ÐµÐ½ Ñ‡ÐµÐºÐ±Ð¾ÐºÑ "Remember URL"
            if (RememberUrlCheckBox.Checked)
            {
                // Ð¡Ð¾Ñ…Ñ€Ð°Ð½ÑÐµÐ¼ URL Ð² Ð½Ð°ÑÑ‚Ñ€Ð¾Ð¹ÐºÐ°Ñ…
                Properties.Settings.Default.Url = UrlTextBox.Text;
            }
            else
            {
                // ÐžÑ‡Ð¸Ñ‰Ð°ÐµÐ¼ URL Ð² Ð½Ð°ÑÑ‚Ñ€Ð¾Ð¹ÐºÐ°Ñ…
                Properties.Settings.Default.Url = string.Empty;
            }
            Properties.Settings.Default.Save();
        }

        private void UrlTextBox_TextChanged(object sender, EventArgs e)
        {
            // ÐžÐ±Ð½Ð¾Ð²Ð»ÑÐµÐ¼ ÑÑ‚Ñ€Ð¾ÐºÑƒ Ð¿Ð¾Ð´ÐºÐ»ÑŽÑ‡ÐµÐ½Ð¸Ñ Ð¿Ñ€Ð¸ Ð¸Ð·Ð¼ÐµÐ½ÐµÐ½Ð¸Ð¸ Ñ‚ÐµÐºÑÑ‚Ð° Ð² UrlTextBox
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