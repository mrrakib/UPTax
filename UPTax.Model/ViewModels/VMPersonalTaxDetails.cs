using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMPersonalTaxDetails
    {
        public string FinYearName { get; set; }
        public int? TotalBuildingHouse { get; set; }
        public int? TotalSemiBuildingHouse { get; set; }
        public int? TotalRawHouse { get; set; }
        public DateTime? DateOfTaxCreation { get; set; }
        public decimal TotalTax { get; set; }
    }
}
