using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMRPTTaxCollectionSingle
    {
        public string HoldingNo { get; set; }
        public string WardNo { get; set; }
        public string OwnerName { get; set; }
        public string BillAddress { get; set; }
        public string ParentName { get; set; }
        public string MobileNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string FinancialYearName { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal CurrentDueAmount { get; set; }
        public decimal PrevAmount { get; set; }
        public decimal PrevDueAmount { get; set; }
        public decimal PenaltyAmount { get; set; }
        public decimal PenaltyDueAmount { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public string GrandAmountStr { get; set; }
    }
}
