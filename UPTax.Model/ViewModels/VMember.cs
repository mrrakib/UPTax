using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UPTax.Model.ViewModels
{
    public class VMember
    {
        public int Id { get; set; }

        [DisplayName("হোল্ডিং নাম্বার")]
        public string HoldingNo { get; set; }

        [DisplayName("সদস্যের নাম (বাংলায়)")]
        public string MemberNameInBangla { get; set; }

        [DisplayName("পেশা")]
        public string Profession { get; set; }

        [DisplayName("জন্ম তারিখ")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d MMM, yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("জন্ম নিবন্ধন নাম্বার")]
        public string BirthRegistrationNumber { get; set; }

        [DisplayName("এনআইডি নাম্বার")]
        public string NIDNumber { get; set; }

        [DisplayName("সদস্যের লিঙ্গ")]
        public string GenderName { get; set; }

        [DisplayName("স্খানা প্রধানের সাথে সম্পর্ক")]
        public string Relationship { get; set; }


        [DisplayName("শিক্ষাগত যোগ্যতা")]
        public string EducationName { get; set; }

        [DisplayName("কি ধরণের সামাজিক সুরক্ষা সুবিধা অতীতে পেয়েছেন")]
        public string SocialBenefitBeforeName { get; set; }
        [DisplayName("অন্যান্য কি ধরণের সামাজিক সুরক্ষা সুবিধা পাওয়ার যোগ্য")]
        public string SocialBenefitEligibleName { get; set; }
        [DisplayName("বর্তমানে কি ধরণের সামাজিক সুরক্ষা সুবিধা পাচ্ছেন")]
        public string SocialBenefitRunningName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
