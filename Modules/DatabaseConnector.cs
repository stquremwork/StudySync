using System;
using System.Data;
using Npgsql;

namespace Kursach
{
    public class DatabaseConnector
    {
        private string _connectionString;

        // Конструктор для инициализации строки подключения
        public DatabaseConnector(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Метод для тестирования подключения к базе данных
        public bool TestConnection(out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    return true; // Подключение успешно
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false; // Ошибка подключения
            }
        }

        // Метод для получения списка таблиц из базы данных
        public string[] GetTableList(bool onlyPublicSchema = true)
        {
            string query;
            if (onlyPublicSchema)
            {
                query = @"
                    SELECT table_schema, table_name
                    FROM information_schema.tables
                    WHERE table_type = 'BASE TABLE' AND table_schema = 'public'
                    ORDER BY table_schema, table_name;";
            }
            else
            {
                query = @"
                    SELECT table_schema, table_name
                    FROM information_schema.tables
                    WHERE table_type = 'BASE TABLE'
                    ORDER BY table_schema, table_name;";
            }

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    conn.Open();
                    var tables = new System.Collections.Generic.List<string>();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tableSchema = reader.GetString(0);
                            string tableName = reader.GetString(1);
                            tables.Add($"{tableSchema}.{tableName}");
                        }
                    }
                    return tables.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении списка таблиц: {ex.Message}", ex);
            }
        }

        // Метод для получения данных из таблицы
        public DataTable GetTableData(string tableName)
        {
            string query = $"SELECT * FROM {tableName};";

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                using (var adapter = new NpgsqlDataAdapter(query, conn))
                {
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при получении данных из таблицы {tableName}: {ex.Message}", ex);
            }
        }

        // Метод для получения строки подключения
        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}