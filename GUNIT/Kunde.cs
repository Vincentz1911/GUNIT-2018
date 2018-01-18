using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUNIT
{
    class Kunde
    {
        static int[] PK_kundenr;
        static string[] kundenavn, CPR, SQLData;
        static DateTime?[] kundedato, kundeslutdato;
        static string SQLSend, CPRString;

        //******************* KUNDEMENU *******************
        static void KundeMenu(int kundenr)
        {
            Console.Clear();
            Console.WriteLine("***********\n*Kundemenu*\n***********");
            SQLSend = "select * from Kunde where PK_kundenr like '" + kundenr + "'";
            SQLData = Database.SQLkommandoGet(SQLSend);
            ParseKunde(SQLData);

            Console.WriteLine("1) Opret konto \n2) Vælg aktiv konto\n3) Slet konto\n4) Indsæt beløb ");
            Console.ReadKey();
        }

        //******************* OPRET KUNDE *******************
        public static void OpretKunde()
        {
            Console.Write("Indtast navn på ny kunde: ");
            string navn = Console.ReadLine();
            do //Checker for om CPR nummeret er på 10 tal og kun numerisk
            {
                Console.Write("Indtast CPR-nr: ");
                CPRString = Console.ReadLine();
                CPRString = CPRString.Replace("-", "").Replace("/", "");
            } while (CPRString.Length != 10 && int.TryParse(CPRString, out int CPRnr));

            //Sender data til database
            string SQLSend = "INSERT INTO Kunde values('" + navn + "', GetDate(), '', " + CPRString + ")";
            Database.SQLkommandoSet(SQLSend);
            //Modtager kundenr fra database baseret på CPR nr
            string SQLGet = "SELECT PK_kundenr from Kunde where CPR = " + CPRString + ";";
            SQLData = Database.SQLkommandoGet(SQLGet);
            //Starter kundemenu med kundenr
            KundeMenu(int.Parse(SQLData[0]));
        }

        //******************* FIND KUNDE *******************
        public static void FindKunde(string valg)
        {
            string str;
            switch (valg)
            {
                case "1": // Søger efter kundenavn
                    Console.Write("Indtast søgning på kunde: ");
                    str = Console.ReadLine();
                    SQLSend = $"select * from Kunde where kundenavn like '%{str}%'";
                    break;

                case "2": // Søger efter kunde baseret på kontonummer
                    Console.Write("Indtast søgning på konto: ");
                    str = Console.ReadLine();
                    SQLSend = $"select PK_kundenr, kundenavn, kundedato, kundeslutdato, CPR from Konto, Kunde where PK_kontonr = '{str}' and PK_kundenr = FK_kundenr";
                    break;

                case "3": // Søger efter kunde på kundenummer
                    Console.Write("Indtast søgning på kundenummer: ");
                    str = Console.ReadLine();
                    SQLSend = $"select * from Kunde where PK_kundenr like '%{str}%'";
                    break;

                case "4": // Søger efter kunde på CPR-nummer
                    do
                    {
                        Console.Write("Indtast CPR-nr: ");
                        CPRString = Console.ReadLine();
                        CPRString = CPRString.Replace("-", "").Replace("/", "");
                    } while (CPRString.Length != 10 && int.TryParse(CPRString, out int CPRnr));
                    SQLSend = $"select * from Kunde where CPR like '%{CPRString}%'";
                    break;

                default:
                    Program.Menu();
                    break;
            }
            SQLData = Database.SQLkommandoGet(SQLSend);
            ParseKunde(SQLData);
            if (PK_kundenr.Length > 1)
            {
                Console.Write("Vælg nr: ");
                int knr = int.Parse(Console.ReadLine());
                KundeMenu(PK_kundenr[knr]);
            }
            else KundeMenu(PK_kundenr[0]);
        }

        //******************* SLET KUNDE *******************
        public static void SletKunde()
        {
            Console.Write("Indtast CPR-nr på kunde der skal slettes: ");
            string CPRString = Console.ReadLine();
            int CPR = int.Parse(CPRString.Replace("-", ""));
            string SQLSend = $"UPDATE Kunde set kundeslutdato = GetDate() where CPR = {CPR};";
            Database.SQLkommandoSet(SQLSend);
        }

        //******************* PARSE SQL KUNDE TIL C# *******************
        static void ParseKunde(string[] SQLData)
        {
            int count = 0;
            for (int i = 0; i < SQLData.Length; i += 5)
            {
                //Forøger arrays med 1
                Array.Resize(ref PK_kundenr, count + 1);
                Array.Resize(ref kundenavn, count + 1);
                Array.Resize(ref kundedato, count + 1);
                Array.Resize(ref kundeslutdato, count + 1);
                Array.Resize(ref CPR, count + 1);

                //Tager datastrømmen fra SQL og parser den med trinstørrelse af antal variabler
                PK_kundenr[count] = int.Parse(SQLData[i]);
                kundenavn[count] = SQLData[i + 1];
                kundedato[count] = Convert.ToDateTime(SQLData[i + 2]);
                if (SQLData[i + 3] != "") { kundeslutdato[count] = Convert.ToDateTime(SQLData[i + 3]); }
                CPR[count] = SQLData[i + 4];
                count++;
            }

            if (SQLData.Length != 0) //Checker at datastrømmen ikke er tom
            {
                string slutdato = "";
                for (int i = 0; i < PK_kundenr.Length; i++)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(i + " ");
                    //Sikrer at hvis der ikke er slutdato, skal den ikke vises
                    Console.ForegroundColor = ConsoleColor.White;
                    if (kundeslutdato[i] != null) { slutdato = $"Slutdato: {kundeslutdato[i]}"; } else { slutdato = ""; }
                    Console.WriteLine($"\nKundenr: {PK_kundenr[i]} - Kundenavn: {kundenavn[i]} - CPR: {CPR[i]} \nStartdato: {kundedato[i]} " + slutdato);
                }
            }
            else
            {
                Console.WriteLine("Ingen resultater fundet.");
            }
        }
    }
}
