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

        public static string[] SQLArray = new string[0];

        public static void SQLkommando(string sqltext)
        {
            string ConnectionString = @"Data Source = (local); Initial Catalog = GUNIT; Integrated Security = True";
            var connection = new SqlConnection(ConnectionString);
            SqlCommand cmd;
            connection.Open();

            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = sqltext;
                cmd.ExecuteNonQuery();
                //Console.ReadKey();
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

            string ConnectionString = @"Data Source = (local); Initial Catalog = GUNIT; Integrated Security = True";
            var connection = new SqlConnection(ConnectionString);
            //SqlCommand cmd;
            connection.Open();

            int count = 0;


            using (SqlCommand command = new SqlCommand(sqltext, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Array.Resize(ref SQLArray, count + 1); // Udvider arrayet med en pr. gennemgang af felter
                            SQLArray[count] = reader.GetValue(i).ToString();
                            //Console.Write(reader.GetValue(i));
                            count++;
                        }
                        //Console.WriteLine();
                    }
                }
            }

            {
                connection.Close();
            }


            return SQLArray;
        }
    }
}

