using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GUNIT
{
    class Konto {

        public static string[] kontoTypeNavne = { "Løn", "Opsparing", "Lån" };
        public static int _kontoNummer;

        public static void KontoMenu() {
            Console.Clear();
            Console.WriteLine("**********\n*Kontomenu*\n***********");

            Console.Write("1) Indsæt\n2) Hæv\n3) Overfør\n4) Slet konto\n5) Vis transaktioner\n6) Find transaktioner\n7) Afslut\n\nIndtast valg: ");
            string valg = Console.ReadLine();

            switch (valg) {
                case "1":
                    IndsætBeløb();
                    break;
                case "2":
                    HævBeløb();
                    break;
                case "3":
                    OverførBeløb();
                    break;
                case "4":
                    SletKonto();
                    break;
                case "5":
                    VisTransaktion();
                    break;
                case "6":
                    FindTransaktion();
                    break;
                case "7":
                    Environment.Exit(0);
                    break;
                default:
                    KontoMenu();
                    break;
            }
        }

        private static void FindTransaktion() {
            Console.Clear();

            Console.Write("1) Søg med dato\n2) Søg med Trans.ID\n3) Søg med beløb\n");
            string valg = Console.ReadLine();
            Console.Clear();

            switch (valg) {
                case "1":
                    Console.Write("Søg \"fra\" dato (YYYY-MM-DD): ");
                    string datofra = Console.ReadLine();
                    Console.Write("Søg \"til\" dato (YYYY-MM-DD): ");
                    string datotil = Console.ReadLine();
                    TransaktionSøger($"select * from Transaktion where FK_kontonr = '{_kontoNummer}' and transdato >= convert(datetime, '{datofra}') and transdato <= convert(datetime, '{datotil}');");
                    break;
                case "2":
                    Console.Write("Indtast Transaktions ID: ");
                    string ID = Console.ReadLine();
                    TransaktionSøger($"select * from Transaktion where FK_kontonr = '{_kontoNummer}' and PK_transID = '{ID}'");
                    break;
                case "3":
                    Console.Write("Søg \"fra\". beløb: ");
                    string min = Console.ReadLine();
                    Console.Write("Søg \"til\". beløb: ");
                    string max = Console.ReadLine();
                    TransaktionSøger($"select * from Transaktion where FK_kontonr = '{_kontoNummer}' and beloeb > '{min}' and beloeb < '{max}';");
                    break;
                default:
                    KontoMenu();
                    break;
            }

            Console.Write("\nTryk tast.");
            Console.ReadKey();
            KontoMenu();

        }

        private static void VisTransaktion() {
            Console.Clear();
            TransaktionSøger($"select * from Transaktion where FK_kontonr = '{_kontoNummer}';");

            Console.Write("\nTryk tast.");
            Console.ReadKey();
            KontoMenu();

        }

        // Hjælpefunktion til transaktion søgning.
        private static void TransaktionSøger(string sql) {
            string SQLGet = sql;
            string[] transArr = Database.SQLkommandoGet(SQLGet);

            if (transArr.Length == 0) {
                Console.Write("Ingen transaktioner fundet!");
                return;
            }

            int count = 0;

            for (int i = 0; i < transArr.Length / 4; i++) {
                Console.WriteLine($"Transaktions ID: {transArr[0 + count]}, Trans. dato: {transArr[1 + count]}, Beløb: {transArr[2 + count]}, Kunde nummer: {transArr[3 + count]}");
                count += 4;
            }
        }



        public static void OpretKonto(int kundenr) {
            Console.Clear();

            string kontoType;
            int kontoNummer;

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

            // Opretter konto.
            string SQLSend = $"IF exists (select 1 from Kunde where PK_kundenr = '{kundenr}') insert into Konto values(0, GETDATE(), null, (select PK_kundenr from Kunde where PK_kundenr = '{kundenr}') , '{kontoType}');";
            Database.SQLkommandoSet(SQLSend);

            // Finder senest oprettet kontonr til brug i kontomenu.
            string SQLGet = $"select PK_kontonr from Konto where FK_kundenr = '{kundenr}' order by PK_kontonr desc;";
            string strKontonummer = Database.SQLkommandoGet(SQLGet)[0];
            kontoNummer = int.Parse(strKontonummer);

            Console.Write("Konto oprettet! Tryk en tast!");
            Console.ReadKey();

            // Vigtig, gemmer kontonummer i lokal variabel.
            _kontoNummer = kontoNummer;
            KontoMenu();
        }

        public static void VælgKonto(int kundenr) {
            Console.Clear();

            // Find Konti
            string SQLGet = $"select PK_kontonr, saldo, kontodato, kontoslutdato, FK_kontotypeID from Konto where FK_kundenr = {kundenr};";
            string[] kontoArr = Database.SQLkommandoGet(SQLGet);

            // Retunerer hvis ingen konti er tilknyttet.
            if (kontoArr.Length == 0) {
                Console.Write("Ingen konti tilknyttet bruger, tryk tast for at retunerer");
                Console.ReadKey();
                Kunde.KundeMenu(kundenr);
            }

            int count = 0;

            for (int i = 0; i < kontoArr.Length / 5; i++) {

                // Check om oprettelses dato er null, erstatter med "Ingen".
                string slutdato = kontoArr[3 + count] == "" ? "Ingen" : kontoArr[3 + count];

                // Ændre konto type nr til "lån", "opsparing" etc.
                string kontotype = kontoTypeNavne[int.Parse(kontoArr[4 + count]) - 1];

                Console.WriteLine($"Kontonr: {kontoArr[0 + count]}, Saldo: {kontoArr[1 + count]}, Oprettelses dato: {kontoArr[2 + count]}, Konto slut dato: {slutdato}, Konto Type: {kontotype}");
                count += 5;
            }

            //TODO : Error Handling.

            Console.Write("Indtast konto nummer: ");
            string valg = Console.ReadLine();

            int.TryParse(valg, out var kontoNummer);

            // Vigtig, gemmer kontonummer i lokal variabel.
            _kontoNummer = kontoNummer;
            KontoMenu();
        }

        public static void SletKonto() {
            //string kontoNummer = IndtastKontonummer();
            Console.Clear();

            string SQLSend = $"if exists (select 1 from Konto where PK_kontonr = '{_kontoNummer}')  update Konto set kontoslutdato = GETDATE() where PK_kontonr = '{_kontoNummer}';";

            Database.SQLkommandoSet(SQLSend);

            Console.Write($"Konto nummer {_kontoNummer} slettet!");

            Console.Write("\nTryk tast for afslut.");
            Console.ReadKey();
            Console.Clear();

            KontoMenu();
        }


        public static void IndsætBeløb() {
            //string kontoNummer = IndtastKontonummer();
            Console.Clear();

            string strBeløb = IndtastBeløb();

            string SQLSend = $"if exists (select 1 from Konto where PK_kontonr = '{_kontoNummer}')  update Konto set saldo = saldo + {strBeløb} where PK_kontonr = '{_kontoNummer}';";
            Database.SQLkommandoSet(SQLSend);

            string SQLGet = $"SELECT saldo from Konto where PK_kontonr = '{_kontoNummer}';";
            string[] sum = Database.SQLkommandoGet(SQLGet);

            Console.WriteLine($"{strBeløb} indsat på kontonummer '{_kontoNummer}', ny saldo: {sum[0]}");

            // Opret af transaktion.
            OpretTransaktion(strBeløb, _kontoNummer);

            Console.Write("Tryk tast for afslut.");
            Console.ReadKey();
            Console.Clear();

            KontoMenu();
        }

        public static void HævBeløb() {
            //string kontoNummer = IndtastKontonummer();
            Console.Clear();

            string strBeløb = IndtastBeløb();

            string SQLSend = $"if exists (select 1 from Konto where PK_kontonr = '{_kontoNummer}')  update Konto set saldo = saldo - {strBeløb} where PK_kontonr = '{_kontoNummer}';";
            Database.SQLkommandoSet(SQLSend);

            string SQLGet = $"SELECT saldo from Konto where PK_kontonr = '{_kontoNummer}';";
            string[] sum = Database.SQLkommandoGet(SQLGet);

            Console.WriteLine($"{strBeløb} hævet på kontonummer '{_kontoNummer}', ny saldo: {sum[0]}");

            // Opret af transaktion.
            OpretTransaktion($"-{strBeløb}", _kontoNummer);

            Console.Write("Tryk tast for afslut.");
            Console.ReadKey();
            Console.Clear();

            KontoMenu();
        }

        public static void OverførBeløb() {
            Console.Clear();

            int fraKonto = _kontoNummer;
            string tilKonto = IndtastKontonummer();
            string strBeløb = IndtastBeløb();

            string SQLSend = $"update Konto set saldo = saldo - {strBeløb} where PK_kontonr = '{fraKonto}';";
            Database.SQLkommandoSet(SQLSend);

            SQLSend = $"update Konto set saldo = saldo + {strBeløb} where PK_kontonr = {tilKonto};";
            Database.SQLkommandoSet(SQLSend);

            Console.WriteLine($"{strBeløb} overført fra kontonummer '{fraKonto}' til {tilKonto};");

            // Opret af transaktion. 
            OpretTransaktion($"-{strBeløb}", fraKonto);
            OpretTransaktion($"{strBeløb}", int.Parse(tilKonto));

            Console.Write("Tryk tast for afslut.");
            Console.ReadKey();
            Console.Clear();

            KontoMenu();
        }

        // Hjælpefunktion til bruger input.
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

        // Hjælpefunktion til bruger input.
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