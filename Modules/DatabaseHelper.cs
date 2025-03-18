using Npgsql;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Kursach.Helpers
{
    public static class DatabaseHelper
    {
        public static string GetPrimaryKeyName(NpgsqlConnection conn, string tableName)
        {
            string query = @"
                SELECT a.attname
                FROM pg_index i
                JOIN pg_attribute a ON a.attrelid = i.indrelid AND a.attnum = ANY(i.indkey)
                WHERE i.indrelid = @tableName::regclass AND i.indisprimary;";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@tableName", tableName);
                object result = cmd.ExecuteScalar();
                if (result == null)
                {
                    throw new Exception($"Primary key not found for table '{tableName}'.");
                }
                return result.ToString();
            }
        }

        public static DataTable GetTableData(string tableName, string connectionString)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $"SELECT * FROM {tableName};";

                    using (var dataAdapter = new NpgsqlDataAdapter(query, conn))
                    {
                        dataAdapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dataTable;
        }

        public static void UpdateRow(NpgsqlConnection conn, DataRow row, string tableName, NpgsqlTransaction transaction)
        {
            StringBuilder setClause = new StringBuilder();
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            string primaryKey = GetPrimaryKeyName(conn, tableName); // Get primary key name

            foreach (DataColumn column in row.Table.Columns)
            {
                if (!row[column.ColumnName].Equals(row[column.ColumnName, DataRowVersion.Original]))
                {
                    setClause.Append($"{column.ColumnName} = @{column.ColumnName}, ");
                    parameters.Add(new NpgsqlParameter($"@{column.ColumnName}", row[column.ColumnName]));
                }
            }

            if (setClause.Length > 0)
            {
                setClause.Length -= 2; // Remove trailing comma

                string query = $"UPDATE {tableName} SET {setClause.ToString()} WHERE {primaryKey} = @{primaryKey}";
                parameters.Add(new NpgsqlParameter($"@{primaryKey}", row[primaryKey]));

                using (var cmd = new NpgsqlCommand(query, conn, transaction))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());

                    Console.WriteLine($"Executing UPDATE query: {cmd.CommandText}");
                    foreach (NpgsqlParameter param in cmd.Parameters)
                    {
                        Console.WriteLine($"Parameter: {param.ParameterName} = {param.Value}");
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"Row with primary key '{row[primaryKey]}' not found.");
                    }
                }
            }
        }

        public static void InsertRow(NpgsqlConnection conn, DataRow row, string tableName, NpgsqlTransaction transaction)
        {
            StringBuilder columns = new StringBuilder();
            StringBuilder values = new StringBuilder();
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            foreach (DataColumn column in row.Table.Columns)
            {
                columns.Append($"{column.ColumnName}, ");
                values.Append($"@{column.ColumnName}, ");
                parameters.Add(new NpgsqlParameter($"@{column.ColumnName}", row[column.ColumnName]));
            }

            if (columns.Length > 0 && values.Length > 0)
            {
                columns.Length -= 2; // Remove trailing comma
                values.Length -= 2; // Remove trailing comma

                string query = $"INSERT INTO {tableName} ({columns.ToString()}) VALUES ({values.ToString()})";
                using (var cmd = new NpgsqlCommand(query, conn, transaction))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());

                    Console.WriteLine($"Executing INSERT query: {cmd.CommandText}");
                    foreach (NpgsqlParameter param in cmd.Parameters)
                    {
                        Console.WriteLine($"Parameter: {param.ParameterName} = {param.Value}");
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteRow(NpgsqlConnection conn, DataRow row, string tableName, NpgsqlTransaction transaction)
        {
            string primaryKey = GetPrimaryKeyName(conn, tableName); // Get primary key name

            string query = $"DELETE FROM {tableName} WHERE {primaryKey} = @{primaryKey}";
            using (var cmd = new NpgsqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue($"@{primaryKey}", row[primaryKey, DataRowVersion.Original]);

                Console.WriteLine($"Executing DELETE query: {cmd.CommandText}");
                foreach (NpgsqlParameter param in cmd.Parameters)
                {
                    Console.WriteLine($"Parameter: {param.ParameterName} = {param.Value}");
                }

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    throw new Exception($"Row with primary key '{row[primaryKey, DataRowVersion.Original]}' not found.");
                }
            }
        }
    }
}
