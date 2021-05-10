using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UPTax.Model.ViewModels
{
    public class VMDailyPostingReport
    {
        public VMDailyPostingReport()
        {
            DailyPostingReports = new List<SPDailyPostingReport>();
        }
        public int WardId { get; set; }
        public int FinancialYearId { get; set; }
        public string ReportType { get; set; }

        [DisplayName("শুরুর তারিখ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM, yyyy}")]
        public DateTime StartDate { get; set; }

        [DisplayName("শেষের তারিখ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM, yyyy}")]
        public DateTime EndDate { get; set; }

        public List<SPDailyPostingReport> DailyPostingReports { get; set; }
    }
}
