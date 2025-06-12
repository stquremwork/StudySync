using System;

namespace StudySync
{
    partial class Form2
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
        /// 


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.button1 = new System.Windows.Forms.Button();
            this.guna2ButtonClose = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ButtonSendData = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ButtonConnect = new Guna.UI2.WinForms.Guna2Button();
            this.guna2CheckBoxRemSet = new Guna.UI2.WinForms.Guna2CheckBox();
            this.HostTextBox = new System.Windows.Forms.TextBox();
            this.HostLabel = new System.Windows.Forms.Label();
            this.PortLabel = new System.Windows.Forms.Label();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.DatabaseLabel = new System.Windows.Forms.Label();
            this.DatabaseTextBox = new System.Windows.Forms.TextBox();
            this.UserLabel = new System.Windows.Forms.Label();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.UserTextBox = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(506, 129);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(0, 0);
            this.button1.TabIndex = 13;
            this.button1.Text = "Send data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // guna2ButtonClose
            // 
            this.guna2ButtonClose.BorderRadius = 10;
            this.guna2ButtonClose.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonClose.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonClose.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2ButtonClose.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2ButtonClose.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2ButtonClose.ForeColor = System.Drawing.Color.White;
            this.guna2ButtonClose.Location = new System.Drawing.Point(488, 189);
            this.guna2ButtonClose.Name = "guna2ButtonClose";
            this.guna2ButtonClose.Size = new System.Drawing.Size(185, 44);
            this.guna2ButtonClose.TabIndex = 23;
            this.guna2ButtonClose.Text = "Закрыть";
            this.guna2ButtonClose.Click += new System.EventHandler(this.guna2ButtonClose_Click);
            // 
            // guna2ButtonSendData
            // 
            this.guna2ButtonSendData.BorderRadius = 10;
            this.guna2ButtonSendData.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonSendData.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonSendData.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2ButtonSendData.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2ButtonSendData.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2ButtonSendData.ForeColor = System.Drawing.Color.White;
            this.guna2ButtonSendData.Location = new System.Drawing.Point(488, 90);
            this.guna2ButtonSendData.Name = "guna2ButtonSendData";
            this.guna2ButtonSendData.Size = new System.Drawing.Size(185, 44);
            this.guna2ButtonSendData.TabIndex = 24;
            this.guna2ButtonSendData.Text = "Загрузить";
            this.guna2ButtonSendData.Click += new System.EventHandler(this.guna2ButtonSendData_Click);
            // 
            // guna2ButtonConnect
            // 
            this.guna2ButtonConnect.BorderRadius = 10;
            this.guna2ButtonConnect.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonConnect.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonConnect.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2ButtonConnect.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2ButtonConnect.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2ButtonConnect.ForeColor = System.Drawing.Color.White;
            this.guna2ButtonConnect.Location = new System.Drawing.Point(488, 26);
            this.guna2ButtonConnect.Name = "guna2ButtonConnect";
            this.guna2ButtonConnect.Size = new System.Drawing.Size(185, 44);
            this.guna2ButtonConnect.TabIndex = 26;
            this.guna2ButtonConnect.Text = "Подключится";
            this.guna2ButtonConnect.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // guna2CheckBoxRemSet
            // 
            this.guna2CheckBoxRemSet.AutoSize = true;
            this.guna2CheckBoxRemSet.BackColor = System.Drawing.Color.White;
            this.guna2CheckBoxRemSet.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CheckBoxRemSet.CheckedState.BorderRadius = 0;
            this.guna2CheckBoxRemSet.CheckedState.BorderThickness = 0;
            this.guna2CheckBoxRemSet.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CheckBoxRemSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.guna2CheckBoxRemSet.ForeColor = System.Drawing.Color.Black;
            this.guna2CheckBoxRemSet.Location = new System.Drawing.Point(20, 212);
            this.guna2CheckBoxRemSet.Name = "guna2CheckBoxRemSet";
            this.guna2CheckBoxRemSet.Size = new System.Drawing.Size(172, 21);
            this.guna2CheckBoxRemSet.TabIndex = 37;
            this.guna2CheckBoxRemSet.Text = "Сохранить настройки";
            this.guna2CheckBoxRemSet.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2CheckBoxRemSet.UncheckedState.BorderRadius = 0;
            this.guna2CheckBoxRemSet.UncheckedState.BorderThickness = 0;
            this.guna2CheckBoxRemSet.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2CheckBoxRemSet.UseVisualStyleBackColor = false;
            this.guna2CheckBoxRemSet.CheckedChanged += new System.EventHandler(this.guna2CheckBoxRemSet_CheckedChanged);
            // 
            // HostTextBox
            // 
            this.HostTextBox.Location = new System.Drawing.Point(130, 26);
            this.HostTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.HostTextBox.Name = "HostTextBox";
            this.HostTextBox.Size = new System.Drawing.Size(304, 22);
            this.HostTextBox.TabIndex = 28;
            this.HostTextBox.Text = "localhost";
            this.HostTextBox.TextChanged += new System.EventHandler(this.HostTextBox_TextChanged_1);
            // 
            // HostLabel
            // 
            this.HostLabel.AutoSize = true;
            this.HostLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.HostLabel.ForeColor = System.Drawing.Color.Black;
            this.HostLabel.Location = new System.Drawing.Point(16, 29);
            this.HostLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HostLabel.Name = "HostLabel";
            this.HostLabel.Size = new System.Drawing.Size(40, 19);
            this.HostLabel.TabIndex = 27;
            this.HostLabel.Text = "Хост:";
            this.HostLabel.Click += new System.EventHandler(this.HostLabel_Click_1);
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.PortLabel.ForeColor = System.Drawing.Color.Black;
            this.PortLabel.Location = new System.Drawing.Point(16, 61);
            this.PortLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(44, 19);
            this.PortLabel.TabIndex = 29;
            this.PortLabel.Text = "Порт:";
            this.PortLabel.Click += new System.EventHandler(this.PortLabel_Click_1);
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(130, 58);
            this.PortTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(304, 22);
            this.PortTextBox.TabIndex = 30;
            this.PortTextBox.Text = "5432";
            this.PortTextBox.TextChanged += new System.EventHandler(this.PortTextBox_TextChanged_1);
            // 
            // DatabaseLabel
            // 
            this.DatabaseLabel.AutoSize = true;
            this.DatabaseLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.DatabaseLabel.ForeColor = System.Drawing.Color.Black;
            this.DatabaseLabel.Location = new System.Drawing.Point(16, 93);
            this.DatabaseLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DatabaseLabel.Name = "DatabaseLabel";
            this.DatabaseLabel.Size = new System.Drawing.Size(91, 19);
            this.DatabaseLabel.TabIndex = 31;
            this.DatabaseLabel.Text = "База данных:";
            this.DatabaseLabel.Click += new System.EventHandler(this.DatabaseLabel_Click_1);
            // 
            // DatabaseTextBox
            // 
            this.DatabaseTextBox.Location = new System.Drawing.Point(130, 91);
            this.DatabaseTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.DatabaseTextBox.Name = "DatabaseTextBox";
            this.DatabaseTextBox.Size = new System.Drawing.Size(304, 22);
            this.DatabaseTextBox.TabIndex = 32;
            this.DatabaseTextBox.Text = "postgres";
            this.DatabaseTextBox.TextChanged += new System.EventHandler(this.DatabaseTextBox_TextChanged_1);
            // 
            // UserLabel
            // 
            this.UserLabel.AutoSize = true;
            this.UserLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.UserLabel.ForeColor = System.Drawing.Color.Black;
            this.UserLabel.Location = new System.Drawing.Point(16, 125);
            this.UserLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UserLabel.Name = "UserLabel";
            this.UserLabel.Size = new System.Drawing.Size(99, 19);
            this.UserLabel.TabIndex = 33;
            this.UserLabel.Text = "Пользователь:";
            this.UserLabel.Click += new System.EventHandler(this.UserLabel_Click_1);
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(130, 155);
            this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(304, 22);
            this.PasswordTextBox.TabIndex = 36;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged_1);
            // 
            // UserTextBox
            // 
            this.UserTextBox.Location = new System.Drawing.Point(130, 122);
            this.UserTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.UserTextBox.Name = "UserTextBox";
            this.UserTextBox.Size = new System.Drawing.Size(304, 22);
            this.UserTextBox.TabIndex = 34;
            this.UserTextBox.Text = "postgres";
            this.UserTextBox.TextChanged += new System.EventHandler(this.UserTextBox_TextChanged_1);
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.PasswordLabel.ForeColor = System.Drawing.Color.Black;
            this.PasswordLabel.Location = new System.Drawing.Point(16, 157);
            this.PasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(59, 19);
            this.PasswordLabel.TabIndex = 35;
            this.PasswordLabel.Text = "Пароль:";
            this.PasswordLabel.Click += new System.EventHandler(this.PasswordLabel_Click_1);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(685, 270);
            this.Controls.Add(this.guna2CheckBoxRemSet);
            this.Controls.Add(this.HostTextBox);
            this.Controls.Add(this.HostLabel);
            this.Controls.Add(this.PortLabel);
            this.Controls.Add(this.PortTextBox);
            this.Controls.Add(this.DatabaseLabel);
            this.Controls.Add(this.DatabaseTextBox);
            this.Controls.Add(this.UserLabel);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.UserTextBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.guna2ButtonConnect);
            this.Controls.Add(this.guna2ButtonSendData);
            this.Controls.Add(this.guna2ButtonClose);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.Text = "ㅤ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private Guna.UI2.WinForms.Guna2Button guna2ButtonClose;
        private Guna.UI2.WinForms.Guna2Button guna2ButtonSendData;
        private Guna.UI2.WinForms.Guna2Button guna2ButtonConnect;
        private Guna.UI2.WinForms.Guna2CheckBox guna2CheckBoxRemSet;
        private System.Windows.Forms.TextBox HostTextBox;
        private System.Windows.Forms.Label HostLabel;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.Label DatabaseLabel;
        private System.Windows.Forms.TextBox DatabaseTextBox;
        private System.Windows.Forms.Label UserLabel;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.TextBox UserTextBox;
        private System.Windows.Forms.Label PasswordLabel;
    }


}
