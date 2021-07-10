using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UPTax.Model.ViewModels
{
    public class VMInstituteInfo
    {
        public int Id { get; set; }
        [DisplayName("ওয়ার্ড নাম্বার")]
        public string WardNo { get; set; }
        [DisplayName("গ্রাম")]
        public string VillageName { get; set; }
        [DisplayName("হোল্ডিং নাম্বার")]
        public string HoldingNo { get; set; }
        [DisplayName("বাৎসরিক গড় আয়")]
        public double YearlyIncome { get; set; }

        [DisplayName("প্রতিষ্ঠানের নাম (বাংলা)")]
        public string NameOfInstituteBangla { get; set; }
        [DisplayName("প্রতিষ্ঠানের নাম (ইংরেজিতে)")]
        public string NameOfInstituteEnglish { get; set; }

        [DisplayName("মোবাইল নাম্বার")]
        public string MobileNo { get; set; }

        [DisplayName("প্রতিষ্ঠানের তারিখ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM, yyyy}")]
        public DateTime DateOfEstablishment { get; set; }

        [DisplayName("নলকূপ আছে কিনা")]
        public bool IsTubeWell { get; set; }

        [DisplayName("স্যানিটারি আছে কিনা")]
        public string Sanitary { get; set; }

        public int? TotalBuildingHouse { get; set; }
        public int? TotalSemiBuildingHouse { get; set; }
        public int? TotalRawHouse { get; set; }

        [DisplayName("প্রতিষ্ঠানের ধরণ")]
        public string InstituteType { get; set; }

        [DisplayName("পূর্বের বকেয়া")]
        public double? PreviousDueAmount { get; set; }


    }
}
