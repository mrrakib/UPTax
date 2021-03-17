using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using UPTax.Model.Models.UnionDetails;

namespace UPTax.Model.Models
{
    [Table("HouseOwners")]
    public class HouseOwner : BaseEntity<int>
    {
        [DisplayName("নাম (ইংরেজিতে)")]
        public string OwnerNameInEnglish { get; set; }
        [DisplayName("খানা প্রধানের নাম")]
        public string OwnerNameInBangla { get; set; }
        [DisplayName("বাৎসরিক গড় আয়")]
        public double YearlyIncome { get; set; }
        [DisplayName("ওয়ার্ড নাম্বার")]
        public int WardInfoId { get; set; }
        [ForeignKey("WardInfoId")]
        public virtual WardInfo WardInfo { get; set; }
        [DisplayName("গ্রাম")]
        public int VillageInfoId { get; set; }
        public virtual VillageInfo VillageInfo { get; set; }
        [DisplayName("হোল্ডিং নাম্বার")]
        public string HoldingNo { get; set; }
        [DisplayName("শিক্ষাগত যোগ্যতা")]
        public int EducationInfoId { get; set; }
        public virtual EducationInfo EducationInfo { get; set; }

        [DisplayName("মোবাইল নাম্বার")]
        public string MobileNo { get; set; }

        [DisplayName("পিতা/স্বামীর নাম")]
        public string FatherHusbandName { get; set; }

        [DisplayName("মাতার নাম")]
        public string MotherName { get; set; }

        [DisplayName("জন্ম তারিখ")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("জন্ম নিবন্ধন নাম্বার")]
        public string BirthRegistrationNumber { get; set; }

        [DisplayName("এনআইডি নাম্বার")]
        public string NIDNumber { get; set; }

        [DisplayName("খানা প্রধানের লিঙ্গ")]
        public int GenderId { get; set; }
        public virtual Gender Gender { get; set; }

        [DisplayName("খানা প্রধানের ধর্ম")]
        public int ReligionId { get; set; }
        public virtual Religion Religion { get; set; }

        [DisplayName("পেশা")]
        public int ProfessionId { get; set; }
        public virtual ProfessionInfo Profession { get; set; }

        [DisplayName("নলকূপ আছে কিনা")]
        public bool IsTubeWell { get; set; }

        [DisplayName("স্যানিটারি আছে কিনা")]
        public string Sanitary { get; set; }


        [DisplayName("কি ধরণের সামাজিক সুরক্ষা সুবিধা অতীতে পেয়েছেন")]
        public int? SocialBenefitBeforeId { get; set; }
        public virtual SocialBenefit SocialBenefitBefore { get; set; }
        [DisplayName("অন্যান্য কি ধরণের সামাজিক সুরক্ষা সুবিধা পাওয়ার যোগ্য")]
        public int? SocialBenefitEligibleId { get; set; }
        public virtual SocialBenefit SocialBenefitEligible { get; set; }
        [DisplayName("বর্তমানে কি ধরণের সামাজিক সুরক্ষা সুবিধা পাচ্ছেন")]
        public int? SocialBenefitRunningId { get; set; }
        public virtual SocialBenefit SocialBenefitRunning { get; set; }

        [DisplayName("অবকাঠামো ধরণ")]
        public int InfrastructureTypeId { get; set; }
        public virtual InfrastructureInfo InfrastructureType { get; set; }

        [DisplayName("বাৎসরিক ভাড়া")]
        public double? YearlyRentAmount { get; set; }

        [DisplayName("ঋনের বার্ষিক সুদ")]
        public double? YearlyLoanAmount { get; set; }

        [DisplayName("বসবাসের ধরণ")]
        public string LivingType { get; set; }

        [DisplayName("পূর্বের বকেয়া")]
        public double? PreviousDueAmount { get; set; }
    }
}
