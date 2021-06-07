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
        public string CurrentAmountStr { get; set; }
        public decimal CurrentDueAmount { get; set; }
        public string CurrentDueAmountStr { get; set; }
        public decimal PrevAmount { get; set; }
        public string PrevAmountStr { get; set; }
        public decimal PrevDueAmount { get; set; }
        public string PrevDueAmountStr { get; set; }
        public decimal PenaltyAmount { get; set; }
        public string PenaltyAmountStr { get; set; }
        public decimal PenaltyDueAmount { get; set; }
        public string PenaltyDueAmountStr { get; set; }
        public decimal GrandTotalAmount { get; set; }
        public string GrandTotalAmountStr { get; set; }
        public string GrandAmountStr { get; set; }
        public string LastPaymentDateStr { get; set; }
        public decimal HoldingTotal { get; set; }
        public string HoldingTotalStr { get; set; }
        public string PrevTotalStr { get; set; }
        public decimal PrevTotal { get; set; }
        public decimal TotalPenalty { get; set; }
        public string TotalPenaltyStr { get; set; }
    }
}
