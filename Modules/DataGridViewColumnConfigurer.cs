using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;
using System.Collections.Generic;
using System.Globalization;

namespace Kursach.Helpers
{
    public static class DataGridViewColumnConfigurer
    {
        public static readonly Dictionary<string, string> ColumnNameTranslations = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"id_group", "Группа"},
            {"GroupName", "Группа"},
            {"last_name", "Фамилия"},
            {"first_name", "Имя"},
            {"middle_name", "Отчество"},
        };

        public static void ConfigureColumns(DataGridView dataGridView, string tableName, string connectionString)
        {
            try
            {
                if (tableName.Equals("public.grades", StringComparison.OrdinalIgnoreCase))
                {
                    ConfigureGradesTable(dataGridView, connectionString);
                }

                ConfigureBasicTableSettings(dataGridView);
                SetColumnOrder(dataGridView);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка настройки столбцов: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void ConfigureGradesTable(DataGridView dataGridView, string connectionString)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (ColumnNameTranslations.TryGetValue(column.Name, out string translated))
                {
                    column.HeaderText = translated;
                }
                else if (IsDateColumn(column.Name))
                {
                    column.HeaderText = column.Name.Replace('_', '.');
                }
                else
                {
                    column.HeaderText = ToTitleCase(column.Name);
                }
            }
        }

        private static void ConfigureBasicTableSettings(DataGridView dataGridView)
        {
            if (dataGridView.Columns.Contains("CheckBoxColumn"))
            {
                dataGridView.Columns["CheckBoxColumn"].DisplayIndex = 0;
            }

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Name != "CheckBoxColumn")
                {
                    if (ColumnNameTranslations.TryGetValue(column.Name, out string translatedName))
                    {
                        column.HeaderText = translatedName;
                    }
                    else if (IsDateColumn(column.Name))
                    {
                        column.HeaderText = column.Name.Replace('_', '.');
                    }
                    else
                    {
                        column.HeaderText = ToTitleCase(column.Name);
                    }
                }
            }
        }

        internal static void SetColumnOrder(DataGridView dataGridView)
        {
            if (dataGridView.Columns.Contains("CheckBoxColumn"))
            {
                dataGridView.Columns["CheckBoxColumn"].DisplayIndex = 0;
            }

            string[] columnOrder = { "GroupName", "id_group", "last_name", "first_name", "middle_name" };
            int displayIndex = 1;

            foreach (string columnName in columnOrder)
            {
                if (dataGridView.Columns.Contains(columnName))
                {
                    dataGridView.Columns[columnName].DisplayIndex = displayIndex++;
                }
            }

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (IsDateColumn(column.Name) && column.Visible)
                {
                    column.DisplayIndex = displayIndex++;
                }
            }
        }

        private static string ToTitleCase(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            text = text.Replace('_', ' ');
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(text.ToLower());
        }

        private static bool IsDateColumn(string columnName)
        {
            if (columnName.Length != 10) return false;
            if (columnName[2] != '_' || columnName[5] != '_') return false;

            try
            {
                _ = new DateTime(
                    int.Parse(columnName.Substring(6, 4)),
                    int.Parse(columnName.Substring(3, 2)),
                    int.Parse(columnName.Substring(0, 2)));
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static Tuple<string, string, string> GetStudentData(NpgsqlConnection conn, int studentId)
        {
            string query = "SELECT last_name, first_name, middle_name FROM public.students WHERE id = @StudentID";
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("StudentID", studentId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Tuple.Create(
                            reader["last_name"] != DBNull.Value ? reader["last_name"].ToString() : "",
                            reader["first_name"] != DBNull.Value ? reader["first_name"].ToString() : "",
                            reader["middle_name"] != DBNull.Value ? reader["middle_name"].ToString() : "");
                    }
                    return Tuple.Create("", "", "");
                }
            }
        }

        private static string GetGroupName(NpgsqlConnection conn, int groupId)
        {
            string query = "SELECT group_name FROM public.groups WHERE group_id = @GroupID";
            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("GroupID", groupId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader["group_name"] != DBNull.Value ?
                            reader["group_name"].ToString() :
                            $"Группа {groupId}";
                    }
                    return $"Группа {groupId}";
                }
            }
        }

        private static DataTable GetDataTableFromDataSource(DataGridView dataGridView)
        {
            if (dataGridView.DataSource is DataTable)
            {
                return (DataTable)dataGridView.DataSource;
            }
            else if (dataGridView.DataSource is DataView)
            {
                return ((DataView)dataGridView.DataSource).ToTable();
            }
            else if (dataGridView.DataSource is BindingSource)
            {
                var bs = (BindingSource)dataGridView.DataSource;
                if (bs.DataSource is DataTable)
                {
                    return (DataTable)bs.DataSource;
                }
            }

            throw new InvalidCastException("Неподдерживаемый тип DataSource");
        }
    }

    public class StudentGradesColumnConfigurer
    {
        public void ConfigureGradesTable(DataGridView dataGridView, List<Student> students, List<DateTime> dates)
        {
            dataGridView.Columns.Clear();
            dataGridView.Rows.Clear();

            dataGridView.Columns.Add("GroupName", DataGridViewColumnConfigurer.ColumnNameTranslations["GroupName"]);
            dataGridView.Columns.Add("last_name", DataGridViewColumnConfigurer.ColumnNameTranslations["last_name"]);
            dataGridView.Columns.Add("first_name", DataGridViewColumnConfigurer.ColumnNameTranslations["first_name"]);
            dataGridView.Columns.Add("middle_name", DataGridViewColumnConfigurer.ColumnNameTranslations["middle_name"]);

            foreach (DateTime date in dates)
            {
                string columnName = date.ToString("dd_MM_yyyy");
                string headerText = date.ToString("dd.MM.yyyy");
                dataGridView.Columns.Add(columnName, headerText);
            }

            foreach (Student student in students)
            {
                int rowIndex = dataGridView.Rows.Add();
                var row = dataGridView.Rows[rowIndex];

                row.Cells["GroupName"].Value = student.GroupName;
                row.Cells["last_name"].Value = student.LastName;
                row.Cells["first_name"].Value = student.FirstName;
                row.Cells["middle_name"].Value = student.MiddleName;

                foreach (var date in dates)
                {
                    string columnName = date.ToString("dd_MM_yyyy");
                    row.Cells[columnName].Value = GetGrade(student, date);
                }
            }

            DataGridViewColumnConfigurer.SetColumnOrder(dataGridView);
        }

        private object GetGrade(Student student, DateTime date)
        {
            // Реализация получения оценки
            return null;
        }
    }

    public class Student
    {
        public string GroupName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
    }
}
