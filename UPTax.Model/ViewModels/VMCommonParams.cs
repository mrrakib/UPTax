using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMCommonParams
    {
        public string UnionName { get; set; }
        public string UnionAddress { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string ChairmanSignature { get; set; }
        public string FinancialYearName { get; set; }
        public int FinancialYearId { get; set; }
        public string HoldingNo { get; set; }
    }
}
