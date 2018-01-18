using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUNIT
{
    class Program
    {
        static string SQLSend;

        static void Main(string[] args)
        {
            Menu();
        }

        static void Menu()
        {
            Console.Write("G-UNIT Banking \n\n" + 
                "1) Opret kunde \n" +
                "2) Find kunde \n" +
                "3) Find konto  \n" +
                "4) Find transaktion \n" +
                "5) Slet kunde \n\n" +
                "Indtast valg: ");
            string valg = Console.ReadLine();
            switch (valg)
            {
                case "1":
                    Kunde.OpretKunde();                  
                    break;
                case "2":
                    FindKundeMenu();
                    Menu();
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    Kunde.SletKunde();
                    break;

                default:
                    break;
            }

        }

        static void FindKundeMenu()
        {
            Console.Clear();
            Console.Write("Søg på \n1) Navn \n2) konto \n3) Kundenummer \n4) CPR \n\nIndtast valg : ");
            string valg = Console.ReadLine();
            Kunde.FindKunde(valg);
        }


        //                "3b) Slet kunde \n \n" +

        //Database.SQLkommando("INSERT INTO Kunde values('frede', '2018-01-17 08:11:45')");
    }
}
