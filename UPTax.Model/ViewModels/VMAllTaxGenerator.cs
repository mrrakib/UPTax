using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMAllTaxGenerator
    {
        public int HouseOwnerId { get; set; }
        public double TotalYearlyRent { get; set; }
        public double TotalYearlyTax { get; set; }
        public string HoldingNo { get; set; }
        public int MyProperty { get; set; }
        public double YearlyTaxRate { get; set; }
    }
}
