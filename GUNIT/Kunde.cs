using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUNIT
{
    class Kunde
    {
        static string[] kundenavn;
        static int[] PK_kundenr, CPR;
        static DateTime?[] kundedato, kundeslutdato;
        static string SQLSend;

        public static void OpretKunde()
        {
            Console.Write("Indtast navn på ny kunde: ");
            string navn = Console.ReadLine();
            Console.Write("Indtast CPR-nr: ");
            string CPRString = Console.ReadLine();
            int CPR = int.Parse(CPRString.Replace("-", "").Replace("/", ""));
            string SQLSend = "INSERT INTO Kunde values('" + navn + "', GetDate(), '', " + CPR + ")";
            Database.SQLkommando(SQLSend);

            string SQLGet = "SELECT PK_kundenr from Kunde where CPR = " + CPR + ";";
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
                    Console.Write("Indtast søgning på kunde: ");
                    string str = Console.ReadLine();
                    SQLSend = "select * from Kunde where kundenavn like '%" + str + "%'";
                    break;

                case "2":
                    Console.Write("Indtast søgning på konto: ");
                    str = Console.ReadLine();
                    SQLSend = "select PK_kundenr, kundenavn, kundedato, kundeslutdato, CPR from Konto, Kunde where PK_kontonr = '" + str + "' and PK_kundenr = FK_kundenr";
                    //SQLSend = "select PK_kundenr, kundenavn, kundedato, kundeslutdato, CPR from Konto, Kunde where PK_kontonr like '%" + str + "%'";
                    break;

                case "3":
                    Console.Write("Indtast søgning på kundenummer: ");
                    str = Console.ReadLine();
                    SQLSend = "select * from Kunde where PK_kundenr like '%" + str + "%'";
                    break;

                case "4":
                    Console.Write("Indtast søgning på CPR nr: ");
                    str = Console.ReadLine();
                    SQLSend = "select * from Kunde where CPR like '%" + str + "%'";
                    break;

                default:
                    break;

            }

            string[] SQLData = Database.SQLkommandoGet(SQLSend);
            //Console.WriteLine(SQLData.Length);
            int count = 0;
            for (int i = 0; i < SQLData.Length; i += 5)
            {
                Array.Resize(ref PK_kundenr, count + 1);
                Array.Resize(ref kundenavn, count + 1);
                Array.Resize(ref kundedato, count + 1);
                Array.Resize(ref kundeslutdato, count + 1);
                Array.Resize(ref CPR, count + 1);

                PK_kundenr[count] = int.Parse(SQLData[i]);
                kundenavn[count] = SQLData[i + 1];
                kundedato[count] = Convert.ToDateTime(SQLData[i + 2]);
                if (SQLData[i + 3] != "")
                { kundeslutdato[count] = Convert.ToDateTime(SQLData[i + 3]); }

                CPR[count] = int.Parse(SQLData[i + 4]);
                count++;
            }

            if (SQLData.Length != 0)
            {
                for (int i = 0; i < PK_kundenr.Length; i++)
                {
                    Console.WriteLine(i);
                    Console.WriteLine("Kundenr: {0} Kundenavn: {1} Oprettelsesdato: {2} Slutdato {3} CPR {4}", PK_kundenr[i], kundenavn[i], kundedato[i], kundeslutdato[i], CPR[i]);

                }
            }
            else
            {
                Console.WriteLine("Ingen resultater fundet.");
            }
            Console.ReadLine();

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
