using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GUNIT
{
    class Konto {
        public static void OpretKonto() {
            string CPRString;
            string kontoType;

            while (true) {
                Console.Write("Indtast CPR-nr: ");
                CPRString = Console.ReadLine();

                if (!int.TryParse(CPRString.Replace("-", "").Replace("/", ""), out var x) || x.ToString().Length != 10) {
                    Console.Write($"\nUgyldigt CPRNummer {x}.\n");
                    continue;
                }
                break;
            }

            while (true) {
                Console.Write("Indtast Konto Type \n1.Løn\n2.Opsparing\n3.Lån\n");
                kontoType = Console.ReadLine();

                if (!int.TryParse(kontoType.Replace("-", "").Replace("+", ""), out var y) || y > 3 || y < 1) {
                    Console.Write($"Ugyldigt kontotype {y}.");
                    continue;
                }
                break;
            }

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


            string SQLSend = $"IF exists (select 1 from Kunde where CPR = {CPRString}) insert into Konto values(0, GETDATE(), null, (select PK_kundenr from Kunde where CPR = {CPRString}) , {kontoType});";
            Database.SQLkommandoSet(SQLSend);

            Console.Write("Konto oprettet!");

            //string SQLGet = $"SELECT * from Kunde, Konto where CPR = {CPRString} and Kunde.PK_kundenr = Konto.FK_kundenr;";
            //Database.SQLkommandoGet(SQLGet);

            Console.Write("\nTryk tast for afslut.");
            Console.ReadKey();
            Console.Clear();
        }

        public static void SletKonto() {
            string kontoNummer = IndtastKontonummer();

            string SQLSend = $"if exists (select 1 from Konto where PK_kontonr = {kontoNummer})  update Konto set kontoslutdato = GETDATE() where PK_kontonr = {kontoNummer};";

            Database.SQLkommandoSet(SQLSend);

            Console.Write($"Konto nummer {kontoNummer} slettet!");

            Console.Write("\nTryk tast for afslut.");
            Console.ReadKey();
            Console.Clear();
        }


        public static void IndsætBeløb() {
            string kontoNummer = IndtastKontonummer();
            string strBeløb = IndtastBeløb();

            string SQLSend = $"if exists (select 1 from Konto where PK_kontonr = {kontoNummer})  update Konto set saldo = saldo + {strBeløb} where PK_kontonr = {kontoNummer};";
            Database.SQLkommandoSet(SQLSend);

            string SQLGet = $"SELECT saldo from Konto where PK_kontonr = {kontoNummer};";
            string[] sum = Database.SQLkommandoGet(SQLGet);

            Console.WriteLine($"{strBeløb} indsat på kontonummer {kontoNummer}, ny saldo: {sum[0]}");

            // Opret af transaktion.
            OpretTransaktion(strBeløb, int.Parse(kontoNummer));

            Console.Write("Tryk tast for afslut.");
            Console.ReadKey();
            Console.Clear();
        }

        public static void HævBeløb() {
            string kontoNummer = IndtastKontonummer();
            string strBeløb = IndtastBeløb();

            string SQLSend = $"if exists (select 1 from Konto where PK_kontonr = {kontoNummer})  update Konto set saldo = saldo - {strBeløb} where PK_kontonr = {kontoNummer};";
            Database.SQLkommandoSet(SQLSend);

            string SQLGet = $"SELECT saldo from Konto where PK_kontonr = {kontoNummer};";
            string[] sum = Database.SQLkommandoGet(SQLGet);

            Console.WriteLine($"{strBeløb} hævet på kontonummer {kontoNummer}, ny saldo: {sum[0]}");

            // Opret af transaktion.
            OpretTransaktion($"-{strBeløb}", int.Parse(kontoNummer));

            Console.Write("Tryk tast for afslut.");
            Console.ReadKey();
            Console.Clear();
        }

        public static void OverførBeløb() {
            string fraKonto = IndtastKontonummer();
            string tilKonto = IndtastKontonummer();
            string strBeløb = IndtastBeløb();

            string SQLSend = $"update Konto set saldo = saldo - {strBeløb} where PK_kontonr = {fraKonto};";
            Database.SQLkommandoSet(SQLSend);

            SQLSend = $"update Konto set saldo = saldo + {strBeløb} where PK_kontonr = {tilKonto};";
            Database.SQLkommandoSet(SQLSend);

            Console.WriteLine($"{strBeløb} overført fra kontonummer {fraKonto} til {tilKonto};");

            // Opret af transaktion. 
            OpretTransaktion($"-{strBeløb}", int.Parse(fraKonto));
            OpretTransaktion($"{strBeløb}", int.Parse(tilKonto));

            Console.Write("Tryk tast for afslut.");
            Console.ReadKey();
            Console.Clear();
        }

        public static string IndtastKontonummer() {
            string kontoNummer;

            while (true) {
                Console.Write("Indtast Konto Nummer: ");
                kontoNummer = Console.ReadLine();

                if (!int.TryParse(kontoNummer, out var x)) {
                    Console.Write($"\nUgyldigt Kontonummer {x}.\n");
                    continue;
                }
                break;
            }

            return kontoNummer;
        }

        public static string IndtastBeløb() {
            string strBeløb;

            while (true) {
                Console.Write("Indtast Beløb: ");
                strBeløb = Console.ReadLine();

                // Normalisering af beløb.
                strBeløb = strBeløb.Replace("-", "").Replace("+", "").Replace(",", ".");

                if (!float.TryParse(strBeløb, out var x)) {
                    Console.Write($"\nUgyldigt beløb {x}.\n");
                    continue;
                }
                break;
            }

            return strBeløb;
        }

            public static void OpretTransaktion(string beløb, int kontoNummer)
        {
            string SQLSend = $"insert into Transaktion values (GETDATE(), {beløb}, {kontoNummer}); ";
            Database.SQLkommandoSet(SQLSend);
        }
    }
}