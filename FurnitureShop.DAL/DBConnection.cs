using Microsoft.Data.SqlClient;

namespace FurnitureShop.DAL
{
    public class DBConnection
    {
        private readonly string _connectionString;

        public DBConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}