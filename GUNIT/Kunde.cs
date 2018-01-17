using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUNIT
{
    class Kunde
    {

        public static void OpretKunde()
        {
            Console.Write("Indtast navn på ny kunde: ");
            string navn = Console.ReadLine();
            Console.Write("Indtast CPR-nr: ");
            string CPRString = Console.ReadLine();
            int CPR = int.Parse(CPRString.Replace("-", "").Replace("/", ""));
            string SQLSend = "INSERT INTO Kunde values('" + navn + "', GetDate(), '', " + CPR + ")";
            Database.SQLkommando(SQLSend);

            string SQLGet = "SELECT PK_kundenr from Kunde where CPR = "+ CPR + ";";
            Database.SQLkommandoGet(SQLGet);
            //Console.WriteLine(SQLGet);
            //int Kundenr = int.Parse(SQLGet);
            //Console.WriteLine("Der er blevet oprettet en kunde med navnet {0}, CPR {1} og kundenr {2}", navn, CPR, Kundenr);
            Console.ReadKey();
            Console.Clear();
        }

        public static void FindKunde(string valg)
        {

            switch (valg)
            {
                case "1":
                    Console.WriteLine("Indtast søgning på kunde: ");
                    string str = Console.ReadLine();
                    string SQLSend = "";
                    Database.SQLkommando(str);

                    break;

                case "2":
                    Console.WriteLine("Indtast søgning på konto: ");
                    str = Console.ReadLine();
                    SQLSend = "";
                    Database.SQLkommando(str);

                    break;

                case "3":
                    Console.WriteLine("Indtast søgning på kundenummer: ");
                    str = Console.ReadLine();
                    SQLSend = "";
                    Database.SQLkommando(str);

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
            Database.SQLkommando(SQLSend);
            //Database.SQLkommando("INSERT INTO Kunde values('frede', '2018-01-17 08:11:45')");        

        }



    }
}
