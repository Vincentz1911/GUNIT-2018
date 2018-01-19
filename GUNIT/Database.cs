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
        public static void SQLkommandoSet(string sqltext)
        {
            string ConnectionString = @"Data Source = (LOCAL); Initial Catalog = GUNIT; Integrated Security = True";
            var connection = new SqlConnection(ConnectionString);
            SqlCommand cmd;
            connection.Open();

            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = sqltext;
                cmd.ExecuteNonQuery();
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

        public static string[] SQLkommandoGet(string sqltext)
        {
            string[] SQLArray = new string[0];
            string ConnectionString = @"Data Source = (LOCAL); Initial Catalog = GUNIT; Integrated Security = True";
            var connection = new SqlConnection(ConnectionString);

            connection.Open();
            using (SqlCommand command = new SqlCommand(sqltext, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    int count = 0;
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Array.Resize(ref SQLArray, count + 1); // Udvider arrayet med en pr. gennemgang af felter
                            SQLArray[count] = reader.GetValue(i).ToString();
                            count++;
                            //Console.Write(reader.GetValue(i));
                        }
                        //Console.WriteLine();
                    }
                }
            }
            connection.Close();
            return SQLArray;
        }
    }
}

