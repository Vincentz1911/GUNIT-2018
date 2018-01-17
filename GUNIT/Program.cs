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
                "3b) Slet kunde \n \n" +
                "4) Find konto  \n" +
                "5b) Opret konto \n" +
                "6b) Slet konto  \n \n" +
                "7) Find transaktion \n \n");
            Console.Write("Indtast valg: ");
            string valg = Console.ReadLine();
            switch (valg)
            {
                case "1":
                    break;
                case "2":
                    Kunde.OpretKunde();
                    Console.WriteLine("Ny kunde oprettet");
                    Menu();
                    //Database.OpretKonto();
                    break;
                case "3":
                    Kunde.SletKunde();
                    Console.WriteLine("Kunde slettet");
                    Menu();
                    break;
                case "4":
                    break;

                default:
                    break;
            }

        }


        static void OpretKunde()
        {

        }

        //Database.SQLkommando("INSERT INTO Kunde values('frede', '2018-01-17 08:11:45')");
    }
}
