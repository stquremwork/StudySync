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
        private void InitializeComponent()
        {
            this.HostLabel = new System.Windows.Forms.Label();
            this.HostTextBox = new System.Windows.Forms.TextBox();
            this.PortLabel = new System.Windows.Forms.Label();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.DatabaseLabel = new System.Windows.Forms.Label();
            this.DatabaseTextBox = new System.Windows.Forms.TextBox();
            this.UserLabel = new System.Windows.Forms.Label();
            this.UserTextBox = new System.Windows.Forms.TextBox();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.TestConnectionButton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RememberUrlCheckBox = new System.Windows.Forms.CheckBox();
            this.UrlTextBox = new System.Windows.Forms.TextBox();
            this.UseUrlCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // HostLabel
            // 
            this.HostLabel.AutoSize = true;
            this.HostLabel.Location = new System.Drawing.Point(16, 18);
            this.HostLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HostLabel.Name = "HostLabel";
            this.HostLabel.Size = new System.Drawing.Size(38, 16);
            this.HostLabel.TabIndex = 0;
            this.HostLabel.Text = "Host:";
            // 
            // HostTextBox
            // 
            this.HostTextBox.Location = new System.Drawing.Point(107, 15);
            this.HostTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.HostTextBox.Name = "HostTextBox";
            this.HostTextBox.Size = new System.Drawing.Size(265, 22);
            this.HostTextBox.TabIndex = 1;
            this.HostTextBox.Text = "localhost";
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Location = new System.Drawing.Point(16, 50);
            this.PortLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(34, 16);
            this.PortLabel.TabIndex = 2;
            this.PortLabel.Text = "Port:";
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(107, 47);
            this.PortTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(265, 22);
            this.PortTextBox.TabIndex = 3;
            this.PortTextBox.Text = "5432";
            // 
            // DatabaseLabel
            // 
            this.DatabaseLabel.AutoSize = true;
            this.DatabaseLabel.Location = new System.Drawing.Point(16, 82);
            this.DatabaseLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DatabaseLabel.Name = "DatabaseLabel";
            this.DatabaseLabel.Size = new System.Drawing.Size(70, 16);
            this.DatabaseLabel.TabIndex = 4;
            this.DatabaseLabel.Text = "Database:";
            // 
            // DatabaseTextBox
            // 
            this.DatabaseTextBox.Location = new System.Drawing.Point(107, 79);
            this.DatabaseTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.DatabaseTextBox.Name = "DatabaseTextBox";
            this.DatabaseTextBox.Size = new System.Drawing.Size(265, 22);
            this.DatabaseTextBox.TabIndex = 5;
            this.DatabaseTextBox.Text = "postgres";
            // 
            // UserLabel
            // 
            this.UserLabel.AutoSize = true;
            this.UserLabel.Location = new System.Drawing.Point(16, 114);
            this.UserLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UserLabel.Name = "UserLabel";
            this.UserLabel.Size = new System.Drawing.Size(39, 16);
            this.UserLabel.TabIndex = 6;
            this.UserLabel.Text = "User:";
            // 
            // UserTextBox
            // 
            this.UserTextBox.Location = new System.Drawing.Point(107, 111);
            this.UserTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.UserTextBox.Name = "UserTextBox";
            this.UserTextBox.Size = new System.Drawing.Size(265, 22);
            this.UserTextBox.TabIndex = 7;
            this.UserTextBox.Text = "postgres";
            // 
            // PasswordLabel
            // 
            this.PasswordLabel.AutoSize = true;
            this.PasswordLabel.Location = new System.Drawing.Point(16, 146);
            this.PasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PasswordLabel.Name = "PasswordLabel";
            this.PasswordLabel.Size = new System.Drawing.Size(70, 16);
            this.PasswordLabel.TabIndex = 8;
            this.PasswordLabel.Text = "Password:";
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(107, 143);
            this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(265, 22);
            this.PasswordTextBox.TabIndex = 9;
            this.PasswordTextBox.UseSystemPasswordChar = true;
            // 
            // TestConnectionButton
            // 
            this.TestConnectionButton.Location = new System.Drawing.Point(20, 175);
            this.TestConnectionButton.Margin = new System.Windows.Forms.Padding(4);
            this.TestConnectionButton.Name = "TestConnectionButton";
            this.TestConnectionButton.Size = new System.Drawing.Size(352, 28);
            this.TestConnectionButton.TabIndex = 10;
            this.TestConnectionButton.Text = "Test Connection";
            this.TestConnectionButton.UseVisualStyleBackColor = true;
            this.TestConnectionButton.Click += new System.EventHandler(this.TestConnectionButton_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(20, 210);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(351, 24);
            this.comboBox1.TabIndex = 11;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(20, 242);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(154, 20);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "Only Public schemes";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 271);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(352, 28);
            this.button1.TabIndex = 13;
            this.button1.Text = "Send data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(20, 306);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(146, 20);
            this.checkBox2.TabIndex = 14;
            this.checkBox2.Text = "Remember settings";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(410, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "URL";
            // 
            // RememberUrlCheckBox
            // 
            this.RememberUrlCheckBox.AutoSize = true;
            this.RememberUrlCheckBox.Location = new System.Drawing.Point(407, 66);
            this.RememberUrlCheckBox.Name = "RememberUrlCheckBox";
            this.RememberUrlCheckBox.Size = new System.Drawing.Size(175, 20);
            this.RememberUrlCheckBox.TabIndex = 17;
            this.RememberUrlCheckBox.Text = "RememberUrlCheckBox";
            this.RememberUrlCheckBox.UseVisualStyleBackColor = true;
            this.RememberUrlCheckBox.CheckedChanged += new System.EventHandler(this.RememberUrlCheckBox_CheckedChanged);
            // 
            // UrlTextBox
            // 
            this.UrlTextBox.Location = new System.Drawing.Point(407, 37);
            this.UrlTextBox.Name = "UrlTextBox";
            this.UrlTextBox.Size = new System.Drawing.Size(265, 22);
            this.UrlTextBox.TabIndex = 18;
            this.UrlTextBox.TextChanged += new System.EventHandler(this.UrlTextBox_TextChanged);
            // 
            // UseUrlCheckBox
            // 
            this.UseUrlCheckBox.AutoSize = true;
            this.UseUrlCheckBox.Location = new System.Drawing.Point(20, 333);
            this.UseUrlCheckBox.Name = "UseUrlCheckBox";
            this.UseUrlCheckBox.Size = new System.Drawing.Size(132, 20);
            this.UseUrlCheckBox.TabIndex = 19;
            this.UseUrlCheckBox.Text = "UseUrlCheckBox";
            this.UseUrlCheckBox.UseVisualStyleBackColor = true;
            this.UseUrlCheckBox.CheckedChanged += new System.EventHandler(this.UseUrlCheckBox_CheckedChanged);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.UseUrlCheckBox);
            this.Controls.Add(this.UrlTextBox);
            this.Controls.Add(this.RememberUrlCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.TestConnectionButton);
            this.Controls.Add(this.PasswordTextBox);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.UserTextBox);
            this.Controls.Add(this.UserLabel);
            this.Controls.Add(this.DatabaseTextBox);
            this.Controls.Add(this.DatabaseLabel);
            this.Controls.Add(this.PortTextBox);
            this.Controls.Add(this.PortLabel);
            this.Controls.Add(this.HostTextBox);
            this.Controls.Add(this.HostLabel);
            this.Name = "Form2";
            this.Text = "Form2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label HostLabel;
        private System.Windows.Forms.TextBox HostTextBox;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.Label DatabaseLabel;
        private System.Windows.Forms.TextBox DatabaseTextBox;
        private System.Windows.Forms.Label UserLabel;
        private System.Windows.Forms.TextBox UserTextBox;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Button TestConnectionButton;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox RememberUrlCheckBox;
        private System.Windows.Forms.TextBox UrlTextBox;
        private System.Windows.Forms.CheckBox UseUrlCheckBox;
    }


}
