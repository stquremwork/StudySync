using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Kursach.Helpers;
using Npgsql;
using Kursach.Forms;
using System.Linq;

namespace Kursach
{
    public partial class Form1 : Form
    {
        public static Form1 Instance { get; private set; }   

        // В классе формы объявляем поле для картинки крестика
        private Image closeImage;

        // В конструкторе формы или методе инициализации:
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized; //На весь экран 
            Instance = this;

            // Загрузка картинки крестика из ресурсов (или из файла)
            closeImage = Properties.Resources.close; // или Image.FromFile("close.png");


            // Включаем пользовательскую отрисовку вкладок
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;

            // Подписываемся на события отрисовки и клика мыши
            tabControl1.DrawItem += TabControl1_DrawItem;
            tabControl1.MouseClick += TabControl1_MouseClick;



        }

        // Обработчик отрисовки вкладок — рисуем текст и крестик
        private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabPage = tabControl1.TabPages[e.Index];
            var tabRect = tabControl1.GetTabRect(e.Index);

            e.Graphics.FillRectangle(SystemBrushes.Control, tabRect);

            // Измеряем ширину текста вкладки
            Size textSize = TextRenderer.MeasureText(tabPage.Text, tabPage.Font);

            // Рисуем текст с отступом слева
            Point textLocation = new Point(tabRect.Left + 2, tabRect.Top + (tabRect.Height - textSize.Height) / 2);
            TextRenderer.DrawText(e.Graphics, tabPage.Text, tabPage.Font, textLocation, tabPage.ForeColor);

            int imageSize = 10;
            int padding = 5; // Отступ между текстом и крестиком

            // Вычисляем позицию крестика — справа от текста + padding
            int imageX = textLocation.X + textSize.Width + padding;

            // Ограничиваем, чтобы крестик не выходил за правый край вкладки
            int maxX = tabRect.Right - imageSize - 3; // 3 пикселя отступ от края вкладки
            if (imageX > maxX)
                imageX = maxX;

            Rectangle imageRect = new Rectangle(
                imageX,
                tabRect.Top + (tabRect.Height - imageSize) / 2,
                imageSize,
                imageSize);

            if (closeImage != null)
            {
                e.Graphics.DrawImage(closeImage, imageRect);
            }

        }

        // Обработчик клика мыши — если клик на крестик, закрываем вкладку
        private void TabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                Rectangle tabRect = tabControl1.GetTabRect(i);
                int imageSize = 16;
                Rectangle imageRect = new Rectangle(tabRect.Right - imageSize - 5, tabRect.Top + (tabRect.Height - imageSize) / 2, imageSize, imageSize);

                if (imageRect.Contains(e.Location))
                {
                    tabControl1.TabPages.RemoveAt(i);
                    break;
                }
            }
        }
        

        public void LoadData(DataTable dataTable, string tableName)
        {
            // Проверка, открыта ли уже такая таблица
            TabPage existingTabPage = tabControl1.TabPages.Cast<TabPage>()
                .FirstOrDefault(tp => tp.Text == tableName);

            if (existingTabPage != null)
            {
                // Если таблица уже открыта, сделать эту вкладку активной
                tabControl1.SelectedTab = existingTabPage;
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


        public void CloseAllDataTabs()
        {
            // Проходим по всем вкладкам, кроме первой (вкладка "Data Source")
            for (int i = tabControl1.TabPages.Count - 1; i > 0; i--)
            {
                // Закрываем вкладку
                tabControl1.TabPages.RemoveAt(i);
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
            // Создаём и показываем Form2
            Form2 form2 = new Form2();

            // Устанавливаем Form2 по центру относительно Form1
            form2.StartPosition = FormStartPosition.Manual;
            form2.Location = new Point(
                this.Location.X + (this.Width - form2.Width) / 2,
                this.Location.Y + (this.Height - form2.Height) / 2);

            // Показываем Form2 в модальном режиме
            form2.ShowDialog();
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
            // Создаём и показываем Form2
            Form3 form3 = new Form3();

            // Устанавливаем Form2 по центру относительно Form1
            form3.StartPosition = FormStartPosition.Manual;
            form3.Location = new Point(
                this.Location.X + (this.Width - form3.Width) / 2,
                this.Location.Y + (this.Height - form3.Height) / 2);
            
            form3.ShowDialog(); // Показываем Form2 в модальном режиме

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripDropDownButton5_Click(object sender, EventArgs e)
        {

        }

        private void connectDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Создаём и показываем Form2
            Form2 form2 = new Form2();

            // Устанавливаем Form2 по центру относительно Form1
            form2.StartPosition = FormStartPosition.Manual;
            form2.Location = new Point(
                this.Location.X + (this.Width - form2.Width) / 2,
                this.Location.Y + (this.Height - form2.Height) / 2);

            // Показываем Form2 в модальном режиме
            form2.ShowDialog();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

        }
    }
}