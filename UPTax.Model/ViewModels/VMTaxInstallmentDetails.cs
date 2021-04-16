using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMTaxInstallmentDetails
    {
        public int HouseOwnerId { get; set; }
        [DisplayName("খানা প্রধান")]
        public string HouseOwnerName { get; set; }
        [DisplayName("জমার তারিখ")]
        public DateTime InstallmentDate { get; set; }
        [DisplayName("কিস্তির পরিমাণ")]
        public decimal InstallmentAmount { get; set; }
        [DisplayName("বকেয়া")]
        public decimal DueAmount { get; set; }
        [DisplayName("দন্ড প্রাপ্ত বাবদ")]
        public decimal PenaltyAmount { get; set; }
    }
}
