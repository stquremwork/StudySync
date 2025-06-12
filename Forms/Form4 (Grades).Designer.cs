namespace StudySync.Forms
{
    partial class Form4
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
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.comboBox_first_name = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_middle_name = new System.Windows.Forms.ComboBox();
            this.comboBox_last_name = new System.Windows.Forms.ComboBox();
            this.comboBox_group_id = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_grade = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox_subject = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.comboBox_group_id.SelectedIndexChanged += new System.EventHandler(this.comboBox_group_id_SelectedIndexChanged);
            this.SuspendLayout();
            // 
            // guna2Button1
            // 
            this.guna2Button1.BorderRadius = 10;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(389, 190);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(145, 46);
            this.guna2Button1.TabIndex = 0;
            this.guna2Button1.Text = "Выставить";
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // comboBox_first_name
            // 
            this.comboBox_first_name.FormattingEnabled = true;
            this.comboBox_first_name.Location = new System.Drawing.Point(168, 87);
            this.comboBox_first_name.Name = "comboBox_first_name";
            this.comboBox_first_name.Size = new System.Drawing.Size(121, 24);
            this.comboBox_first_name.TabIndex = 1;
            this.comboBox_first_name.SelectedIndexChanged += new System.EventHandler(this.comboBox_first_name_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Имя студента";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Фамилия студента";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Отчество студента";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // comboBox_middle_name
            // 
            this.comboBox_middle_name.FormattingEnabled = true;
            this.comboBox_middle_name.Location = new System.Drawing.Point(168, 130);
            this.comboBox_middle_name.Name = "comboBox_middle_name";
            this.comboBox_middle_name.Size = new System.Drawing.Size(121, 24);
            this.comboBox_middle_name.TabIndex = 5;
            this.comboBox_middle_name.SelectedIndexChanged += new System.EventHandler(this.comboBox_middle_name_SelectedIndexChanged);
            // 
            // comboBox_last_name
            // 
            this.comboBox_last_name.FormattingEnabled = true;
            this.comboBox_last_name.Location = new System.Drawing.Point(168, 49);
            this.comboBox_last_name.Name = "comboBox_last_name";
            this.comboBox_last_name.Size = new System.Drawing.Size(121, 24);
            this.comboBox_last_name.TabIndex = 6;
            this.comboBox_last_name.SelectedIndexChanged += new System.EventHandler(this.comboBox_last_name_SelectedIndexChanged);
            // 
            // comboBox_group_id
            // 
            this.comboBox_group_id.FormattingEnabled = true;
            this.comboBox_group_id.Location = new System.Drawing.Point(168, 9);
            this.comboBox_group_id.Name = "comboBox_group_id";
            this.comboBox_group_id.Size = new System.Drawing.Size(121, 24);
            this.comboBox_group_id.TabIndex = 7;
            this.comboBox_group_id.SelectedIndexChanged += new System.EventHandler(this.comboBox4_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Группа";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(337, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Оценка";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // comboBox_grade
            // 
            this.comboBox_grade.FormattingEnabled = true;
            this.comboBox_grade.Location = new System.Drawing.Point(413, 46);
            this.comboBox_grade.Name = "comboBox_grade";
            this.comboBox_grade.Size = new System.Drawing.Size(121, 24);
            this.comboBox_grade.TabIndex = 10;
            this.comboBox_grade.SelectedIndexChanged += new System.EventHandler(this.comboBox_grade_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(337, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 16);
            this.label6.TabIndex = 11;
            this.label6.Text = "Дата";
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
            this.guna2Button2.Location = new System.Drawing.Point(17, 191);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.Size = new System.Drawing.Size(145, 46);
            this.guna2Button2.TabIndex = 13;
            this.guna2Button2.Text = "Закрыть";
            this.guna2Button2.Click += new System.EventHandler(this.guna2Button2_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(337, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 16);
            this.label7.TabIndex = 14;
            this.label7.Text = "Предмет";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // comboBox_subject
            // 
            this.comboBox_subject.FormattingEnabled = true;
            this.comboBox_subject.Location = new System.Drawing.Point(413, 92);
            this.comboBox_subject.Name = "comboBox_subject";
            this.comboBox_subject.Size = new System.Drawing.Size(121, 24);
            this.comboBox_subject.TabIndex = 15;
            this.comboBox_subject.SelectedIndexChanged += new System.EventHandler(this.comboBox_subject_SelectedIndexChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "dd.MM.yyyy";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(413, 6);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(121, 22);
            this.dateTimePicker1.TabIndex = 16;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 256);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.comboBox_subject);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.guna2Button2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox_grade);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox_group_id);
            this.Controls.Add(this.comboBox_last_name);
            this.Controls.Add(this.comboBox_middle_name);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_first_name);
            this.Controls.Add(this.guna2Button1);
            this.Name = "Form4";
            this.Text = "ㅤ";
            this.Load += new System.EventHandler(this.Form4_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private System.Windows.Forms.ComboBox comboBox_first_name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_middle_name;
        private System.Windows.Forms.ComboBox comboBox_last_name;
        private System.Windows.Forms.ComboBox comboBox_group_id;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox_grade;
        private System.Windows.Forms.Label label6;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_subject;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}