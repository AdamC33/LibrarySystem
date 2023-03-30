using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    class MoneyFormat
    {
        public static string AddSignAndDecimal(string pounds, string pence) //Function for adding the pound sign and decimal point
        {
            string money = "£" + pounds + "." + pence;
            return money;
        }

        public static string GetWithoutPence(string money) //Function for returning only the pounds without the pence
        {
            int indexOfDecimal = money.IndexOf('.');
            money = money.Remove(indexOfDecimal);
            money = money.Remove(0, 1);
            return money;
        }

        public static string GetOnlyPence (string money) //Function for returning only the pence without the pounds
        {
            int indexOfDecimal = money.IndexOf('.');
            money = money.Remove(0, indexOfDecimal + 1);
            return money;
        }
    }
}
