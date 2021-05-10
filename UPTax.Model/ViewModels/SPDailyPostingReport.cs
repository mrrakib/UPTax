using System;
using System.ComponentModel;

namespace UPTax.Model.ViewModels
{
    public class SPDailyPostingReport
    {
        [DisplayName("হোল্ডিং নং")]
        public string HoldingNo { get; set; }
        [DisplayName("করদাতার নাম")]
        public string HouseOwnerName { get; set; }
        [DisplayName("মোবাইল নাম্বার")]
        public string MobileNo { get; set; }
        [DisplayName("অর্থ বছর")]
        public string YearName { get; set; }
        [DisplayName("আদায় এর তারিখ")]
        public DateTime TaxPaymentDate { get; set; }
        [DisplayName("গ্রামের নাম")]
        public string VillageName { get; set; }
        [DisplayName("ওয়ার্ড নং")]
        public string WardNo { get; set; }
        [DisplayName("হাল আদায়")]
        public decimal OutstandingAmount { get; set; }
        [DisplayName("বকেয়া আদায়")]
        public decimal PreviousDueAmount { get; set; }
        [DisplayName("সর্বমোট আদায়")]
        public decimal TotalCollectAmount { get; set; }
    }
}
