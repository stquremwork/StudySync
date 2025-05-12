using System;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace Kursach.Helpers
{
    public static class DataGridViewColumnConfigurer
    {
        public static void ConfigureColumns(DataGridView dataGridView, string tableName, string connectionString)
        {
            if (tableName == "public.grades")
            {
                DataTable originalTable;

                if (dataGridView.DataSource is DataTable dt)
                {
                    originalTable = dt;
                }
                else if (dataGridView.DataSource is DataView dv)
                {
                    originalTable = dv.Table;
                }
                else
                {
                    throw new InvalidCastException("DataSource is not a DataTable or DataView");
                }

                DataTable modifiedTable = originalTable.Clone();

                // Добавляем новые столбцы для Группы, Фамилии, Имени, Отчества
                modifiedTable.Columns.Add("Группа", typeof(string));
                modifiedTable.Columns.Add("Фамилия", typeof(string));
                modifiedTable.Columns.Add("Имя", typeof(string));
                modifiedTable.Columns.Add("Отчество", typeof(string));

                try
                {
                    using (var conn = new NpgsqlConnection(connectionString))
                    {
                        conn.Open();

                        foreach (DataRow row in originalTable.Rows)
                        {
                            DataRow newRow = modifiedTable.NewRow();

                            // Копируем все исходные данные
                            foreach (DataColumn col in originalTable.Columns)
                            {
                                newRow[col.ColumnName] = row[col.ColumnName];
                            }

                            int studentId = Convert.ToInt32(row["StudentID"]);
                            int groupId = Convert.ToInt32(row["GroupID"]);

                            // Получаем ФИО студента
                            string studentQuery = "SELECT last_name, first_name, middle_name FROM public.students WHERE id = @StudentID";
                            using (var cmd = new NpgsqlCommand(studentQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("StudentID", studentId);
                                using (var reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        newRow["Фамилия"] = reader["last_name"].ToString();
                                        newRow["Имя"] = reader["first_name"].ToString();
                                        newRow["Отчество"] = reader["middle_name"].ToString();
                                    }
                                    else
                                    {
                                        newRow["Фамилия"] = "Неизвестно";
                                        newRow["Имя"] = "Неизвестно";
                                        newRow["Отчество"] = "Неизвестно";
                                    }
                                }
                            }

                            // Получаем название группы
                            string groupQuery = "SELECT group_name FROM public.groups WHERE group_id = @GroupID";
                            using (var cmd = new NpgsqlCommand(groupQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("GroupID", groupId);
                                using (var reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        newRow["Группа"] = reader["group_name"].ToString();
                                    }
                                    else
                                    {
                                        newRow["Группа"] = $"Группа {groupId} (Неизвестно)";
                                    }
                                }
                            }

                            modifiedTable.Rows.Add(newRow);
                        }
                    }

                    dataGridView.DataSource = modifiedTable;

                    // Настраиваем видимость столбцов
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        if (column.Name == "Группа" ||
                            column.Name == "Фамилия" ||
                            column.Name == "Имя" ||
                            column.Name == "Отчество" ||
                            IsDateColumn(column))
                        {
                            column.Visible = true;
                        }
                        else
                        {
                            column.Visible = false;
                        }
                    }

                    // Настраиваем порядок столбцов
                    try
                    {
                        dataGridView.Columns["Группа"].DisplayIndex = 0;
                        dataGridView.Columns["Фамилия"].DisplayIndex = 1;
                        dataGridView.Columns["Имя"].DisplayIndex = 2;
                        dataGridView.Columns["Отчество"].DisplayIndex = 3;
                        // Колонки с датами идут далее автоматически

                        // Заменяем в заголовках колонок с датами символы '_' на '.'
                        foreach (DataGridViewColumn column in dataGridView.Columns)
                        {
                            if (IsDateColumn(column))
                            {
                                column.HeaderText = column.Name.Replace('_', '.');
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при настройке порядка столбцов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при получении данных из базы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static bool IsDateColumn(DataGridViewColumn column)
        {
            // Проверяем, что имя колонки соответствует формату дд_мм_гггг, например "10_05_2025"
            DateTime dt;
            return DateTime.TryParseExact(
                column.Name,
                "dd_MM_yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out dt);
        }

    }
}
