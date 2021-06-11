using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UPTax.Model.ViewModels
{
    public class SPTopSheetReport
    {
        public int UnionId { get; set; }
        public int WardId { get; set; }

        [DisplayName("ইউনিয়ন পরিষদ")]
        public string UnionName { get; set; }
        [DisplayName("ওয়ার্ড নাম্বার")]
        public string WardNo { get; set; }
        [DisplayName("গ্রামের সংখ্যা")]
        public int TotalVillageCount { get; set; }
        [DisplayName("খানার সংখ্যা")]
        public int TotalHoldingCount { get; set; }
        [DisplayName("আদায়ক্রিত  খানার সংখ্যা")]
        public int TotalTaxCollectHoldingCount { get; set; }
        [DisplayName("বকেয়া খানার সংখ্যা")]
        public int DueTotalTaxCollectHoldingCount { get; set; }
        [DisplayName("হাল ধার্য কর")]
        public decimal TotalTaxAmount { get; set; }
        [DisplayName("হাল আদায়")]
        public decimal TotalCollectTaxAmount { get; set; }
        [DisplayName("হাল বকেয়া")]
        public decimal TotalDueTaxAmount { get; set; }
        [DisplayName("পূর্বের বকেয়া")]
        public decimal TotalPreviousDueAmount { get; set; }
        [DisplayName("বকেয়া আদায়")]
        public decimal TotalPreviousDueCollectAmount { get; set; }
        [DisplayName("সর্বমোট আদায়")]
        public decimal TotalCollection { get; set; }
        [DisplayName("সর্বমোট বকেয়া")]
        public decimal TotalDues { get; set; }
        [DisplayName("অর্থ বছর")]
        public string YearName { get; set; }
        public int FinancialYearId { get; set; }
        [DisplayName("আদায়ের শতকরা হার")]
        [Range(0, 9999999999999999.99, ErrorMessage = "Invalid Target Price; Max 18 digits")]
        public decimal TaxCollectPercentage { get; set; }
        public decimal TotalDueCollectAmount { get; set; }
        [DisplayName("মন্তব্য")]
        public string Comments { get; set; }
    }
}
