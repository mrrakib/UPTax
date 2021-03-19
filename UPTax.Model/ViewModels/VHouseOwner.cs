using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UPTax.Model.ViewModels
{
    public class VHouseOwner
    {
        public int Id { get; set; }

        [DisplayName("নাম (ইংরেজিতে)")]
        public string OwnerNameInEnglish { get; set; }
        [DisplayName("খানা প্রধানের নাম")]
        public string OwnerNameInBangla { get; set; }
        [DisplayName("বাৎসরিক গড় আয়")]
        public double YearlyIncome { get; set; }
        [DisplayName("হোল্ডিং নাম্বার")]
        public string HoldingNo { get; set; }

        [DisplayName("মোবাইল নাম্বার")]
        public string MobileNo { get; set; }

        [DisplayName("পিতা/স্বামীর নাম")]
        public string FatherHusbandName { get; set; }

        [DisplayName("মাতার নাম")]
        public string MotherName { get; set; }

        [DisplayName("জন্ম তারিখ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM, yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("জন্ম নিবন্ধন নাম্বার")]
        public string BirthRegistrationNumber { get; set; }

        [DisplayName("এনআইডি নাম্বার")]
        public string NIDNumber { get; set; }

        [DisplayName("নলকূপ আছে কিনা")]
        public bool IsTubeWell { get; set; }

        [DisplayName("স্যানিটারি আছে কিনা")]
        public string Sanitary { get; set; }



        [DisplayName("বাৎসরিক ভাড়া")]
        public double? YearlyRentAmount { get; set; }

        [DisplayName("ঋনের বার্ষিক সুদ")]
        public double? YearlyInterestRate { get; set; }

        [DisplayName("বসবাসের ধরণ")]
        public string LivingType { get; set; }

        [DisplayName("পূর্বের বকেয়া")]
        public double? PreviousDueAmount { get; set; }

        public int? TotalBuildingHouse { get; set; }
        public int? TotalSemiBuildingHouse { get; set; }
        public int? TotalRawHouse { get; set; }
        public string WardName { get; set; }
        public string UnionName { get; set; }
        public string VillageName { get; set; }
        public string EducationName { get; set; }
        public string GenderName { get; set; }
        public string ReligionName { get; set; }
        public string ProfessionName { get; set; }
        public string SocialBenefitRunningName { get; set; }
        public string SocialBenefitEligibleName { get; set; }
        public string SocialBenefitBeforeName { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
