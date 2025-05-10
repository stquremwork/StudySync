using System;
using System.Data;
using System.Linq;
using Npgsql;

namespace Kursach.Helpers
{
    public static class DatabaseHelper
    {
        public static DataTable GetTableData(string tableName, string connectionString)
        {
            DataTable dataTable = new DataTable();
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"SELECT * FROM {tableName}", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        dataTable.Load(reader);
                    }
                }
            }
            return dataTable;
        }

        public static void UpdateRow(NpgsqlConnection conn, DataRow row, string tableName, NpgsqlTransaction transaction)
        {
            string setClause = string.Join(", ", row.Table.Columns.Cast<DataColumn>()
                .Where(c => !c.ColumnName.Equals("id", StringComparison.OrdinalIgnoreCase))
                .Select(c => $"{c.ColumnName} = @{c.ColumnName}"));

            string query = $"UPDATE {tableName} SET {setClause} WHERE id = @id";

            using (var cmd = new NpgsqlCommand(query, conn, transaction))
            {
                foreach (DataColumn column in row.Table.Columns)
                {
                    cmd.Parameters.AddWithValue(column.ColumnName, row[column]);
                }
                cmd.ExecuteNonQuery();
            }
        }

        public static void InsertRow(NpgsqlConnection conn, DataRow row, string tableName, NpgsqlTransaction transaction)
        {
            string columns = string.Join(", ", row.Table.Columns.Cast<DataColumn>()
                .Where(c => !c.ColumnName.Equals("id", StringComparison.OrdinalIgnoreCase))
                .Select(c => c.ColumnName));

            string values = string.Join(", ", row.Table.Columns.Cast<DataColumn>()
                .Where(c => !c.ColumnName.Equals("id", StringComparison.OrdinalIgnoreCase))
                .Select(c => $"@{c.ColumnName}"));

            string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values}) RETURNING id";

            using (var cmd = new NpgsqlCommand(query, conn, transaction))
            {
                foreach (DataColumn column in row.Table.Columns)
                {
                    if (!column.ColumnName.Equals("id", StringComparison.OrdinalIgnoreCase))
                    {
                        cmd.Parameters.AddWithValue(column.ColumnName, row[column]);
                    }
                }

                var newId = cmd.ExecuteScalar();
                if (newId != null)
                {
                    row["id"] = newId;
                }
            }
        }

        public static void DeleteRow(NpgsqlConnection conn, DataRow row, string tableName, NpgsqlTransaction transaction)
        {
            string query = $"DELETE FROM {tableName} WHERE id = @id";

            using (var cmd = new NpgsqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("id", row["id", DataRowVersion.Original]);
                cmd.ExecuteNonQuery();
            }
        }
    }
}