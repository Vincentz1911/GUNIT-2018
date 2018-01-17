using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GUNIT {
    class Konto {
        public static void OpretKonto() {
            Console.Write("Indtast CPR-nr: ");
            string CPRString = Console.ReadLine();

            Console.Write("Indtast Konto Type \n1.Løn\n2.Opsparing\n3.Lån\n");
            string kontoType = Console.ReadLine();

            switch (kontoType) {
                case "1":
                    kontoType = "1";
                    break;
                case "2":
                    kontoType = "2";
                    break;
                case "3":
                    kontoType = "3";
                    break;
                default:
                    break;
            }

            Console.WriteLine(kontoType);

            int CPR = int.Parse(CPRString.Replace("-", "").Replace("/", ""));
        
            string SQLSend = $"IF exists (select 1 from Kunde where CPR = {CPR}) insert into Konto values(0, GETDATE(), (select PK_kundenr from Kunde where CPR = {CPR}) , {kontoType});";
            Database.SQLkommando(SQLSend);

            string SQLGet = $"SELECT * from Kunde, Konto where CPR = {CPR} and Kunde.PK_kundenr = Konto.FK_kundenr;";

            Database.SQLkommandoGet(SQLGet);
            //Console.WriteLine(SQLGet);
            //int Kundenr = int.Parse(SQLGet);
            //Console.WriteLine("Der er blevet oprettet en kunde med navnet {0}, CPR {1} og kundenr {2}", navn, CPR, Kundenr);
            Console.ReadKey();
            Console.Clear();
        }

        public static void IndsætBeløb() {
            Console.Write("Indtast Kontonummer: ");
            string kontoNummer = Console.ReadLine();

            Console.Write("Indtast Beløb: ");
            string strBeløb = Console.ReadLine();

            string SQLSend = $"if exists (select 1 from Konto where PK_kontonr = {kontoNummer})  update Konto set saldo = saldo + {strBeløb} where PK_kontonr = {kontoNummer};";
            Database.SQLkommando(SQLSend);

            Console.WriteLine($"{strBeløb} indsat på kontonummer {kontoNummer}, ny saldo:");
            string SQLGet = $"SELECT saldo from Konto where PK_kontonr = {kontoNummer};";

            Database.SQLkommandoGet(SQLGet);

            // opret af transaktion 
            Metoder.OpretTransaktion(float.Parse(strBeløb), int.Parse(kontoNummer));
        }

        public static void HævBeløb() {
            Console.Write("Indtast Kontonummer: ");
            string kontoNummer = Console.ReadLine();

            Console.Write("Indtast Beløb: ");
            string strBeløb = Console.ReadLine();

            string SQLSend = $"if exists (select 1 from Konto where PK_kontonr = {kontoNummer}) update Konto set saldo = saldo - {strBeløb} where PK_kontonr = {kontoNummer}; ";
            Database.SQLkommando(SQLSend);

            Console.WriteLine($"{strBeløb} hævet fra kontonummer {kontoNummer}, ny saldo:");
            string SQLGet = $"SELECT saldo from Konto where PK_kontonr = {kontoNummer};";

            Database.SQLkommandoGet(SQLGet);

            // opret af transaktion 
            Metoder.OpretTransaktion(-float.Parse(strBeløb), int.Parse(kontoNummer));
        }

        public static void OverførBeløb() {
            Console.Write("Overfør fra Kontonummer: ");
            string fraKonto = Console.ReadLine();

            Console.Write("til Kontonummer: ");
            string tilKonto = Console.ReadLine();

            Console.Write("Indtast Beløb: ");
            string strBeløb = Console.ReadLine();

            string SQLSend = $"update Konto set saldo = saldo - {strBeløb} where PK_kontonr = {fraKonto};";
            Database.SQLkommando(SQLSend);

            SQLSend = $"update Konto set saldo = saldo + {strBeløb} where PK_kontonr = {tilKonto};";
            Database.SQLkommando(SQLSend);

            Console.WriteLine($"{strBeløb} overført fra kontonummer {fraKonto} til {tilKonto};");
            //string SQLGet = "SELECT saldo from Konto where PK_kontonr = " + tilKonto + ";";

            //Database.SQLkommandoGet(SQLGet);

            // opret af transaktion 
            Metoder.OpretTransaktion(-float.Parse(strBeløb), int.Parse(fraKonto));
            Metoder.OpretTransaktion(float.Parse(strBeløb), int.Parse(tilKonto));


        }
    }
}
