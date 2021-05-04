using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMRPTTaxReceipt
    {
        public string HoldingNo { get; set; }
        public string OwnerNameInBangla { get; set; }
        public string FatherHusbandName { get; set; }
        public string MobileNo { get; set; }
        public string WardNo { get; set; }
        public string VillageName { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime TaxPaymentDate { get; set; }
        public string YearName { get; set; }
        public decimal TotalCollection { get; set; }
    }
}
