using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Kursach
{
    public partial class Form2 : Form
    {
        public static string ConnectionString { get; private set; }
        public static string SelectedTable { get; private set; }

        public Form2()
        {
            InitializeComponent();

            // Создаем TabControl
            TabControl tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;

            // Вкладка 1: General
            TabPage tabPageTables = new TabPage("General");
            tabPageTables.Controls.Add(comboBox1);
            tabPageTables.Controls.Add(TestConnectionButton);
            tabPageTables.Controls.Add(HostTextBox);
            tabPageTables.Controls.Add(PortTextBox);
            tabPageTables.Controls.Add(DatabaseTextBox);
            tabPageTables.Controls.Add(UserTextBox);
            tabPageTables.Controls.Add(PasswordTextBox);
            tabPageTables.Controls.Add(checkBox1);
            tabPageTables.Controls.Add(button1);

            // Добавляем метки на первую вкладку
            tabPageTables.Controls.Add(HostLabel);
            tabPageTables.Controls.Add(PortLabel);
            tabPageTables.Controls.Add(DatabaseLabel);
            tabPageTables.Controls.Add(UserLabel);
            tabPageTables.Controls.Add(PasswordLabel);

            // Вкладка 2: Advanced
            TabPage tabPageEmpty = new TabPage("Advanced");

            // Добавляем вкладки в TabControl
            tabControl.TabPages.Add(tabPageTables);
            tabControl.TabPages.Add(tabPageEmpty);

            // Добавляем TabControl на форму
            this.Controls.Add(tabControl);

            // Подписываемся на событие FormClosing
            this.FormClosing += new FormClosingEventHandler(Form2_FormClosing);

            // Подписываемся на событие SelectedIndexChanged для TabControl
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Получаем текущий TabControl
            TabControl tabControl = (TabControl)sender;

            // Проверяем, какая вкладка выбрана
            if (tabControl.SelectedIndex == 0) // Первая вкладка ("General")
            {
                // Показываем элементы первой вкладки
                comboBox1.Visible = true;
                TestConnectionButton.Visible = true;
                HostTextBox.Visible = true;
                PortTextBox.Visible = true;
                DatabaseTextBox.Visible = true;
                UserTextBox.Visible = true;
                PasswordTextBox.Visible = true;
                checkBox1.Visible = true;
                button1.Visible = true;

                // Также показываем метки
                HostLabel.Visible = true;
                PortLabel.Visible = true;
                DatabaseLabel.Visible = true;
                UserLabel.Visible = true;
                PasswordLabel.Visible = true;
            }
            else if (tabControl.SelectedIndex == 1) // Вторая вкладка ("Advanced")
            {
                // Скрываем элементы первой вкладки
                comboBox1.Visible = false;
                TestConnectionButton.Visible = false;
                HostTextBox.Visible = false;
                PortTextBox.Visible = false;
                DatabaseTextBox.Visible = false;
                UserTextBox.Visible = false;
                PasswordTextBox.Visible = false;
                checkBox1.Visible = false;
                button1.Visible = false;

                // Также скрываем метки
                HostLabel.Visible = false;
                PortLabel.Visible = false;
                DatabaseLabel.Visible = false;
                UserLabel.Visible = false;
                PasswordLabel.Visible = false;
                checkBox2.Visible = false;
            }
        }

        private void TestConnectionButton_Click(object sender, EventArgs e)
        {
            string host = HostTextBox.Text;
            string port = PortTextBox.Text;
            string database = DatabaseTextBox.Text;
            string user = UserTextBox.Text;
            string password = PasswordTextBox.Text;

            ConnectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password};";

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

        private void UpdateTableList(NpgsqlConnection conn)
        {
            string query;
            if (checkBox1.Checked)
            {
                query = @"
                    SELECT table_schema, table_name
                    FROM information_schema.tables
                    WHERE table_type = 'BASE TABLE' AND table_schema = 'public'
                    ORDER BY table_schema, table_name;";
            }
            else
            {
                query = @"
                    SELECT table_schema, table_name
                    FROM information_schema.tables
                    WHERE table_type = 'BASE TABLE'
                    ORDER BY table_schema, table_name;";
            }

            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                comboBox1.Items.Clear();
                while (reader.Read())
                {
                    string tableSchema = reader.GetString(0);
                    string tableName = reader.GetString(1);
                    comboBox1.Items.Add($"{tableSchema}.{tableName}");
                }
            }

            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
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
                MessageBox.Show("Выберите таблицу из списка.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tableName = SelectedTable;
            DataTable dataTable = Form1.Instance.GetTableData(tableName);
            Form1.Instance.LoadData(dataTable);
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
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

        private void UserTextBox_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.UserName = UserTextBox.Text;
            Properties.Settings.Default.Save();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Properties.Settings.Default.UserName = UserTextBox.Text;
                Properties.Settings.Default.DatabaseName = DatabaseTextBox.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.UserName = string.Empty;
                Properties.Settings.Default.DatabaseName = string.Empty;
                Properties.Settings.Default.Save();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            UserTextBox.Text = Properties.Settings.Default.UserName;
            DatabaseTextBox.Text = Properties.Settings.Default.DatabaseName;

            if (!string.IsNullOrEmpty(Properties.Settings.Default.UserName))
            {
                checkBox2.Checked = true;
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkBox2.Checked)
            {
                Properties.Settings.Default.UserName = UserTextBox.Text;
                Properties.Settings.Default.DatabaseName = DatabaseTextBox.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void HostLabel_Click(object sender, EventArgs e) { }
        private void PortLabel_Click(object sender, EventArgs e) { }
        private void DatabaseLabel_Click(object sender, EventArgs e) { }
        private void UserLabel_Click(object sender, EventArgs e) { }
    }
}