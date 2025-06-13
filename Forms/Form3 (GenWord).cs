using Microsoft.Office.Interop.Word;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Word = Microsoft.Office.Interop.Word;

namespace StudySync.Forms
{
    public partial class Form3 : Form
    {
        private NpgsqlConnection _connection;
        public Form3(NpgsqlConnection connection)
        {
            InitializeComponent();
            _connection = connection;
        }

        public Form3() : this(null) { }

        private int? selectedGroupId = null;
        private int? selectedStudentId = null;
        private string selectedLastName = null;
        private string selectedFirstName = null;
        private string selectedMiddleName = null;

        private void Form3_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadGroupsIntoComboBox();
            LoadStudentLastNamesIntoComboBox();
            LoadStudentFirstNamesIntoComboBox();
            LoadStudentMiddleNamesIntoComboBox();
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
                    System.Data.DataTable table = new System.Data.DataTable();
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

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void guna2ButtonReview_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void guna2ButtonGenerateWord_Click(object sender, EventArgs e)
        {
            // Проверка выбора всех данных студента
            if (string.IsNullOrEmpty(selectedLastName) ||
                string.IsNullOrEmpty(selectedFirstName) ||
                !selectedGroupId.HasValue ||
                !selectedStudentId.HasValue)
            {
                MessageBox.Show("Пожалуйста, выберите все данные студента: группу, фамилию, имя и отчество.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Получаем список предметов для группы студента
                string subjectsQuery = @"
            SELECT s.id, s.subjects_name 
            FROM public.subjects s
            INNER JOIN public.groups g ON s.specialities_id = g.speciality_id
            WHERE g.group_id = @groupId
            ORDER BY s.subjects_name";

                // Получаем оценки студента по всем предметам
                string gradesQuery = @"
            SELECT g.subject_id, g.grade, g.grade_date
            FROM public.grades g
            WHERE g.student_id = @studentId
            ORDER BY g.grade_date";

                DataTable subjectsTable = new DataTable();
                DataTable gradesTable = new DataTable();

                using (var cmd = new NpgsqlCommand(subjectsQuery, _connection))
                {
                    cmd.Parameters.AddWithValue("groupId", selectedGroupId.Value);
                    using (var reader = cmd.ExecuteReader())
                    {
                        subjectsTable.Load(reader);
                    }
                }

                using (var cmd = new NpgsqlCommand(gradesQuery, _connection))
                {
                    cmd.Parameters.AddWithValue("studentId", selectedStudentId.Value);
                    using (var reader = cmd.ExecuteReader())
                    {
                        gradesTable.Load(reader);
                    }
                }

                // Создаем словарь для хранения оценок по предметам
                Dictionary<int, List<string>> subjectGrades = new Dictionary<int, List<string>>();
                foreach (DataRow subjectRow in subjectsTable.Rows)
                {
                    int subjectId = Convert.ToInt32(subjectRow["id"]);
                    subjectGrades[subjectId] = new List<string>();
                }

                // Заполняем оценки
                foreach (DataRow gradeRow in gradesTable.Rows)
                {
                    int subjectId = Convert.ToInt32(gradeRow["subject_id"]);
                    if (subjectGrades.ContainsKey(subjectId))
                    {
                        object gradeValue = gradeRow["grade"];
                        string gradeStr = gradeValue == DBNull.Value ? "Н" : gradeValue.ToString();
                        subjectGrades[subjectId].Add(gradeStr);
                    }
                }

                // Диалог сохранения файла
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Word Document (*.docx)|*.docx";
                saveDialog.FileName = $"Табель_{selectedLastName}_{selectedFirstName}_{selectedGroupId}.docx";

                if (saveDialog.ShowDialog() != DialogResult.OK)
                {
                    return; // Пользователь отменил сохранение
                }

                // Создаем приложение Word
                var wordApp = new Microsoft.Office.Interop.Word.Application
                {
                    Visible = true
                };

                // Создаем новый документ
                var doc = wordApp.Documents.Add();

                // Заголовок
                var titleParagraph = doc.Content.Paragraphs.Add();
                titleParagraph.Range.Text = "СВЕДЕНИЯ ОБ УСПЕВАЕМОСТИ";
                titleParagraph.Range.Font.Bold = 1;
                titleParagraph.Range.Font.Size = 16;
                titleParagraph.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                titleParagraph.SpaceAfter = 20;
                titleParagraph.Range.InsertParagraphAfter();

                // ФИО
                var nameParagraph = doc.Content.Paragraphs.Add();
                nameParagraph.Range.Text = $"ученика(цы) {selectedLastName} {selectedFirstName} {selectedMiddleName}, группы «{comboBox_group_id.Text}»";
                nameParagraph.Range.Font.Size = 14;
                nameParagraph.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                nameParagraph.SpaceAfter = 10;
                nameParagraph.Range.InsertParagraphAfter();

                // Таблица
                var tableParagraph = doc.Content.Paragraphs.Add();
                tableParagraph.Range.Text = "";
                tableParagraph.Range.InsertParagraphAfter();

                // Создаем таблицу (количество предметов + 1 строка заголовка) × 12 столбцов
                int rowCount = subjectsTable.Rows.Count + 1;
                var table = doc.Tables.Add(tableParagraph.Range, rowCount, 12);
                table.Borders.Enable = 1;

                // Настройка ширины столбцов
                table.Columns[1].Width = 120;  // Ширина колонки "Предмет" (в пунктах)
                table.Columns[12].Width = 80; // Ширина колонки "Средний балл"
                for (int col = 2; col <= 11; col++)
                {
                    table.Columns[col].Width = 30;  // Ширина колонок с оценками
                }

                // Шапка таблицы
                // Первая колонка — "Предмет"
                table.Cell(1, 1).Range.Text = "Предмет";
                table.Cell(1, 1).Range.Paragraphs[1].Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                table.Cell(1, 1).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                // Последний столбец (12) — "Средний балл"
                table.Cell(1, 12).Range.Text = "Средний\nбалл";
                table.Cell(1, 12).Range.Paragraphs[1].Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                table.Cell(1, 12).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                // Объединяем столбцы 2–11 в одну ячейку "Оценки"
                table.Cell(1, 2).Merge(table.Cell(1, 11));
                table.Cell(1, 2).Range.Text = "Оценки";
                table.Cell(1, 2).Range.Paragraphs[1].Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                table.Cell(1, 2).VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                // Форматирование шапки
                table.Rows[1].Range.Font.Bold = 1;
                table.Rows[1].Shading.BackgroundPatternColor = WdColor.wdColorGray25;

                // Заполняем данные по предметам
                for (int i = 0; i < subjectsTable.Rows.Count; i++)
                {
                    DataRow subjectRow = subjectsTable.Rows[i];
                    int subjectId = Convert.ToInt32(subjectRow["id"]);
                    string subjectName = subjectRow["subjects_name"].ToString();

                    // Название предмета
                    table.Cell(i + 2, 1).Range.Text = subjectName;
                    table.Cell(i + 2, 1).Range.Paragraphs[1].Alignment = WdParagraphAlignment.wdAlignParagraphLeft;

                    // Оценки
                    if (subjectGrades.ContainsKey(subjectId) && subjectGrades[subjectId].Count > 0)
                    {
                        // Заполняем оценки (максимум 10 оценок)
                        int maxGrades = Math.Min(subjectGrades[subjectId].Count, 10);
                        for (int j = 0; j < maxGrades; j++)
                        {
                            table.Cell(i + 2, j + 2).Range.Text = subjectGrades[subjectId][j];
                        }

                        // Вычисляем средний балл (игнорируем "Н")
                        double sum = 0;
                        int count = 0;
                        foreach (string grade in subjectGrades[subjectId])
                        {
                            if (grade != "Н" && int.TryParse(grade, out int numericGrade))
                            {
                                sum += numericGrade;
                                count++;
                            }
                        }

                        if (count > 0)
                        {
                            double average = sum / count;
                            table.Cell(i + 2, 12).Range.Text = average.ToString("0.00");
                        }
                        else
                        {
                            table.Cell(i + 2, 12).Range.Text = "-";
                        }
                    }
                    else
                    {
                        table.Cell(i + 2, 12).Range.Text = "-";
                    }

                    // Центрируем оценки и средний балл
                    for (int col = 2; col <= 12; col++)
                    {
                        table.Cell(i + 2, col).Range.Paragraphs[1].Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    }
                }

                // Пустой абзац для отступа после таблицы
                var spacingParagraph = doc.Content.Paragraphs.Add();
                spacingParagraph.Range.Text = "";
                spacingParagraph.SpaceAfter = 10;
                spacingParagraph.Range.InsertParagraphAfter();

                // Подписи
                var signParagraph = doc.Content.Paragraphs.Add();
                signParagraph.Range.Text = "Подпись классного руководителя: ____________________________";
                signParagraph.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                signParagraph.Format.SpaceAfter = 0;
                signParagraph.Range.InsertParagraphAfter();

                signParagraph = doc.Content.Paragraphs.Add();
                signParagraph.Range.Text = "Подпись родителей: _________________________________";
                signParagraph.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                signParagraph.Format.SpaceAfter = 0;
                signParagraph.Range.InsertParagraphAfter();

                // Сохранение документа
                doc.SaveAs2(saveDialog.FileName);
                MessageBox.Show("Документ успешно создан!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании документа: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}