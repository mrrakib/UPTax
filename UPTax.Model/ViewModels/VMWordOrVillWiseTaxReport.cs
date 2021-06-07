using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMWordOrVillWiseTaxReport
    {
        public string HoldingNo { get; set; }
        public string OwnerName { get; set; }
        public string ParentName { get; set; }
        public string MobileNo { get; set; }
        public string WardNo { get; set; }
        public string VillageName { get; set; }
        public int? TotalBuildingHouse { get; set; }
        public int? TotalRawHouse { get; set; }
        public int? TotalSemiBuildingHouse { get; set; }
        public double? YearlyRentAmount { get; set; }
        public double? TotalTax { get; set; }
        public decimal PeviousDue { get; set; }
        public string Comments { get; set; }
    }
}
