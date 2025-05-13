using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Kursach.Helpers
{
    public static class TabDataSaver
    {
        public static bool HasUnsavedChanges(TabPage tabPage)
        {
            var dgv = tabPage?.Controls.OfType<DataGridView>().FirstOrDefault();
            if (dgv?.DataSource == null) return false;

            dgv.EndEdit();
            if (!(dgv.DataSource is DataView dataView) || dataView.Table == null) return false;

            var changes = dataView.Table.GetChanges();
            return changes != null && changes.Rows.Count > 0;
        }

        public static void SaveTabData(TabPage tabPage, string connectionString)
        {
            var dgv = tabPage.Controls.OfType<DataGridView>().FirstOrDefault();
            if (!(dgv?.DataSource is DataView dataView) || dataView.Table == null)
                throw new InvalidOperationException("Нет данных для сохранения");

            dgv.EndEdit();
            string tableName = ParseTableName(tabPage.Text);

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var primaryKeys = DatabaseHelper.GetPrimaryKeys(tableName, conn);
                        var columns = DatabaseHelper.GetTableColumns(conn, tableName);

                        foreach (DataRow row in dataView.Table.Select(null, null, DataViewRowState.Deleted))
                            DatabaseHelper.DeleteRow(conn, row, tableName, transaction, primaryKeys);

                        foreach (DataRow row in dataView.Table.Select(null, null, DataViewRowState.Added | DataViewRowState.ModifiedCurrent))
                        {
                            if (row.RowState == DataRowState.Added)
                                DatabaseHelper.InsertRow(conn, row, tableName, transaction, columns);
                            else
                                DatabaseHelper.UpdateRow(conn, row, tableName, transaction, primaryKeys, columns);
                        }

                        transaction.Commit();
                        dataView.Table.AcceptChanges();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static string ParseTableName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Имя таблицы пустое");

            var parts = fullName.Trim().Replace("..", ".").Split('.');
            return parts.Length == 2 ? parts[1] : fullName;
        }
    }
}