using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUNIT
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu();
            Console.ReadKey();
        }

        static void Menu()
        {
            Console.WriteLine("G-UNIT Banking \n");
            Console.WriteLine(
                "1) Opret kunde \n" +
                "2) Find kunde \n" +
                "3) Find konto  \n" +
                "4) Find transaktion \n \n");
            Console.Write("Indtast valg: ");
            string valg = Console.ReadLine();
            switch (valg)
            {
                case "1":
                    Kunde.OpretKunde();
                    Menu();
                    break;
                case "2":
                    FindKundeMenu();
                    Menu();
                    break;
                case "3":
                    break;

                default:
                    break;
            }

        }

        static void FindKundeMenu()
        {
            Console.Write("Søg på \n(1) Navn \n(2) konto \n(3) Kundenummer \n(4) CPR : ");
            string valg = Console.ReadLine();
            Kunde.FindKunde(valg);
        }

        //                "3b) Slet kunde \n \n" +

        //Database.SQLkommando("INSERT INTO Kunde values('frede', '2018-01-17 08:11:45')");
    }
}
