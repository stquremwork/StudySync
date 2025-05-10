using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

namespace Kursach.Forms
{
    public partial class GradeSelectionForm : Form
    {
        private ComboBox cbStudents;
        private ComboBox cbGroups;
        private ComboBox cbSubjects;
        private TextBox txtGrade;
        private DateTimePicker dtpDate;
        private Button btnSave;
        private Button btnCancel;
        private string connectionString;

        public int? SelectedStudentId { get; private set; }
        public int? SelectedGroupId { get; private set; }
        public int? SelectedSubjectId { get; private set; }
        public int? Grade { get; private set; }
        public DateTime? GradeDate { get; private set; }

        public GradeSelectionForm(string connectionString)
        {
            this.connectionString = connectionString;
            InitializeComponents();
            LoadComboBoxData();
        }

        private void InitializeComponents()
        {
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Выбор данных для оценки";

            cbStudents = new ComboBox { Location = new Point(20, 20), Width = 340 };
            cbGroups = new ComboBox { Location = new Point(20, 60), Width = 340 };
            cbSubjects = new ComboBox { Location = new Point(20, 100), Width = 340 };
            txtGrade = new TextBox { Location = new Point(20, 140), Width = 340 };
            dtpDate = new DateTimePicker { Location = new Point(20, 180), Width = 340 };
            btnSave = new Button { Text = "Сохранить", Location = new Point(20, 220), Width = 160 };
            btnCancel = new Button { Text = "Отмена", Location = new Point(200, 220), Width = 160 };

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.AddRange(new Control[] { cbStudents, cbGroups, cbSubjects, txtGrade, dtpDate, btnSave, btnCancel });
            this.Controls.Add(new Label { Text = "Учащийся:", Location = new Point(20, 5) });
            this.Controls.Add(new Label { Text = "Группа:", Location = new Point(20, 45) });
            this.Controls.Add(new Label { Text = "Предмет:", Location = new Point(20, 85) });
            this.Controls.Add(new Label { Text = "Оценка:", Location = new Point(20, 125) });
            this.Controls.Add(new Label { Text = "Дата:", Location = new Point(20, 165) });
        }

        private void LoadComboBoxData()
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    MessageBox.Show("Строка подключения пуста.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Loading data for GradeSelectionForm", "Debug");
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    var studentAdapter = new NpgsqlDataAdapter("SELECT student_id, first_name || ' ' || last_name AS full_name FROM students", conn);
                    var studentTable = new DataTable();
                    studentAdapter.Fill(studentTable);
                    if (studentTable.Rows.Count == 0)
                        MessageBox.Show("Таблица students пуста.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbStudents.DataSource = studentTable;
                    cbStudents.DisplayMember = "full_name";
                    cbStudents.ValueMember = "student_id";

                    var groupAdapter = new NpgsqlDataAdapter("SELECT group_id, group_name FROM groups", conn);
                    var groupTable = new DataTable();
                    groupAdapter.Fill(groupTable);
                    if (groupTable.Rows.Count == 0)
                        MessageBox.Show("Таблица groups пуста.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbGroups.DataSource = groupTable;
                    cbGroups.DisplayMember = "group_name";
                    cbGroups.ValueMember = "group_id";

                    var subjectAdapter = new NpgsqlDataAdapter("SELECT subject_id, subject_name FROM subjects", conn);
                    var subjectTable = new DataTable();
                    subjectAdapter.Fill(subjectTable);
                    if (subjectTable.Rows.Count == 0)
                        MessageBox.Show("Таблица subjects пуста.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbSubjects.DataSource = subjectTable;
                    cbSubjects.DisplayMember = "subject_name";
                    cbSubjects.ValueMember = "subject_id";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}\nStackTrace: {ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cbStudents.SelectedValue == null || cbGroups.SelectedValue == null || cbSubjects.SelectedValue == null || string.IsNullOrEmpty(txtGrade.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtGrade.Text, out int grade) || grade < 1 || grade > 5)
            {
                MessageBox.Show("Оценка должна быть числом от 1 до 5!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SelectedStudentId = (int)cbStudents.SelectedValue;
            SelectedGroupId = (int)cbGroups.SelectedValue;
            SelectedSubjectId = (int)cbSubjects.SelectedValue;
            Grade = grade;
            GradeDate = dtpDate.Value;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}