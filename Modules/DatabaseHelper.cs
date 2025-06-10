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
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var adapter = new NpgsqlDataAdapter($"SELECT * FROM {tableName}", conn))
                {
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        public static void InsertRow(NpgsqlConnection conn, DataRow row, string tableName, NpgsqlTransaction transaction)
        {
            var columns = row.Table.Columns.Cast<DataColumn>()
                .Where(c => c.ColumnName != "id") // Предполагаем, что id автоинкрементный
                .Select(c => $"\"{c.ColumnName}\"");
            var parameters = row.Table.Columns.Cast<DataColumn>()
                .Where(c => c.ColumnName != "id")
                .Select(c => $"@p{c.Ordinal}");

            string sql = $"INSERT INTO {tableName} ({string.Join(",", columns)}) VALUES ({string.Join(",", parameters)})";

            using (var cmd = new NpgsqlCommand(sql, conn, transaction))
            {
                foreach (DataColumn col in row.Table.Columns)
                {
                    if (col.ColumnName != "id")
                    {
                        cmd.Parameters.AddWithValue($"@p{col.Ordinal}", row[col] == DBNull.Value ? null : row[col]);
                    }
                }
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateRow(NpgsqlConnection conn, DataRow row, string tableName, NpgsqlTransaction transaction)
        {
            var setClause = row.Table.Columns.Cast<DataColumn>()
                .Where(c => c.ColumnName != "id")
                .Select(c => $"\"{c.ColumnName}\" = @p{c.Ordinal}");

            string sql = $"UPDATE {tableName} SET {string.Join(",", setClause)} WHERE id = @id";

            using (var cmd = new NpgsqlCommand(sql, conn, transaction))
            {
                foreach (DataColumn col in row.Table.Columns)
                {
                    if (col.ColumnName != "id")
                    {
                        cmd.Parameters.AddWithValue($"@p{col.Ordinal}", row[col] == DBNull.Value ? null : row[col]);
                    }
                }
                cmd.Parameters.AddWithValue("@id", row["id"]);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteRow(NpgsqlConnection conn, DataRow row, string tableName, NpgsqlTransaction transaction)
        {
            string sql = $"DELETE FROM {tableName} WHERE id = @id";
            using (var cmd = new NpgsqlCommand(sql, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@id", row["id", DataRowVersion.Original]);
                cmd.ExecuteNonQuery();
            }
        }
    }
}