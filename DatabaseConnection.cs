using System;
using System.Data;
using System.Data.SqlClient;

namespace Kursach
{
    public class DatabaseConnection
    {
        public string ConnectionString { get; private set; }

        public bool TestConnection(string host, string port, string database, string user, string password)
        {
            ConnectionString = $"Server={host},{port};Database={database};User Id={user};Password={password};";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataTable GetTableList(bool showAll)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = showAll ? "SELECT table_schema, table_name FROM information_schema.tables" : "SELECT table_schema, table_name FROM information_schema.tables WHERE table_schema = 'public'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    return dataTable;
                }
            }
        }

        public DataTable GetTableData(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = $"SELECT * FROM {tableName}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    return dataTable;
                }
            }
        }
    }
}