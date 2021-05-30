using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMPersonalShortReport
    {
        public string HoldingNo { get; set; }
        public decimal TotalYearlyTax { get; set; }
        public decimal TotalYearlyPaidTax { get; set; }
        public bool IsPaid { get; set; }
    }
}
