using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUNIT
{
    class Metoder {
        static void FindKunde(string str) {
            Database.SQLkommandoSet(str);
        }

        public static void OpretTransaktion(float beløb, int kontoNummer) {
            string SQLSend = $"insert into Transaktion values (GETDATE(), {beløb}, {kontoNummer}); ";
            Database.SQLkommandoSet(SQLSend);
        }
    }
}
