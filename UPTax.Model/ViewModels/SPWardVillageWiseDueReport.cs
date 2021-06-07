using System.ComponentModel;

namespace UPTax.Model.ViewModels
{
    public class SPWardVillageWiseDueReport
    {
        [DisplayName("হোল্ডিং নং")]
        public string HoldingNo { get; set; }

        [DisplayName("করদাতার নাম")]
        public string HouseOwnerName { get; set; }
        [DisplayName("অর্থ বছর")]
        public string YearName { get; set; }
        [DisplayName("পিতার নাম")]
        public string FatherHusbandName { get; set; }

        [DisplayName("মোবাইল নাম্বার")]
        public string MobileNo { get; set; }

        [DisplayName("গ্রামের নাম")]
        public string VillageName { get; set; }
        [DisplayName("ওয়ার্ড নং")]
        public string WardNo { get; set; }
        [DisplayName("হাল কর")]
        public decimal TaxAmount { get; set; }
        [DisplayName("পূর্বের বকেয়া")]
        public decimal PreviousDueAmount { get; set; }
        [DisplayName("সর্বমোট বকেয়া কর")]
        public decimal TotalDueAmount { get; set; }
    }
}
