using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using System.Xml.Linq;

namespace FurnitureDatabase
{
    public partial class MainForm : Form
    {
        private List<FurnitureItem> furnitureList = new List<FurnitureItem>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cmbCategory.Items.AddRange(new string[] { "All", "Chair", "Table", "Bed", "Sofa", "Cabinet" });
            cmbCategory.SelectedIndex = 0;
            UpdateGrid();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Invalid price.");
                return;
            }

            FurnitureItem item = new FurnitureItem
            {
                Id = txtId.Text,
                Name = txtName.Text,
                Type = txtType.Text,
                Price = price,
                Category = cmbCategory.SelectedItem.ToString()
            };

            furnitureList.Add(item);
            ClearInputs();
            UpdateGrid();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            var selectedId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            var item = furnitureList.FirstOrDefault(x => x.Id == selectedId);
            if (item == null) return;

            item.Name = txtName.Text;
            item.Type = txtType.Text;
            item.Price = decimal.Parse(txtPrice.Text);
            item.Category = cmbCategory.SelectedItem.ToString();

            ClearInputs();
            UpdateGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            var selectedId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            furnitureList.RemoveAll(x => x.Id == selectedId);

            ClearInputs();
            UpdateGrid();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridView1.Rows[e.RowIndex];
            txtId.Text = row.Cells[0].Value.ToString();
            txtName.Text = row.Cells[1].Value.ToString();
            txtType.Text = row.Cells[2].Value.ToString();
            txtPrice.Text = row.Cells[3].Value.ToString();
            cmbCategory.SelectedItem = row.Cells[4].Value.ToString();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.ToLower();
            var filtered = furnitureList.Where(x =>
                x.Name.ToLower().Contains(keyword) ||
                x.Type.ToLower().Contains(keyword)).ToList();

            UpdateGrid(filtered);
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            bool minOk = decimal.TryParse(txtMinPrice.Text, out decimal min);
            bool maxOk = decimal.TryParse(txtMaxPrice.Text, out decimal max);
            string category = cmbCategory.SelectedItem.ToString();

            var filtered = furnitureList.Where(x =>
                (!minOk || x.Price >= min) &&
                (!maxOk || x.Price <= max) &&
                (category == "All" || x.Category == category)).ToList();

            UpdateGrid(filtered);
        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                FileName = "furniture.csv"
            };
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            using (var writer = new StreamWriter(saveFileDialog.FileName))
            {
                writer.WriteLine("Id,Name,Type,Price,Category");
                foreach (var item in furnitureList)
                {
                    writer.WriteLine($"{item.Id},{item.Name},{item.Type},{item.Price},{item.Category}");
                }
            }

            MessageBox.Show("Export complete.");
        }

        private void btnImportCsv_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv"
            };
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                var lines = File.ReadAllLines(openFileDialog.FileName).Skip(1);
                furnitureList.Clear();

                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length != 5) continue;

                    furnitureList.Add(new FurnitureItem
                    {
                        Id = parts[0],
                        Name = parts[1],
                        Type = parts[2],
                        Price = decimal.Parse(parts[3]),
                        Category = parts[4]
                    });
                }

                UpdateGrid();
                MessageBox.Show("Import complete.");
            }
            catch
            {
                MessageBox.Show("Error reading file.");
            }
        }

        private void ClearInputs()
        {
            txtId.Text = "";
            txtName.Text = "";
            txtType.Text = "";
            txtPrice.Text = "";
            cmbCategory.SelectedIndex = 0;
        }

        private void UpdateGrid(List<FurnitureItem> list = null)
        {
            var data = list ?? furnitureList;
            dataGridView1.DataSource = data.Select(x => new
            {
                x.Id,
                x.Name,
                x.Type,
                x.Price,
                x.Category
            }).ToList();

            lblTotal.Text = "Total: " + data.Sum(x => x.Price).ToString("C");
        }
    }

    public class FurnitureItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}
