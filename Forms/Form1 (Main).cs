using StudySync.Forms;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace StudySync
{
    public partial class Form1 : Form
    {
        public static Form1 Instance { get; private set; }
        private Image closeImage;
        private TabPage currentDataTab = null;
        private ListView listViewTables = new ListView();

        private ContextMenuStrip CreateGridContextMenu()
        {
            var menu = new ContextMenuStrip();

            // Удалить выделенные строки
            var deleteItem = new ToolStripMenuItem("Удалить выбранные строки");
            deleteItem.Click += (sender, e) =>
            {
                if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.Count > 0)
                {
                    if (tabControl1.SelectedTab.Controls[0] is DataGridView dgv)
                    {
                        var selectedRows = dgv.SelectedRows.Cast<DataGridViewRow>().ToList();
                        if (selectedRows.Count == 0)
                        {
                            MessageBox.Show("Нет выделенных строк для удаления.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        var result = MessageBox.Show($"Вы уверены, что хотите удалить {selectedRows.Count} строк?",
                            "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            foreach (var row in selectedRows)
                            {
                                if (!row.IsNewRow)
                                    dgv.Rows.Remove(row);
                            }
                        }
                    }
                }
            };

            // Выбрать всё
            var selectAllItem = new ToolStripMenuItem("Выбрать всё");
            selectAllItem.Click += (sender, e) =>
            {
                if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.Count > 0)
                {
                    if (tabControl1.SelectedTab.Controls[0] is DataGridView dgv)
                    {
                        dgv.SelectAll();
                    }
                }
            };

            // Отменить всё выделение
            var deselectAllItem = new ToolStripMenuItem("Отменить всё выделение");
            deselectAllItem.Click += (sender, e) =>
            {
                if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.Count > 0)
                {
                    if (tabControl1.SelectedTab.Controls[0] is DataGridView dgv)
                    {
                        dgv.ClearSelection();
                    }
                }
            };

            // Добавляем пункты в меню
            menu.Items.Add(deleteItem);
            menu.Items.Add(new ToolStripSeparator()); // Разделитель
            menu.Items.Add(selectAllItem);
            menu.Items.Add(deselectAllItem);

            return menu;
        }

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

            SetupMainTab();
            InitializeTablesListView();
            UpdateConnectionStatus();
        }

        private void InitializeTablesListView()
        {
            listViewTables.Dock = DockStyle.Fill;
            listViewTables.View = View.Details;
            listViewTables.FullRowSelect = true;
            listViewTables.MultiSelect = false;
            listViewTables.Scrollable = false;
            listViewTables.GridLines = false;

            listViewTables.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            listViewTables.Columns.Add("Название таблицы", 300).ListView.Font = new Font("Segoe UI", 10, FontStyle.Regular);

            listViewTables.DoubleClick += ListViewTables_DoubleClick;
            listViewTables.ShowGroups = true;
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
                    LoadTableList();
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
                    LoadTableList();
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
                    LoadTableList();
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
            if (toolStrip1.Items.Contains(toolStripTextBox1))
            {
                toolStrip1.Items.Remove(toolStripTextBox1);
            }

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
                    LoadTableList();
                }
                catch
                {
                    toolStripTextBox1.Text = "Ошибка подключения";
                    toolStripTextBox1.ForeColor = Color.Red;
                    toolStripTextBox1.Font = new Font(toolStrip1.Font, FontStyle.Regular);
                }
            }

            toolStrip1.Items.Add(toolStripTextBox1);
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

            // Крестик больше не рисуется
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

            // Запоминаем текущую выбранную вкладку перед закрытием
            int currentSelectedIndex = tabControl1.SelectedIndex;

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

            // Если закрыли текущую выбранную вкладку
            if (currentSelectedIndex == tabIndex)
            {
                // Определяем какую вкладку выбрать после закрытия
                int newSelectedIndex = -1;

                // Если остались вкладки
                if (tabControl1.TabPages.Count > 0)
                {
                    // Если закрыли не первую вкладку, выбираем предыдущую
                    if (tabIndex > 0)
                    {
                        newSelectedIndex = tabIndex - 1;
                    }
                    // Если закрыли первую вкладку, но есть другие, выбираем следующую
                    else if (tabControl1.TabPages.Count > 1)
                    {
                        newSelectedIndex = 0;
                    }
                    // Если осталась только главная вкладка
                    else
                    {
                        newSelectedIndex = 0;
                    }

                    tabControl1.SelectedIndex = newSelectedIndex;
                    currentDataTab = tabControl1.SelectedTab;
                }
                else
                {
                    currentDataTab = null;
                }
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

            // Проверка, открыта ли уже эта вкладка
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
                AutoGenerateColumns = true,
                ReadOnly = false,
                AllowUserToOrderColumns = true,
                BorderStyle = BorderStyle.None,
                EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2,
                Enabled = true,
                MultiSelect = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect

            };

            // Включаем двойную буферизацию для лучшей производительности
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, newDataGridView, new object[] { true });

            newDataGridView.DataError += DataGridView_DataError;
            newDataGridView.ContextMenuStrip = CreateGridContextMenu();



            // Добавляем столбец с чекбоксами
            var checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                Name = "CheckBoxColumn",
                HeaderText = "Выбор"
            };
            newDataGridView.Columns.Add(checkBoxColumn);

            // Привязываем данные
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

            // Скрываем автоматически созданную пустую строку (новая запись)
            newDataGridView.RowsAdded += (sender, e) =>
            {
                for (int i = 0; i < newDataGridView.Rows.Count; i++)
                {
                    if (newDataGridView.Rows[i].IsNewRow)
                        newDataGridView.Rows[i].Visible = false;
                }
            };

            // --- НОВЫЕ СОБЫТИЯ ---
            // Обработка клика по чекбоксу
            newDataGridView.CellContentClick += (sender, e) =>
            {
                if (e.ColumnIndex == newDataGridView.Columns["CheckBoxColumn"].Index && e.RowIndex >= 0)
                {
                    var cell = newDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    bool? isChecked = cell.Value as bool?;
                    if (isChecked.HasValue)
                    {
                        // Инвертируем состояние
                        newDataGridView.Rows[e.RowIndex].Selected = isChecked.Value;
                    }
                    else
                    {
                        newDataGridView.Rows[e.RowIndex].Selected = false;
                    }
                }
            };

            // Обработка выделения строк
            newDataGridView.SelectionChanged += (sender, e) =>
            {
                foreach (DataGridViewRow row in newDataGridView.SelectedRows)
                {
                    if (row.Cells["CheckBoxColumn"].Value is bool val)
                    {
                        row.Cells["CheckBoxColumn"].Value = true;
                    }
                    else
                    {
                        row.Cells["CheckBoxColumn"].Value = true;
                    }
                }

                foreach (DataGridViewRow row in newDataGridView.Rows)
                {
                    if (!row.Selected && !row.IsNewRow)
                    {
                        row.Cells["CheckBoxColumn"].Value = false;
                    }
                }
            };

            newDataGridView.Focus();
            newTabPage.Controls.Add(newDataGridView);
            tabControl1.TabPages.Add(newTabPage);
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
            form2.Show(this);
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            SaveCurrentTabData();
        }

        private void toolStripButtonDeleteRows_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null || tabControl1.SelectedTab.Controls.Count == 0)
            {
                MessageBox.Show("Нет активной таблицы для удаления строки", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dgv = tabControl1.SelectedTab.Controls[0] as DataGridView;
            if (dgv == null || dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку для удаления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранную строку?",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    foreach (DataGridViewRow row in dgv.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            dgv.Rows.Remove(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении строки: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveCurrentTabData()
        {
            if (tabControl1.SelectedTab == null || tabControl1.SelectedTab.Controls.Count == 0)
            {
                MessageBox.Show("Нет активной таблицы для сохранения", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dgv = tabControl1.SelectedTab.Controls[0] as DataGridView;
            if (dgv == null || dgv.DataSource == null)
            {
                MessageBox.Show("Нет данных для сохранения", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dgv.EndEdit();
                var dataView = dgv.DataSource as DataView;
                if (dataView == null) return;

                var dataTable = dataView.Table;
                string tableName = tabControl1.SelectedTab.Text;

                using (var conn = new NpgsqlConnection(Form2.ConnectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            var deletedRows = dataTable.Select(null, null, DataViewRowState.Deleted);
                            foreach (DataRow row in deletedRows)
                            {
                                DatabaseHelper.DeleteRow(conn, row, tableName, transaction);
                            }

                            var modifiedRows = dataTable.Select(null, null, DataViewRowState.ModifiedCurrent);
                            foreach (DataRow row in modifiedRows)
                            {
                                DatabaseHelper.UpdateRow(conn, row, tableName, transaction);
                            }

                            var addedRows = dataTable.Select(null, null, DataViewRowState.Added);
                            foreach (DataRow row in addedRows)
                            {
                                DatabaseHelper.InsertRow(conn, row, tableName, transaction);
                            }

                            transaction.Commit();
                            dataTable.AcceptChanges();

                            // Обновляем только данные в существующем DataGridView
                            RefreshDataGridView(dgv, tableName);

                            MessageBox.Show("Данные успешно сохранены", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshDataGridView(DataGridView dgv, string tableName)
        {
            try
            {
                DataTable refreshedData = DatabaseHelper.GetTableData(tableName, Form2.ConnectionString);
                if (refreshedData != null)
                {
                    foreach (DataColumn column in refreshedData.Columns)
                    {
                        column.ReadOnly = false;
                    }

                    var dataView = new DataView(refreshedData)
                    {
                        AllowEdit = true,
                        AllowNew = true,
                        AllowDelete = true
                    };

                    dgv.DataSource = dataView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении таблицы: {ex.Message}", "Ошибка",
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
            System.Diagnostics.Process.Start("https://stquremwork.github.io/StudySync-site//");

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
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Form2.ConnectionString))
            {
                MessageBox.Show("Нет подключения к базе данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(Form2.ConnectionString))
                {
                    conn.Open();
                    Form4 form4 = new Form4(conn);
                    form4.StartPosition = FormStartPosition.CenterParent;
                    form4.ShowDialog(this); // Открываем как диалоговое окно
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при подключении к БД: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void toolStripDropDownButton1_Click_1(object sender, EventArgs e) { }
        private void terminalToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void toolStripDropDownButton5_Click(object sender, EventArgs e) { }
        private void toolStripDropDownButton3_Click(object sender, EventArgs e) { }
        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void toolStripButton5_Click(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
        private void toolStripDropDownButton2_Click(object sender, EventArgs e) { }
        private void toolStripComboBox1_Click(object sender, EventArgs e) { }
        private void toolStripDropDownButton4_Click(object sender, EventArgs e) { }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void toolStripDropDownButton1_Click(object sender, EventArgs e) { }

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
        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e) { }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e) { }
        private void listView1_SelectedIndexChanged_2(object sender, EventArgs e) { }

        private void удалитьСтрокуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null || tabControl1.SelectedTab.Controls.Count == 0)
            {
                MessageBox.Show("Нет активной таблицы для удаления строки", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dgv = tabControl1.SelectedTab.Controls[0] as DataGridView;
            if (dgv == null || dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку для удаления", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранную строку?",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    foreach (DataGridViewRow row in dgv.SelectedRows)
                    {
                        if (!row.IsNewRow)
                        {
                            dgv.Rows.Remove(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении строки: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void toolStripDropDownButtonEdit_Click(object sender, EventArgs e)
        {

        }

        private void выбратьВсёToolStripMenuItem1_Click(object sender, EventArgs e)

        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.Count > 0)
            {
                if (tabControl1.SelectedTab.Controls[0] is DataGridView dgv)
                {
                    dgv.SelectAll();
                }
            }
        }

        private void отменитьВыделениеToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.Count > 0)
            {
                if (tabControl1.SelectedTab.Controls[0] is DataGridView dgv)
                {
                    dgv.ClearSelection();
                }
            }
        }

        private void копироватьВыделенноеToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.Count > 0)
            {
                if (tabControl1.SelectedTab.Controls[0] is DataGridView dgv)
                {
                    var selectedRows = dgv.SelectedRows.Cast<DataGridViewRow>().ToList();
                    if (selectedRows.Count == 0)
                    {
                        MessageBox.Show("Нет выделенных строк для копирования.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Создаем текстовое представление строк (например, CSV)
                    StringBuilder sb = new StringBuilder();

                    foreach (var row in selectedRows.Where(r => !r.IsNewRow))
                    {
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            if (i > 0) sb.Append("\t");
                            sb.Append(row.Cells[i].Value?.ToString().Replace("\n", "").Replace("\r", ""));
                        }
                        sb.AppendLine();
                    }

                    Clipboard.SetText(sb.ToString());
                }
            }
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Controls.Count > 0)
            {
                if (tabControl1.SelectedTab.Controls[0] is DataGridView dgv)
                {
                    try
                    {
                        string clipboardText = Clipboard.GetText();
                        string[] lines = clipboardText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string line in lines)
                        {
                            string[] cells = line.Split('\t');
                            if (cells.Length == dgv.Columns.Count)
                            {
                                dgv.Rows.Add(cells);
                            }
                            else
                            {
                                MessageBox.Show($"Строка не соответствует количеству столбцов: {line}", "Ошибка вставки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при вставке: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}

    public static class DatabaseHelper
    {
        public static DataTable GetTableData(string tableName, string connectionString)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var adapter = new NpgsqlDataAdapter($"SELECT * FROM {tableName}", conn))
                {
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        public static void InsertRow(NpgsqlConnection conn, DataRow row, string tableName, NpgsqlTransaction transaction)
        {
            var columns = row.Table.Columns.Cast<DataColumn>()
                .Where(c => c.ColumnName != "id")
                .Select(c => $"\"{c.ColumnName}\"");
            var parameters = row.Table.Columns.Cast<DataColumn>()
                .Where(c => c.ColumnName != "id")
                .Select(c => $"@p{c.Ordinal}");

            string sql = $"INSERT INTO {tableName} ({string.Join(",", columns)}) VALUES ({string.Join(",", parameters)})";

            using (var cmd = new NpgsqlCommand(sql, conn, transaction))
            {
                foreach (DataColumn col in row.Table.Columns)
                {
                    if (col.ColumnName != "id")
                    {
                        cmd.Parameters.AddWithValue($"@p{col.Ordinal}", row[col] == DBNull.Value ? null : row[col]);
                    }
                }
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateRow(NpgsqlConnection conn, DataRow row, string tableName, NpgsqlTransaction transaction)
        {
            var setClause = row.Table.Columns.Cast<DataColumn>()
                .Where(c => c.ColumnName != "id")
                .Select(c => $"\"{c.ColumnName}\" = @p{c.Ordinal}");

            string sql = $"UPDATE {tableName} SET {string.Join(",", setClause)} WHERE id = @id";

            using (var cmd = new NpgsqlCommand(sql, conn, transaction))
            {
                foreach (DataColumn col in row.Table.Columns)
                {
                    if (col.ColumnName != "id")
                    {
                        cmd.Parameters.AddWithValue($"@p{col.Ordinal}", row[col] == DBNull.Value ? null : row[col]);
                    }
                }
                cmd.Parameters.AddWithValue("@id", row["id"]);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteRow(NpgsqlConnection conn, DataRow row, string tableName, NpgsqlTransaction transaction)
        {
            string sql = $"DELETE FROM {tableName} WHERE id = @id";
            using (var cmd = new NpgsqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@id", row["id", DataRowVersion.Original]);
                cmd.ExecuteNonQuery();
            }
        }

        
}