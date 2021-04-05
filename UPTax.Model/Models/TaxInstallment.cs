using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.Models
{
    public class TaxInstallment : BaseEntity<int>
    {
        public int? UnionId { get; set; }
        public int WardInfoId { get; set; }
        public int FinancialYearId { get; set; }
        public string HoldingNo { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal PenaltyAmount { get; set; }
        public DateTime TaxPaymentDate { get; set; }
    }
}
