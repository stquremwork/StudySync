using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Npgsql;
using Kursach.Helpers;
using Kursach.Forms;

namespace Kursach
{
    public partial class Form1 : Form
    {
        public static Form1 Instance { get; private set; }
        private Image closeImage;
        private TabPage currentDataTab = null;

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
                            dgv.EndEdit(DataGridViewDataErrorContexts.Commit);
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
                BorderStyle = BorderStyle.None
            };

            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null, newDataGridView, new object[] { true });

            newDataGridView.DataError += DataGridView_DataError;
            newTabPage.Controls.Add(newDataGridView);
            tabControl1.TabPages.Add(newTabPage);

            var dataView = new DataView(dataTable);
            newDataGridView.DataSource = dataView;


            tabControl1.SelectedTab = newTabPage;
            currentDataTab = newTabPage;
        }

        private void DataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show($"Data error: {e.Exception.Message}\n\nError context: {e.Context}",
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
            form2.StartPosition = FormStartPosition.CenterParent;
            form2.ShowDialog(this);
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
            form2.ShowDialog(this);
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("H:\\Other\\Програмирование\\Проекты C#\\Project-Repository\\Program\\Other\\help.html");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open help file: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Empty event handlers
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void toolStripDropDownButton1_Click_1(object sender, EventArgs e) { }
        private void importToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e) { }
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


    }
}