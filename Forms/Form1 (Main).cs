using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Kursach.Helpers;
using Npgsql;

namespace Kursach
{
    public partial class Form1 : Form
    {
        public static Form1 Instance { get; private set; }

        public Form1()
        {
            InitializeComponent();
            //На весь экран this.WindowState = FormWindowState.Maximized;
            Instance = this;
        }

        public void LoadData(DataTable dataTable, string tableName)
        {
            // Проверка наличия вкладок
            if (tabControl1.TabPages.Count > 0)
            {
                // Проверка пустоты первой вкладки
                TabPage firstTabPage = tabControl1.TabPages[0];
                DataGridView firstDataGridView = firstTabPage.Controls[0] as DataGridView;

                if (firstDataGridView != null && firstDataGridView.DataSource == null)
                {
                    // Загрузка данных в пустую существующую вкладку
                    firstDataGridView.DataSource = dataTable;
                    firstTabPage.Text = tableName; // Установка названия вкладки
                }
                else
                {
                    // Создание новой вкладки и загрузка данных в неё
                    TabPage newTabPage = new TabPage(tableName);
                    tabControl1.TabPages.Add(newTabPage);

                    DataGridView newDataGridView = new DataGridView();
                    newDataGridView.Dock = DockStyle.Fill; // Заполнять всю вкладку
                    newTabPage.Controls.Add(newDataGridView);

                    newDataGridView.DataSource = dataTable;
                    newDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                    // Сделать новую вкладку активной
                    tabControl1.SelectedTab = newTabPage;
                }
            }
            else
            {
                // Создание новой вкладки и загрузка данных в неё, если вкладок нет
                TabPage newTabPage = new TabPage(tableName);
                tabControl1.TabPages.Add(newTabPage);

                DataGridView newDataGridView = new DataGridView();
                newDataGridView.Dock = DockStyle.Fill; // Заполнять всю вкладку
                newTabPage.Controls.Add(newDataGridView);

                newDataGridView.DataSource = dataTable;
                newDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                // Сделать новую вкладку активной
                tabControl1.SelectedTab = newTabPage;
            }
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("No data to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable dataTable = (DataTable)dataGridView1.DataSource;

            if (string.IsNullOrEmpty(Form2.ConnectionString) || string.IsNullOrEmpty(Form2.SelectedTable))
            {
                MessageBox.Show("You must select a table and connect to the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(Form2.ConnectionString))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (DataRow row in dataTable.Rows)
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
                            MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Error executing transaction: {ex.Message}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        public DataTable GetTableData(string tableName)
        {
            return DatabaseHelper.GetTableData(tableName, Form2.ConnectionString);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void terminalToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("H:\\Other\\Програмирование\\Проекты C#\\Project-Repository\\Program\\Other\\help.html");
        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();

        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("No data to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable dataTable = (DataTable)dataGridView1.DataSource;

            if (string.IsNullOrEmpty(Form2.ConnectionString) || string.IsNullOrEmpty(Form2.SelectedTable))
            {
                MessageBox.Show("You must select a table and connect to the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(Form2.ConnectionString))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (DataRow row in dataTable.Rows)
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
                            MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Error executing transaction: {ex.Message}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            // Создаем новую страницу
            TabPage newTabPage = new TabPage();

            // Указываем название страницы
            newTabPage.Text = "New Page";

            // Добавляем страницу в TabControl
            tabControl1.TabPages.Add(newTabPage);

            // Добавляем элементы управления на новую страницу (например, DataGridView)
            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            newTabPage.Controls.Add(dgv);

            // Выбираем созданную страницу
            tabControl1.SelectedTab = newTabPage;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            // Проверяем, есть ли страницы в TabControl
            if (tabControl1.TabPages.Count > 0)
            {
                // Удаляем текущую страницу
                tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);

                // Если страниц больше не осталось, выходим из метода
                if (tabControl1.TabPages.Count == 0)
                    return;

                // Выбираем последнюю страницу, если текущая была удалена
                tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
            }
            else
            {
                MessageBox.Show("Нет страниц для удаления.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDropDownButton4_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}