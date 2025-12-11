conexion.cs
using MySql.Data.MySqlClient;

namespace EcommerceApi.BD
{
    public class Conexion
    {
        private readonly string connectionString;

        public Conexion(IConfiguration config)
        {
            connectionString = config.GetConnectionString("MySqlConnection");
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public bool ProbarConexion()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}