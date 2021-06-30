using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Data.Infrastructure
{
    public static class E2B
    {
        public static string SwitchEngBan(string number)
        {
            string en = "1234567890.,/-";
            string bn = "১২৩৪৫৬৭৮৯০.,/-";
            return number.Select(o => en.Contains(o)
            ? bn.Substring(en.IndexOf(o), 1)
            : en.Substring(bn.IndexOf(o), 1))
            .Aggregate((a, b) => a + b);
        }
    }
}
