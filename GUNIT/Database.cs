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
        public static void OpretKunde()
        {
            Console.Write("Indtast navn på ny kunde: ");
            string navn = Console.ReadLine();
            Console.Write("Indtast CPR-nr: ");
            string CPRString = Console.ReadLine();
            int CPR = int.Parse(CPRString.Replace("-", "").Replace("/", ""));
            string SQLSend = "INSERT INTO Kunde values('" + navn + "', GetDate(), '', "+CPR+")";
            SQLkommando(SQLSend);
        }

        public static void FindKunde(string valg)
        {

            switch (valg)
            {
                case "1":
                    Console.WriteLine("Indtast søgning på kunde: ");
                    string str = Console.ReadLine();
                    string SQLSend = "";
                    SQLkommando(str);

                    break;

                case "2":
                    Console.WriteLine("Indtast søgning på konto: ");
                    str = Console.ReadLine();
                    SQLSend = "";
                    SQLkommando(str);

                    break;

                case "3":
                    Console.WriteLine("Indtast søgning på kundenummer: ");
                    str = Console.ReadLine();
                    SQLSend = "";
                    SQLkommando(str);

                    break;

                default:
                    break;
            }

        }

        public static void SletKunde()
        {
            Console.Write("Indtast CPR-nr på kunde der skal slettes: ");
            string CPRString = Console.ReadLine();
            int CPR = int.Parse(CPRString.Replace("-", ""));
            string SQLSend = "UPDATE Kunde set kundeslutdato = GetDate() where CPR = " + CPR + ";";
            SQLkommando(SQLSend);
            //Database.SQLkommando("INSERT INTO Kunde values('frede', '2018-01-17 08:11:45')");        

        }



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
