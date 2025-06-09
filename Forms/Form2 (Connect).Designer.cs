using System;

namespace Kursach
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
            this.guna2TabControl1 = new Guna.UI2.WinForms.Guna2TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
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
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.guna2RememberUrl = new Guna.UI2.WinForms.Guna2CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.UrlTextBox = new System.Windows.Forms.TextBox();
            this.guna2ButtonClose = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ButtonSendData = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ButtonConnect = new Guna.UI2.WinForms.Guna2Button();
            this.guna2TabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(605, 151);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(0, 0);
            this.button1.TabIndex = 13;
            this.button1.Text = "Send data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // guna2TabControl1
            // 
            this.guna2TabControl1.Controls.Add(this.tabPage3);
            this.guna2TabControl1.Controls.Add(this.tabPage4);
            this.guna2TabControl1.ItemSize = new System.Drawing.Size(180, 40);
            this.guna2TabControl1.Location = new System.Drawing.Point(12, 4);
            this.guna2TabControl1.Name = "guna2TabControl1";
            this.guna2TabControl1.SelectedIndex = 0;
            this.guna2TabControl1.Size = new System.Drawing.Size(561, 305);
            this.guna2TabControl1.TabButtonHoverState.BorderColor = System.Drawing.Color.Empty;
            this.guna2TabControl1.TabButtonHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.guna2TabControl1.TabButtonHoverState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.guna2TabControl1.TabButtonHoverState.ForeColor = System.Drawing.Color.White;
            this.guna2TabControl1.TabButtonHoverState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(52)))), ((int)(((byte)(70)))));
            this.guna2TabControl1.TabButtonIdleState.BorderColor = System.Drawing.Color.Empty;
            this.guna2TabControl1.TabButtonIdleState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.guna2TabControl1.TabButtonIdleState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.guna2TabControl1.TabButtonIdleState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(160)))), ((int)(((byte)(167)))));
            this.guna2TabControl1.TabButtonIdleState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.guna2TabControl1.TabButtonSelectedState.BorderColor = System.Drawing.Color.Empty;
            this.guna2TabControl1.TabButtonSelectedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(37)))), ((int)(((byte)(49)))));
            this.guna2TabControl1.TabButtonSelectedState.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.guna2TabControl1.TabButtonSelectedState.ForeColor = System.Drawing.Color.White;
            this.guna2TabControl1.TabButtonSelectedState.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(132)))), ((int)(((byte)(255)))));
            this.guna2TabControl1.TabButtonSize = new System.Drawing.Size(180, 40);
            this.guna2TabControl1.TabIndex = 22;
            this.guna2TabControl1.TabMenuBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.guna2TabControl1.TabMenuOrientation = Guna.UI2.WinForms.TabMenuOrientation.HorizontalTop;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(55)))), ((int)(((byte)(70)))));
            this.tabPage3.Controls.Add(this.guna2CheckBoxRemSet);
            this.tabPage3.Controls.Add(this.HostTextBox);
            this.tabPage3.Controls.Add(this.HostLabel);
            this.tabPage3.Controls.Add(this.PortLabel);
            this.tabPage3.Controls.Add(this.PortTextBox);
            this.tabPage3.Controls.Add(this.DatabaseLabel);
            this.tabPage3.Controls.Add(this.DatabaseTextBox);
            this.tabPage3.Controls.Add(this.UserLabel);
            this.tabPage3.Controls.Add(this.PasswordTextBox);
            this.tabPage3.Controls.Add(this.UserTextBox);
            this.tabPage3.Controls.Add(this.PasswordLabel);
            this.tabPage3.Location = new System.Drawing.Point(4, 44);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(553, 257);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "General";
            this.tabPage3.Click += new System.EventHandler(this.tabPage3_Click);
            // 
            // guna2CheckBoxRemSet
            // 
            this.guna2CheckBoxRemSet.AutoSize = true;
            this.guna2CheckBoxRemSet.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CheckBoxRemSet.CheckedState.BorderRadius = 0;
            this.guna2CheckBoxRemSet.CheckedState.BorderThickness = 0;
            this.guna2CheckBoxRemSet.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2CheckBoxRemSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.guna2CheckBoxRemSet.ForeColor = System.Drawing.Color.White;
            this.guna2CheckBoxRemSet.Location = new System.Drawing.Point(18, 211);
            this.guna2CheckBoxRemSet.Name = "guna2CheckBoxRemSet";
            this.guna2CheckBoxRemSet.Size = new System.Drawing.Size(152, 21);
            this.guna2CheckBoxRemSet.TabIndex = 26;
            this.guna2CheckBoxRemSet.Text = "Remember settings";
            this.guna2CheckBoxRemSet.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2CheckBoxRemSet.UncheckedState.BorderRadius = 0;
            this.guna2CheckBoxRemSet.UncheckedState.BorderThickness = 0;
            this.guna2CheckBoxRemSet.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2CheckBoxRemSet.CheckedChanged += new System.EventHandler(this.guna2CheckBox1_CheckedChanged);
            // 
            // HostTextBox
            // 
            this.HostTextBox.Location = new System.Drawing.Point(106, 17);
            this.HostTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.HostTextBox.Name = "HostTextBox";
            this.HostTextBox.Size = new System.Drawing.Size(265, 22);
            this.HostTextBox.TabIndex = 16;
            this.HostTextBox.Text = "localhost";
            this.HostTextBox.TextChanged += new System.EventHandler(this.HostTextBox_TextChanged);
            // 
            // HostLabel
            // 
            this.HostLabel.AutoSize = true;
            this.HostLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.HostLabel.ForeColor = System.Drawing.Color.White;
            this.HostLabel.Location = new System.Drawing.Point(15, 20);
            this.HostLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HostLabel.Name = "HostLabel";
            this.HostLabel.Size = new System.Drawing.Size(41, 19);
            this.HostLabel.TabIndex = 15;
            this.HostLabel.Text = "Host:";
            this.HostLabel.Click += new System.EventHandler(this.HostLabel_Click);
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.PortLabel.ForeColor = System.Drawing.Color.White;
            this.PortLabel.Location = new System.Drawing.Point(15, 52);
            this.PortLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(37, 19);
            this.PortLabel.TabIndex = 17;
            this.PortLabel.Text = "Port:";
            this.PortLabel.Click += new System.EventHandler(this.PortLabel_Click);
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(106, 49);
            this.PortTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(265, 22);
            this.PortTextBox.TabIndex = 18;
            this.PortTextBox.Text = "5432";
            this.PortTextBox.TextChanged += new System.EventHandler(this.PortTextBox_TextChanged);
            // 
            // DatabaseLabel
            // 
            this.DatabaseLabel.AutoSize = true;
            this.DatabaseLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.DatabaseLabel.ForeColor = System.Drawing.Color.White;
            this.DatabaseLabel.Location = new System.Drawing.Point(15, 84);
            this.DatabaseLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DatabaseLabel.Name = "DatabaseLabel";
            this.DatabaseLabel.Size = new System.Drawing.Size(69, 19);
            this.DatabaseLabel.TabIndex = 19;
            this.DatabaseLabel.Text = "Database:";
            this.DatabaseLabel.Click += new System.EventHandler(this.DatabaseLabel_Click);
            // 
            // DatabaseTextBox
            // 
            this.DatabaseTextBox.Location = new System.Drawing.Point(106, 81);
            this.DatabaseTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.DatabaseTextBox.Name = "DatabaseTextBox";
            this.DatabaseTextBox.Size = new System.Drawing.Size(265, 22);
            this.DatabaseTextBox.TabIndex = 20;
            this.DatabaseTextBox.Text = "postgres";
            this.DatabaseTextBox.TextChanged += new System.EventHandler(this.DatabaseTextBox_TextChanged);
            // 
            // UserLabel
            // 
            this.UserLabel.AutoSize = true;
            this.UserLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.UserLabel.ForeColor = System.Drawing.Color.White;
            this.UserLabel.Location = new System.Drawing.Point(15, 116);
            this.UserLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UserLabel.Name = "UserLabel";
            this.UserLabel.Size = new System.Drawing.Size(40, 19);
            this.UserLabel.TabIndex = 21;
            this.UserLabel.Text = "User:";
            this.UserLabel.Click += new System.EventHandler(this.UserLabel_Click);
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(106, 145);
            this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(265, 22);
            this.PasswordTextBox.TabIndex = 24;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
            // 
            // UserTextBox
            // 
            this.UserTextBox.Location = new System.Drawing.Point(106, 113);
            this.UserTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.UserTextBox.Name = "UserTextBox";
            this.UserTextBox.Size = new System.Drawing.Size(265, 22);
            this.UserTextBox.TabIndex = 22;
            this.UserTextBox.Text = "postgres";
            this.UserTextBox.TextChanged += new System.EventHandler(this.UserTextBox_TextChanged);
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.PasswordLabel.ForeColor = System.Drawing.Color.White;
            this.PasswordLabel.Location = new System.Drawing.Point(15, 148);
            this.PasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(70, 19);
            this.PasswordLabel.TabIndex = 23;
            this.PasswordLabel.Text = "Password:";
            this.PasswordLabel.Click += new System.EventHandler(this.PasswordLabel_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.tabPage4.Controls.Add(this.guna2RememberUrl);
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Controls.Add(this.UrlTextBox);
            this.tabPage4.Location = new System.Drawing.Point(4, 44);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(553, 257);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "URL";
            this.tabPage4.Click += new System.EventHandler(this.tabPage4_Click);
            // 
            // guna2RememberUrl
            // 
            this.guna2RememberUrl.AutoSize = true;
            this.guna2RememberUrl.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2RememberUrl.CheckedState.BorderRadius = 0;
            this.guna2RememberUrl.CheckedState.BorderThickness = 0;
            this.guna2RememberUrl.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2RememberUrl.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.guna2RememberUrl.ForeColor = System.Drawing.Color.White;
            this.guna2RememberUrl.Location = new System.Drawing.Point(13, 47);
            this.guna2RememberUrl.Name = "guna2RememberUrl";
            this.guna2RememberUrl.Size = new System.Drawing.Size(126, 23);
            this.guna2RememberUrl.TabIndex = 25;
            this.guna2RememberUrl.Text = "Remember URL";
            this.guna2RememberUrl.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2RememberUrl.UncheckedState.BorderRadius = 0;
            this.guna2RememberUrl.UncheckedState.BorderThickness = 0;
            this.guna2RememberUrl.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.guna2RememberUrl.CheckedChanged += new System.EventHandler(this.guna2CheckBox1_CheckedChanged_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 19);
            this.label1.TabIndex = 22;
            this.label1.Text = "URL";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // UrlTextBox
            // 
            this.UrlTextBox.Location = new System.Drawing.Point(49, 9);
            this.UrlTextBox.Name = "UrlTextBox";
            this.UrlTextBox.Size = new System.Drawing.Size(265, 22);
            this.UrlTextBox.TabIndex = 24;
            this.UrlTextBox.TextChanged += new System.EventHandler(this.UrlTextBox_TextChanged);
            // 
            // guna2ButtonClose
            // 
            this.guna2ButtonClose.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonClose.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonClose.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2ButtonClose.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2ButtonClose.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2ButtonClose.ForeColor = System.Drawing.Color.White;
            this.guna2ButtonClose.Location = new System.Drawing.Point(587, 261);
            this.guna2ButtonClose.Name = "guna2ButtonClose";
            this.guna2ButtonClose.Size = new System.Drawing.Size(185, 44);
            this.guna2ButtonClose.TabIndex = 23;
            this.guna2ButtonClose.Text = "Close";
            this.guna2ButtonClose.Click += new System.EventHandler(this.guna2ButtonClose_Click);
            // 
            // guna2ButtonSendData
            // 
            this.guna2ButtonSendData.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonSendData.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonSendData.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2ButtonSendData.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2ButtonSendData.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2ButtonSendData.ForeColor = System.Drawing.Color.White;
            this.guna2ButtonSendData.Location = new System.Drawing.Point(588, 199);
            this.guna2ButtonSendData.Name = "guna2ButtonSendData";
            this.guna2ButtonSendData.Size = new System.Drawing.Size(185, 44);
            this.guna2ButtonSendData.TabIndex = 24;
            this.guna2ButtonSendData.Text = "Send data";
            this.guna2ButtonSendData.Click += new System.EventHandler(this.guna2ButtonSendData_Click);
            // 
            // guna2ButtonConnect
            // 
            this.guna2ButtonConnect.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonConnect.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2ButtonConnect.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2ButtonConnect.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2ButtonConnect.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2ButtonConnect.ForeColor = System.Drawing.Color.White;
            this.guna2ButtonConnect.Location = new System.Drawing.Point(587, 48);
            this.guna2ButtonConnect.Name = "guna2ButtonConnect";
            this.guna2ButtonConnect.Size = new System.Drawing.Size(185, 44);
            this.guna2ButtonConnect.TabIndex = 26;
            this.guna2ButtonConnect.Text = "Connect";
            this.guna2ButtonConnect.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(42)))), ((int)(((byte)(57)))));
            this.ClientSize = new System.Drawing.Size(785, 329);
            this.Controls.Add(this.guna2ButtonConnect);
            this.Controls.Add(this.guna2ButtonSendData);
            this.Controls.Add(this.guna2ButtonClose);
            this.Controls.Add(this.guna2TabControl1);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.Text = "ㅤ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.guna2TabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private Guna.UI2.WinForms.Guna2TabControl guna2TabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox HostTextBox;
        private System.Windows.Forms.Label HostLabel;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.Label DatabaseLabel;
        private System.Windows.Forms.TextBox DatabaseTextBox;
        private System.Windows.Forms.Label UserLabel;
        private System.Windows.Forms.TextBox UserTextBox;
        private System.Windows.Forms.Label PasswordLabel;
        private Guna.UI2.WinForms.Guna2Button guna2ButtonClose;
        private Guna.UI2.WinForms.Guna2Button guna2ButtonSendData;
        private Guna.UI2.WinForms.Guna2Button guna2ButtonConnect;
        private Guna.UI2.WinForms.Guna2CheckBox guna2CheckBoxRemSet;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox UrlTextBox;
        private Guna.UI2.WinForms.Guna2CheckBox guna2RememberUrl;
        private System.Windows.Forms.TextBox PasswordTextBox;
    }


}
