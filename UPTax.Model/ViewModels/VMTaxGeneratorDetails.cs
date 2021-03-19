using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMTaxGeneratorDetails
    {
        public string HouseOwnerName { get; set; }
        public double TotalYearlyRent { get; set; }
        public double TotalYearlyTax { get; set; }
    }
}
