using MySql.Data.MySqlClient;

namespace MISProject
{
    public class DatabaseHelper
    {
        private static string connectionString = @"Server=localhost;Port=3306;Database=employee_db;Uid=root; Pwd=testpassword;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}