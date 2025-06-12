using Npgsql;
using NpgsqlTypes;
using System;
using System.Data;
using System.Windows.Forms;

namespace StudySync.Forms
{
    public partial class Form4 : Form
    {
        private NpgsqlConnection _connection;
        public Form4(NpgsqlConnection connection)
        {
            InitializeComponent();
            _connection = connection;
        }

        public Form4() : this(null) { }

        private int? selectedGroupId = null;
        private int? selectedStudentId = null;
        private string selectedLastName = null;
        private string selectedFirstName = null;
        private string selectedMiddleName = null;

        private void Form4_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadGradesIntoComboBox();
            LoadGroupsIntoComboBox();
            LoadSubjectsIntoComboBox(); // Загружаем все или фильтрованные предметы
            LoadStudentLastNamesIntoComboBox();
            LoadStudentFirstNamesIntoComboBox();
            LoadStudentMiddleNamesIntoComboBox();
        }

        private void LoadGradesIntoComboBox()
        {
            comboBox_grade.Items.Clear();
            for (int i = 1; i <= 10; i++)
                comboBox_grade.Items.Add(i.ToString());
            comboBox_grade.Items.Add("Н");
        }

        private void LoadGroupsIntoComboBox()
        {
            string query = "SELECT group_id, group_name FROM groups";
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                MessageBox.Show("Соединение с БД не установлено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                using (var cmd = new NpgsqlCommand(query, _connection))
                using (var reader = cmd.ExecuteReader())
                {
                    DataTable table = new DataTable();
                    table.Load(reader);
                    comboBox_group_id.DataSource = null;
                    comboBox_group_id.DisplayMember = "group_name";
                    comboBox_group_id.ValueMember = "group_id";
                    comboBox_group_id.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки групп: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSubjectsIntoComboBox()
        {
            string query = @"
                SELECT s.id, s.subjects_name 
                FROM public.subjects s
                INNER JOIN public.groups g ON s.specialities_id = g.specialty_id
                WHERE g.group_id = @groupId";

            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                MessageBox.Show("Соединение с БД не установлено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    if (selectedGroupId.HasValue)
                        cmd.Parameters.AddWithValue("groupId", selectedGroupId.Value);
                    else
                        cmd.Parameters.AddWithValue("groupId", DBNull.Value);

                    DataTable table = new DataTable();
                    using (var reader = cmd.ExecuteReader())
                    {
                        table.Load(reader);
                    }

                    comboBox_subject.DataSource = null;
                    comboBox_subject.DisplayMember = "subjects_name";
                    comboBox_subject.ValueMember = "id";
                    comboBox_subject.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки предметов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStudentLastNamesIntoComboBox()
        {
            string query = "SELECT DISTINCT last_name, id FROM public.students WHERE TRUE";
            if (selectedGroupId.HasValue)
                query += " AND group_id = @groupId";
            query += " ORDER BY last_name, id";

            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                MessageBox.Show("Соединение с БД не установлено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    if (selectedGroupId.HasValue)
                        cmd.Parameters.AddWithValue("groupId", selectedGroupId.Value);

                    DataTable table = new DataTable();
                    using (var reader = cmd.ExecuteReader())
                        table.Load(reader);

                    comboBox_last_name.DataSource = null;
                    comboBox_last_name.Items.Clear();
                    comboBox_last_name.DisplayMember = "last_name";
                    comboBox_last_name.ValueMember = "id";
                    comboBox_last_name.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки фамилий студентов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStudentFirstNamesIntoComboBox()
        {
            string query = "SELECT DISTINCT first_name, id FROM public.students WHERE TRUE";
            if (selectedGroupId.HasValue)
                query += " AND group_id = @groupId";
            if (!string.IsNullOrEmpty(selectedLastName))
                query += " AND last_name = @lastName";
            query += " ORDER BY first_name, id";

            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                MessageBox.Show("Соединение с БД не установлено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    if (selectedGroupId.HasValue)
                        cmd.Parameters.AddWithValue("groupId", selectedGroupId.Value);
                    if (!string.IsNullOrEmpty(selectedLastName))
                        cmd.Parameters.AddWithValue("lastName", selectedLastName);

                    DataTable table = new DataTable();
                    using (var reader = cmd.ExecuteReader())
                        table.Load(reader);

                    comboBox_first_name.DataSource = null;
                    comboBox_first_name.Items.Clear();
                    comboBox_first_name.DisplayMember = "first_name";
                    comboBox_first_name.ValueMember = "id";
                    comboBox_first_name.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки имён студентов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStudentMiddleNamesIntoComboBox()
        {
            string query = "SELECT DISTINCT middle_name, id FROM public.students WHERE middle_name IS NOT NULL AND middle_name <> ''";
            if (selectedGroupId.HasValue)
                query += " AND group_id = @groupId";
            if (!string.IsNullOrEmpty(selectedLastName))
                query += " AND last_name = @lastName";
            if (!string.IsNullOrEmpty(selectedFirstName))
                query += " AND first_name = @firstName";
            query += " ORDER BY middle_name, id";

            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                MessageBox.Show("Соединение с БД не установлено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    if (selectedGroupId.HasValue)
                        cmd.Parameters.AddWithValue("groupId", selectedGroupId.Value);
                    if (!string.IsNullOrEmpty(selectedLastName))
                        cmd.Parameters.AddWithValue("lastName", selectedLastName);
                    if (!string.IsNullOrEmpty(selectedFirstName))
                        cmd.Parameters.AddWithValue("firstName", selectedFirstName);

                    DataTable table = new DataTable();
                    using (var reader = cmd.ExecuteReader())
                        table.Load(reader);

                    comboBox_middle_name.DataSource = null;
                    comboBox_middle_name.Items.Clear();
                    comboBox_middle_name.DisplayMember = "middle_name";
                    comboBox_middle_name.ValueMember = "id";
                    comboBox_middle_name.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки отчеств студентов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox_group_id_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_group_id.SelectedValue != null &&
                int.TryParse(comboBox_group_id.SelectedValue.ToString(), out int groupId))
            {
                selectedGroupId = groupId;
            }
            else
            {
                selectedGroupId = null;
            }

            // Очистка зависимых полей
            selectedLastName = null;
            selectedFirstName = null;
            selectedMiddleName = null;
            selectedStudentId = null;

            // Перезагрузка данных
            LoadStudentLastNamesIntoComboBox();
            LoadStudentFirstNamesIntoComboBox();
            LoadStudentMiddleNamesIntoComboBox();

            // Динамически обновляем список предметов
            LoadSubjectsIntoComboBox();

            // Сброс выбранных значений в ComboBox'ах
            comboBox_first_name.DataSource = null;
            comboBox_first_name.Items.Clear();
            comboBox_middle_name.DataSource = null;
            comboBox_middle_name.Items.Clear();
        }

        private void comboBox_last_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_last_name.SelectedItem != null)
            {
                var row = (DataRowView)comboBox_last_name.SelectedItem;
                selectedLastName = row["last_name"].ToString();
                selectedStudentId = Convert.ToInt32(row["id"]);
            }
            else
            {
                selectedLastName = null;
                selectedStudentId = null;
            }

            LoadStudentFirstNamesIntoComboBox();
            LoadStudentMiddleNamesIntoComboBox();
        }

        private void comboBox_first_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_first_name.SelectedItem != null)
            {
                var row = (DataRowView)comboBox_first_name.SelectedItem;
                selectedFirstName = row["first_name"].ToString();
                selectedStudentId = Convert.ToInt32(row["id"]);
            }
            else
            {
                selectedFirstName = null;
                selectedStudentId = null;
            }

            LoadStudentMiddleNamesIntoComboBox();
        }

        private void comboBox_middle_name_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_middle_name.SelectedItem != null)
            {
                var row = (DataRowView)comboBox_middle_name.SelectedItem;
                selectedMiddleName = row["middle_name"].ToString();
                selectedStudentId = Convert.ToInt32(row["id"]);
            }
            else
            {
                selectedMiddleName = null;
                selectedStudentId = null;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                MessageBox.Show("Соединение с БД не установлено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBox_subject.SelectedValue == null)
            {
                MessageBox.Show("Выберите предмет.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!selectedStudentId.HasValue)
            {
                MessageBox.Show("Выберите студента.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox_grade.SelectedItem == null)
            {
                MessageBox.Show("Выберите оценку.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox_group_id.SelectedValue == null)
            {
                MessageBox.Show("Выберите группу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int subjectId = Convert.ToInt32(comboBox_subject.SelectedValue);
            int studentId = selectedStudentId.Value;
            string gradeStr = comboBox_grade.SelectedItem.ToString();
            DateTime gradeDate = dateTimePicker1.Value;
            short? gradeValue = null;

            if (gradeStr != "Н")
            {
                if (!short.TryParse(gradeStr, out short parsedGrade))
                {
                    MessageBox.Show("Некорректное значение оценки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                gradeValue = parsedGrade;
            }

            string query = @"
                INSERT INTO public.grades (subject_id, group_id, student_id, grade_date, grade)
                VALUES (@subjectId, @groupId, @studentId, @gradeDate, @grade)";

            try
            {
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@subjectId", NpgsqlDbType.Bigint, subjectId);
                    cmd.Parameters.AddWithValue("@groupId", NpgsqlDbType.Bigint, selectedGroupId.Value);
                    cmd.Parameters.AddWithValue("@studentId", NpgsqlDbType.Bigint, studentId);
                    cmd.Parameters.AddWithValue("@gradeDate", NpgsqlDbType.Date, gradeDate);
                    cmd.Parameters.AddWithValue("@grade", gradeValue.HasValue
                        ? (object)gradeValue.Value
                        : (object)DBNull.Value);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        MessageBox.Show("Оценка успешно добавлена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Не удалось добавить запись.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении оценки:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Close();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Unused Events

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void comboBox_grade_SelectedIndexChanged(object sender, EventArgs e) { }
        private void comboBox_subject_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e) { }

        #endregion
    }
}