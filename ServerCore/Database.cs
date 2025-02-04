using System.Data;
using MySql.Data.MySqlClient;

namespace ServerCore
{
    public class Database
    {
        private static string DBIp = "localhost";
        private static string DBPort = "3306";
        private static string DBName = "chat_program";
        private static string DBUser = "root";
        private static string DBPassword = "root1234";
        private static string connStr = $"Server={DBIp};Port={DBPort};Database={DBName};Uid={DBUser};Pwd={DBPassword};";

        public MySqlConnection conn;


        public void Open()
        {
            try
            {
                if (conn == null)
                {
                    conn = new MySqlConnection(connStr);
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connect Failed {ex}");
            }
        }

        public void Close()
        {
            try
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database disconnect Failed {ex}");
            }
        }

        public void CUDQuery(string sql)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql,conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database CUD Failed {ex}");
            }
        }

    }
}
