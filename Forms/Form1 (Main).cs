using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Npgsql;
using Kursach.Helpers;
using Kursach.Forms;
using System.IO;
using System.Collections.Generic;

namespace Kursach
{
    public partial class Form1 : Form
    {
        public static Form1 Instance { get; private set; }
        private Image closeImage;
        private TabPage currentDataTab = null;
        private ListView listViewTables = new ListView();

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            Instance = this;

            
            
            closeImage = Properties.Resources.close ?? GenerateDefaultCloseImage();

            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.DrawItem += TabControl1_DrawItem;
            tabControl1.MouseClick += TabControl1_MouseClick;
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            tabControl1.MouseDown += TabControl1_MouseDown;

            // Настройка вкладки main
            SetupMainTab();

            // Инициализация ListView для таблиц
            InitializeTablesListView();

            // Инициализация статуса подключения
            UpdateConnectionStatus();
        }

        private void InitializeTablesListView()
        {
            listViewTables.Dock = DockStyle.Fill;
            listViewTables.View = View.Details;
            listViewTables.FullRowSelect = true;
            listViewTables.GridLines = true;
            listViewTables.MultiSelect = false;

            // Установка шрифта для ListView 
            listViewTables.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            // Оставляем только одну колонку и увеличиваем шрифт заголовка колонки
            listViewTables.Columns.Add("Название таблицы", 300).ListView.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            listViewTables.DoubleClick += ListViewTables_DoubleClick;
            listViewTables.ShowGroups = true;

            // Добавляем контекстное меню
            listViewTables.ContextMenuStrip = CreateTableContextMenu();

            splitContainer1.Panel1.Controls.Add(listViewTables);
        }

        public void LoadTableList()
        {
            listViewTables.Items.Clear();
            listViewTables.Groups.Clear();

            if (string.IsNullOrEmpty(Form2.ConnectionString))
            {
                MessageBox.Show("Нет подключения к базе данных", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(Form2.ConnectionString))
                {
                    conn.Open();

                    var publicGroup = new ListViewGroup("public", "");
                    listViewTables.Groups.Add(publicGroup);

                    using (var cmd = new NpgsqlCommand(
                        "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string tableName = reader.GetString(0);
                                var item = new ListViewItem(tableName);
                                item.Group = publicGroup;
                                listViewTables.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка таблиц: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private ContextMenuStrip CreateTableContextMenu()
        {
            var menu = new ContextMenuStrip();

            var addTableItem = new ToolStripMenuItem("Создать таблицу");
            addTableItem.Click += (sender, e) => CreateTableDialog();

            var renameTableItem = new ToolStripMenuItem("Переименовать таблицу");
            renameTableItem.Click += (sender, e) => RenameSelectedTable();

            var deleteTableItem = new ToolStripMenuItem("Удалить таблицу");
            deleteTableItem.Click += (sender, e) => DeleteSelectedTable();

            menu.Items.Add(addTableItem);
            menu.Items.Add(renameTableItem);
            menu.Items.Add(deleteTableItem);

            return menu;
        }

        private void CreateTableDialog()
        {
            string input = Prompt.ShowDialog("Введите название новой таблицы:", "Создание таблицы");
            if (!string.IsNullOrWhiteSpace(input))
            {
                try
                {
                    using (var conn = new NpgsqlConnection(Form2.ConnectionString))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand($"CREATE TABLE public.\"{input}\" (id serial PRIMARY KEY);", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadTableList(); // Обновляем список таблиц
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при создании таблицы: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void RenameSelectedTable()
        {
            if (listViewTables.SelectedItems.Count == 0)
                return;

            string oldName = listViewTables.SelectedItems[0].Text;
            string newName = Prompt.ShowDialog($"Переименовать '{oldName}' в:", "Переименование таблицы");

            if (!string.IsNullOrWhiteSpace(newName) && newName != oldName)
            {
                try
                {
                    using (var conn = new NpgsqlConnection(Form2.ConnectionString))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand($"ALTER TABLE public.\"{oldName}\" RENAME TO \"{newName}\";", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadTableList(); // Обновляем список
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при переименовании таблицы: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteSelectedTable()
        {
            if (listViewTables.SelectedItems.Count == 0)
                return;

            string tableName = listViewTables.SelectedItems[0].Text;
            var result = MessageBox.Show($"Вы уверены, что хотите удалить таблицу '{tableName}'?", "Подтверждение удаления",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (var conn = new NpgsqlConnection(Form2.ConnectionString))
                    {
                        conn.Open();
                        using (var cmd = new NpgsqlCommand($"DROP TABLE public.\"{tableName}\" CASCADE;", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    LoadTableList(); // Обновляем список
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении таблицы: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 300,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };

                Label textLabel = new Label() { Left = 20, Top = 20, Text = text };
                TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 240 };
                Button confirmation = new Button() { Text = "OK", Left = 200, Width = 60, Top = 80, DialogResult = DialogResult.OK };

                prompt.Controls.Add(textBox);
                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(confirmation);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
            }
        }

        private void ListViewTables_DoubleClick(object sender, EventArgs e)
        {
            if (listViewTables.SelectedItems.Count > 0 && !string.IsNullOrEmpty(Form2.ConnectionString))
            {
                var selectedItem = listViewTables.SelectedItems[0];
                string selectedTable = selectedItem.Text;
                LoadTableData($"public.{selectedTable}");
            }
        }

        private void LoadTableData(string tableName)
        {
            try
            {
                DataTable dataTable = DatabaseHelper.GetTableData(tableName, Form2.ConnectionString);
                if (dataTable != null)
                {
                    LoadData(dataTable, tableName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке таблицы {tableName}: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateConnectionStatus()
        {
            // Удаляем текущий ToolStripTextBox, если он есть
            if (toolStrip1.Items.Contains(toolStripTextBox1))
            {
                toolStrip1.Items.Remove(toolStripTextBox1);
            }

            // Устанавливаем текст и стиль
            if (string.IsNullOrEmpty(Form2.ConnectionString))
            {
                toolStripTextBox1.Text = "Не подключена";
                toolStripTextBox1.Font = new Font(toolStrip1.Font, FontStyle.Regular);
            }
            else
            {
                try
                {
                    var builder = new NpgsqlConnectionStringBuilder(Form2.ConnectionString);
                    toolStripTextBox1.Text = builder.Database;
                    toolStripTextBox1.Font = new Font(toolStrip1.Font, FontStyle.Bold);

                    // Загружаем список таблиц при успешном подключении
                    LoadTableList();
                }
                catch
                {
                    toolStripTextBox1.Text = "Ошибка подключения";
                    toolStripTextBox1.ForeColor = Color.Red;
                    toolStripTextBox1.Font = new Font(toolStrip1.Font, FontStyle.Regular);
                }
            }

            // Добавляем TextBox на ToolStrip
            toolStrip1.Items.Add(toolStripTextBox1);

            // Обработчик для предотвращения фокуса
            toolStripTextBox1.GotFocus += (sender, e) => { toolStrip1.Focus(); };
        }

        private void SetupMainTab()
        {
            if (tabControl1.TabPages.Count > 0)
            {
                TabPage mainTab = tabControl1.TabPages[0];
                mainTab.Text = "Main";
                mainTab.Controls.Clear();

                Label welcomeLabel = new Label
                {
                    AutoSize = true,
                    TextAlign = ContentAlignment.TopLeft,
                    Font = new Font("Segoe UI", 12),
                    BackColor = SystemColors.ControlLightLight,
                    Text = "Добро пожаловать в StudySync\n\n" +
                           "Версия программы: 1.0\n" +
                           "Более подробную информацию о программе\n" +
                           "вы можете найти в документации."
                };

                mainTab.Resize += (s, e) =>
                {
                    welcomeLabel.Location = new Point(
                        (mainTab.Width - welcomeLabel.Width) / 2,
                        (mainTab.Height - welcomeLabel.Height) / 2
                    );
                };

                welcomeLabel.Location = new Point(
                    (mainTab.Width - welcomeLabel.Width) / 2,
                    (mainTab.Height - welcomeLabel.Height) / 2
                );

                mainTab.Controls.Add(welcomeLabel);
            }
        }

        private Image GenerateDefaultCloseImage()
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawLine(Pens.Black, 0, 0, 15, 15);
                g.DrawLine(Pens.Black, 15, 0, 0, 15);
            }
            return bmp;
        }

        private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabPage = tabControl1.TabPages[e.Index];
            var tabRect = tabControl1.GetTabRect(e.Index);
            bool isSelected = tabControl1.SelectedIndex == e.Index;

            using (var bgBrush = new SolidBrush(isSelected ? SystemColors.ControlLightLight : SystemColors.Control))
            {
                e.Graphics.FillRectangle(bgBrush, tabRect);
            }

            TextRenderer.DrawText(
                e.Graphics,
                tabPage.Text,
                tabPage.Font,
                new Point(tabRect.Left + 4, tabRect.Top + (tabRect.Height - tabPage.Font.Height) / 2),
                tabPage.ForeColor);

            if (closeImage != null && e.Index > 0)
            {
                int imageSize = 12;
                int imageX = tabRect.Right - imageSize - 4;
                int imageY = tabRect.Top + (tabRect.Height - imageSize) / 2;
                e.Graphics.DrawImage(closeImage, imageX, imageY, imageSize, imageSize);
            }
        }

        private void TabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                for (int i = 0; i < tabControl1.TabPages.Count; i++)
                {
                    if (tabControl1.GetTabRect(i).Contains(e.Location) && i > 0)
                    {
                        CloseTab(i);
                        break;
                    }
                }
            }
        }

        private void TabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                var tabRect = tabControl1.GetTabRect(i);
                if (i > 0 && tabRect.Contains(e.Location))
                {
                    int imageSize = 12;
                    int imageX = tabRect.Right - imageSize - 4;
                    int imageY = tabRect.Top + (tabRect.Height - imageSize) / 2;
                    var closeRect = new Rectangle(imageX, imageY, imageSize, imageSize);

                    if (closeRect.Contains(e.Location))
                    {
                        CloseTab(i);
                        break;
                    }
                }
            }
        }

        private void CloseTab(int tabIndex)
        {
            if (tabIndex < 0 || tabIndex >= tabControl1.TabPages.Count) return;

            var tabPage = tabControl1.TabPages[tabIndex];

            foreach (Control control in tabPage.Controls)
            {
                if (control is DataGridView dgv)
                {
                    dgv.DataSource = null;
                    dgv.Dispose();
                }
            }

            tabPage.Controls.Clear();
            tabControl1.TabPages.RemoveAt(tabIndex);

            if (tabControl1.TabPages.Count > 0)
            {
                currentDataTab = tabControl1.SelectedTab;
            }
            else
            {
                currentDataTab = null;
            }
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != currentDataTab)
            {
                if (currentDataTab != null)
                {
                    foreach (Control control in currentDataTab.Controls)
                    {
                        if (control is DataGridView dgv)
                        {
                            try
                            {
                                dgv.EndEdit(DataGridViewDataErrorContexts.Commit);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error in EndEdit: {ex.Message}");
                            }
                        }
                    }
                }
                currentDataTab = tabControl1.SelectedTab;
            }
        }

        public void LoadData(DataTable dataTable, string tableName)
        {
            if (dataTable == null || string.IsNullOrEmpty(tableName))
                return;

            foreach (TabPage tabPage in tabControl1.TabPages)
            {
                if (tabPage.Text.Equals(tableName, StringComparison.OrdinalIgnoreCase))
                {
                    tabControl1.SelectedTab = tabPage;
                    currentDataTab = tabPage;
                    return;
                }
            }

            TabPage newTabPage = new TabPage(tableName);
            DataGridView newDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = true,
                ReadOnly = false,
                AllowUserToOrderColumns = true,
                BorderStyle = BorderStyle.None,
                EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2,
                Enabled = true
            };

            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, newDataGridView, new object[] { true });

            newDataGridView.DataError += DataGridView_DataError;
            newTabPage.Controls.Add(newDataGridView);
            tabControl1.TabPages.Add(newTabPage);

            foreach (DataColumn column in dataTable.Columns)
            {
                column.ReadOnly = false;
            }

            var dataView = new DataView(dataTable)
            {
                AllowEdit = true,
                AllowNew = true,
                AllowDelete = true
            };
            newDataGridView.DataSource = dataView;

            newDataGridView.Focus();
            tabControl1.SelectedTab = newTabPage;
            currentDataTab = newTabPage;
        }

        private void DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Console.WriteLine($"DataGridView Error: {e.Exception.Message}, Context: {e.Context}, Row: {e.RowIndex}, Column: {e.ColumnIndex}");
            MessageBox.Show($"Data error: {e.Exception.Message}\n\nError context: {e.Context}\nRow: {e.RowIndex}, Column: {e.ColumnIndex}",
                "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            e.ThrowException = false;
        }

        public void CloseAllDataTabs()
        {
            for (int i = tabControl1.TabPages.Count - 1; i > 0; i--)
            {
                CloseTab(i);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            while (tabControl1.TabPages.Count > 0)
            {
                CloseTab(0);
            }
        }

        public DataTable GetTableData(string tableName)
        {
            return DatabaseHelper.GetTableData(tableName, Form2.ConnectionString);
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.MainForm = this; 
            form2.StartPosition = FormStartPosition.CenterParent;
            if (form2.ShowDialog(this) == DialogResult.OK)
            {
                UpdateConnectionStatus();
            }
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            SaveCurrentTabData();
        }

        private void SaveCurrentTabData()
        {
            if (tabControl1.SelectedTab == null || tabControl1.SelectedTab.Controls.Count == 0)
            {
                MessageBox.Show("No active table to save", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dgv = tabControl1.SelectedTab.Controls[0] as DataGridView;
            if (dgv == null || dgv.DataSource == null)
            {
                MessageBox.Show("No data to save", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var dataView = dgv.DataSource as DataView;
                if (dataView == null) return;

                var dataTable = dataView.Table;
                Console.WriteLine($"Modified rows: {dataTable.Select(null, null, DataViewRowState.ModifiedCurrent).Length}");
                Console.WriteLine($"Added rows: {dataTable.Select(null, null, DataViewRowState.Added).Length}");
                Console.WriteLine($"Deleted rows: {dataTable.Select(null, null, DataViewRowState.Deleted).Length}");

                using (var conn = new NpgsqlConnection(Form2.ConnectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (DataRow row in dataView.Table.Rows)
                            {
                                if (row.RowState == DataRowState.Modified)
                                {
                                    DatabaseHelper.UpdateRow(conn, row, Form2.SelectedTable, transaction);
                                }
                                else if (row.RowState == DataRowState.Added)
                                {
                                    DatabaseHelper.InsertRow(conn, row, Form2.SelectedTable, transaction);
                                }
                                else if (row.RowState == DataRowState.Deleted)
                                {
                                    DatabaseHelper.DeleteRow(conn, row, Form2.SelectedTable, transaction);
                                }
                            }

                            transaction.Commit();
                            dataTable.AcceptChanges();
                            MessageBox.Show("Data saved successfully", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Error saving data: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Critical error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            TabPage newTabPage = new TabPage("New Page");
            DataGridView dgv = new DataGridView { Dock = DockStyle.Fill };
            newTabPage.Controls.Add(dgv);
            tabControl1.TabPages.Add(newTabPage);
            tabControl1.SelectedTab = newTabPage;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count > 0)
            {
                CloseTab(tabControl1.SelectedIndex);
            }
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.StartPosition = FormStartPosition.CenterParent;
            form3.ShowDialog(this);
        }

        private void connectDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.StartPosition = FormStartPosition.CenterParent;
            if (form2.ShowDialog(this) == DialogResult.OK)
            {
                UpdateConnectionStatus();
            }
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("H:\\Other\\Програмирование\\Kursach\\Resources\\help.html");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open help file: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Select CSV file to import"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable dt = ReadCSVFile(openFileDialog.FileName);
                    if (tabControl1.SelectedTab != null)
                    {
                        var dgv = tabControl1.SelectedTab.Controls[0] as DataGridView;
                        if (dgv != null)
                        {
                            dgv.DataSource = dt;
                        }
                    }
                    else
                    {
                        LoadData(dt, Path.GetFileNameWithoutExtension(openFileDialog.FileName));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading file: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private DataTable ReadCSVFile(string filePath)
        {
            DataTable dt = new DataTable();

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
                throw new Exception("File is empty");

            string[] headers = lines[0].Split(',');
            foreach (string header in headers)
            {
                dt.Columns.Add(header.Trim());
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(',');
                if (fields.Length < dt.Columns.Count)
                {
                    fields = fields.Concat(Enumerable.Repeat(string.Empty, dt.Columns.Count - fields.Length)).ToArray();
                }
                dt.Rows.Add(fields);
            }

            return dt;
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null || tabControl1.SelectedTab.Controls.Count == 0)
            {
                MessageBox.Show("No active table to export", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dgv = tabControl1.SelectedTab.Controls[0] as DataGridView;
            if (dgv == null || dgv.Rows.Count == 0)
            {
                MessageBox.Show("No data to export", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Export to CSV"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportDataGridViewToCSV(dgv, saveFileDialog.FileName);
                    MessageBox.Show("Data exported successfully", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting data: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportDataGridViewToCSV(DataGridView dgv, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                var headers = dgv.Columns.Cast<DataGridViewColumn>()
                    .Where(c => c.Visible)
                    .Select(c => EscapeCsvValue(c.HeaderText));
                sw.WriteLine(string.Join(",", headers));

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        var cells = row.Cells.Cast<DataGridViewCell>()
                            .Where(c => c.Visible)
                            .Select(c => EscapeCsvValue(Convert.ToString(c.Value)));
                        sw.WriteLine(string.Join(",", cells));
                    }
                }
            }
        }

        private string EscapeCsvValue(string value)
        {
            if (value == null)
                return "";

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                value = value.Replace("\"", "\"\"");
                return $"\"{value}\"";
            }
            return value;
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            UpdateConnectionStatus();
            
        }

        private void toolStripButton3_Click_2(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.StartPosition = FormStartPosition.CenterParent;
            form3.ShowDialog(this);
        }

        // Остальные пустые обработчики событий
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void toolStripDropDownButton1_Click_1(object sender, EventArgs e) { }
        private void terminalToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void toolStripDropDownButton5_Click(object sender, EventArgs e) { }
        private void toolStripDropDownButton3_Click(object sender, EventArgs e) { }
        private void toolStripButton3_Click_1(object sender, EventArgs e) { }
        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void toolStripButton5_Click(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
        private void toolStripDropDownButton2_Click(object sender, EventArgs e) { }
        private void toolStripComboBox1_Click(object sender, EventArgs e) { }
        private void toolStripDropDownButton4_Click(object sender, EventArgs e) { }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void toolStripDropDownButton1_Click(object sender, EventArgs e) { }
        private void toolStripButton3_Click(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void toolStripDropDownButton7_Click(object sender, EventArgs e) { }
        private void toolStripDropDownButton6_Click(object sender, EventArgs e) { }
        private void button1_Click_2(object sender, EventArgs e) { }
        private void toolStripButton6_Click(object sender, EventArgs e) { }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Orientation = Orientation.Vertical;
            splitContainer1.SplitterDistance = 300;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.Panel1MinSize = 100;
            splitContainer1.Panel2MinSize = 0;
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged_2(object sender, EventArgs e)
        {

        }
    }
}