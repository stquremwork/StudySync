namespace StudySync.Forms
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.comboBox_first_name = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_middle_name = new System.Windows.Forms.ComboBox();
            this.comboBox_last_name = new System.Windows.Forms.ComboBox();
            this.comboBox_group_id = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ButtonGenerateWord = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // comboBox_first_name
            // 
            this.comboBox_first_name.FormattingEnabled = true;
            this.comboBox_first_name.Location = new System.Drawing.Point(170, 106);
            this.comboBox_first_name.Name = "comboBox_first_name";
            this.comboBox_first_name.Size = new System.Drawing.Size(121, 24);
            this.comboBox_first_name.TabIndex = 1;
            this.comboBox_first_name.SelectedIndexChanged += new System.EventHandler(this.comboBox_first_name_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Имя студента";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Фамилия студента";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Отчество студента";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // comboBox_middle_name
            // 
            this.comboBox_middle_name.FormattingEnabled = true;
            this.comboBox_middle_name.Location = new System.Drawing.Point(170, 158);
            this.comboBox_middle_name.Name = "comboBox_middle_name";
            this.comboBox_middle_name.Size = new System.Drawing.Size(121, 24);
            this.comboBox_middle_name.TabIndex = 5;
            this.comboBox_middle_name.SelectedIndexChanged += new System.EventHandler(this.comboBox_middle_name_SelectedIndexChanged);
            // 
            // comboBox_last_name
            // 
            this.comboBox_last_name.FormattingEnabled = true;
            this.comboBox_last_name.Location = new System.Drawing.Point(170, 58);
            this.comboBox_last_name.Name = "comboBox_last_name";
            this.comboBox_last_name.Size = new System.Drawing.Size(121, 24);
            this.comboBox_last_name.TabIndex = 6;
            this.comboBox_last_name.SelectedIndexChanged += new System.EventHandler(this.comboBox_last_name_SelectedIndexChanged);
            // 
            // comboBox_group_id
            // 
            this.comboBox_group_id.FormattingEnabled = true;
            this.comboBox_group_id.Location = new System.Drawing.Point(170, 12);
            this.comboBox_group_id.Name = "comboBox_group_id";
            this.comboBox_group_id.Size = new System.Drawing.Size(121, 24);
            this.comboBox_group_id.TabIndex = 7;
            this.comboBox_group_id.SelectedIndexChanged += new System.EventHandler(this.comboBox_group_id_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Группа";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // guna2Button2
            // 
            this.guna2Button2.BorderRadius = 10;
            this.guna2Button2.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button2.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button2.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button2.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button2.ForeColor = System.Drawing.Color.White;
            this.guna2Button2.Location = new System.Drawing.Point(380, 136);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.Size = new System.Drawing.Size(154, 46);
            this.guna2Button2.TabIndex = 13;
            this.guna2Button2.Text = "Закрыть";
            this.guna2Button2.Click += new System.EventHandler(this.guna2Button2_Click);
            // 
            // guna2ButtonGenerateWord
            // 
            this.guna2ButtonGenerateWord.BorderRadius = 10;
            this.guna2ButtonGenerateWord.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonGenerateWord.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonGenerateWord.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2ButtonGenerateWord.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2ButtonGenerateWord.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2ButtonGenerateWord.ForeColor = System.Drawing.Color.White;
            this.guna2ButtonGenerateWord.Location = new System.Drawing.Point(380, 12);
            this.guna2ButtonGenerateWord.Name = "guna2ButtonGenerateWord";
            this.guna2ButtonGenerateWord.Size = new System.Drawing.Size(154, 46);
            this.guna2ButtonGenerateWord.TabIndex = 15;
            this.guna2ButtonGenerateWord.Text = "Сгенерировать";
            this.guna2ButtonGenerateWord.Click += new System.EventHandler(this.guna2ButtonGenerateWord_Click); // ✅ Однократное подключение
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 202);
            this.Controls.Add(this.guna2ButtonGenerateWord);
            this.Controls.Add(this.guna2Button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox_group_id);
            this.Controls.Add(this.comboBox_last_name);
            this.Controls.Add(this.comboBox_middle_name);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_first_name);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form3";
            this.Text = "ㅤ";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_first_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_middle_name;
        private System.Windows.Forms.ComboBox comboBox_last_name;
        private System.Windows.Forms.ComboBox comboBox_group_id;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI2.WinForms.Guna2Button guna2ButtonGenerateWord;
    }
}