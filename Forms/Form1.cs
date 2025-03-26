using System;
using System.Data;
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
            this.WindowState = FormWindowState.Maximized;
            Instance = this;
        }

        public void LoadData(DataTable dataTable)
        {
            dataGridView1.DataSource = dataTable;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
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
    }
}
