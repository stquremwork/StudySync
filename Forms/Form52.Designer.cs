using System.Windows.Forms;

namespace FurnitureDatabase
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox txtId, txtName, txtType, txtPrice;
        private System.Windows.Forms.Button btnAdd, btnUpdate, btnDelete, btnClear;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblId, lblName, lblType, lblPrice;

        private System.Windows.Forms.TextBox txtSearch, txtMinPrice, txtMaxPrice;
        private System.Windows.Forms.Label lblSearch, lblMinPrice, lblMaxPrice, lblTotal;
        private System.Windows.Forms.Button btnSearch, btnFilter, btnExportCsv, btnImportCsv;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label lblCategory;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            txtId = new TextBox();
            txtName = new TextBox();
            txtType = new TextBox();
            txtPrice = new TextBox();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnClear = new Button();
            dataGridView1 = new DataGridView();
            lblId = new Label();
            lblName = new Label();
            lblType = new Label();
            lblPrice = new Label();

            // new controls
            txtSearch = new TextBox();
            lblSearch = new Label();
            btnSearch = new Button();

            txtMinPrice = new TextBox();
            txtMaxPrice = new TextBox();
            lblMinPrice = new Label();
            lblMaxPrice = new Label();
            btnFilter = new Button();

            lblTotal = new Label();

            btnExportCsv = new Button();
            btnImportCsv = new Button();

            cmbCategory = new ComboBox();
            lblCategory = new Label();

            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();

            // Layout

            // Labels
            lblId.Text = "ID:";
            lblId.Location = new System.Drawing.Point(30, 20);
            lblName.Text = "Name:";
            lblName.Location = new System.Drawing.Point(30, 60);
            lblType.Text = "Type:";
            lblType.Location = new System.Drawing.Point(30, 100);
            lblPrice.Text = "Price:";
            lblPrice.Location = new System.Drawing.Point(30, 140);
            lblCategory.Text = "Category:";
            lblCategory.Location = new System.Drawing.Point(30, 180);

            // TextBoxes
            txtId.Location = new System.Drawing.Point(136, 20);
            txtName.Location = new System.Drawing.Point(136, 60);
            txtType.Location = new System.Drawing.Point(136, 100);
            txtPrice.Location = new System.Drawing.Point(136, 140);

            txtId.Size = txtName.Size = txtType.Size = txtPrice.Size = new System.Drawing.Size(184, 27);

            // ComboBox
            cmbCategory.Location = new System.Drawing.Point(136, 180);
            cmbCategory.Size = new System.Drawing.Size(184, 27);
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;

            // Buttons
            btnAdd.Text = "Add";
            btnUpdate.Text = "Update";
            btnDelete.Text = "Delete";
            btnClear.Text = "Clear";

            btnAdd.Location = new System.Drawing.Point(350, 20);
            btnUpdate.Location = new System.Drawing.Point(350, 60);
            btnDelete.Location = new System.Drawing.Point(350, 100);
            btnClear.Location = new System.Drawing.Point(350, 140);

            btnAdd.Size = btnUpdate.Size = btnDelete.Size = btnClear.Size = new System.Drawing.Size(75, 34);

            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;

            // Search controls
            lblSearch.Text = "Search Name/Type:";
            lblSearch.Location = new System.Drawing.Point(460, 20);
            txtSearch.Location = new System.Drawing.Point(600, 20);
            txtSearch.Size = new System.Drawing.Size(150, 27);
            btnSearch.Text = "Search";
            btnSearch.Location = new System.Drawing.Point(760, 20);
            btnSearch.Click += btnSearch_Click;

            // Filter controls
            lblMinPrice.Text = "Min Price:";
            lblMaxPrice.Text = "Max Price:";
            lblMinPrice.Location = new System.Drawing.Point(460, 60);
            lblMaxPrice.Location = new System.Drawing.Point(460, 100);
            txtMinPrice.Location = new System.Drawing.Point(600, 60);
            txtMaxPrice.Location = new System.Drawing.Point(600, 100);
            txtMinPrice.Size = txtMaxPrice.Size = new System.Drawing.Size(150, 27);
            btnFilter.Text = "Filter";
            btnFilter.Location = new System.Drawing.Point(760, 80);
            btnFilter.Click += btnFilter_Click;

            // Export/Import
            btnExportCsv.Text = "Export CSV";
            btnImportCsv.Text = "Import CSV";
            btnExportCsv.Location = new System.Drawing.Point(880, 20);
            btnImportCsv.Location = new System.Drawing.Point(880, 60);
            btnExportCsv.Click += btnExportCsv_Click;
            btnImportCsv.Click += btnImportCsv_Click;

            // Total
            lblTotal.Text = "Total: 0";
            lblTotal.Location = new System.Drawing.Point(30, 580);
            lblTotal.Size = new System.Drawing.Size(400, 30);
            lblTotal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);

            // DataGridView
            dataGridView1.Location = new System.Drawing.Point(30, 220);
            dataGridView1.Size = new System.Drawing.Size(1192, 340);
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // MainForm
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1260, 620);
            Text = "Furniture Database";

            Controls.AddRange(new Control[] {
                lblId, txtId, lblName, txtName, lblType, txtType, lblPrice, txtPrice, lblCategory, cmbCategory,
                btnAdd, btnUpdate, btnDelete, btnClear,
                lblSearch, txtSearch, btnSearch,
                lblMinPrice, txtMinPrice, lblMaxPrice, txtMaxPrice, btnFilter,
                btnExportCsv, btnImportCsv,
                lblTotal, dataGridView1
            });

            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
