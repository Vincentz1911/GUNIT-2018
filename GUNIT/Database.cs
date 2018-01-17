using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUNIT
{
    public static class Database
    {
        public static void SQLkommando(string sqltext)
        {
            string ConnectionString = @"Data Source = skab3-pc-04; Initial Catalog = GUNIT; Integrated Security = True";
            var connection = new SqlConnection(ConnectionString);
            SqlCommand cmd;
            connection.Open();

            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = sqltext;
                cmd.ExecuteNonQuery();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}
