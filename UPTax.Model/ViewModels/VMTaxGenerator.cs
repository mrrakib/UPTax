using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMTaxGenerator
    {
        public int Id { get; set; }
        [DisplayName("অর্থবছর")]
        public int FinancialYearId { get; set; }
        [DisplayName("হোল্ডিং নাম্বার")]
        public string HoldingNo { get; set; }
        [DisplayName("বাৎসরিক মুনাফার হার")]
        public double YearlyTaxRate { get; set; }
        public VMTaxGeneratorDetails VMTaxGeneratorDetails { get; set; }
    }
}
