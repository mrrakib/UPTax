using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMTaxInstallment
    {
        public int Id { get; set; }
        [DisplayName("অর্থবছর")]
        public int FinancialYearId { get; set; }
        [DisplayName("হোল্ডিং নাম্বার")]
        public string HoldingNo { get; set; }
        public VMTaxInstallmentDetails vMTaxInstallmentDetails { get; set; }
    }
}
