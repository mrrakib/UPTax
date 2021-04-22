using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMReportViewerCommon
    {
        [DisplayName("হোল্ডিং নাম্বার")]
        public string HoldingNo { get; set; }
        [DisplayName("অর্থবছর")]
        public string FinancialYearId { get; set; }
    }
}
