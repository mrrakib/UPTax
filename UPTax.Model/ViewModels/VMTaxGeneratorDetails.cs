using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMTaxGeneratorDetails
    {
        public int HouseOwnerId { get; set; }
        [DisplayName("খানা প্রধান")]
        public string HouseOwnerName { get; set; }
        [DisplayName("মোট বাৎসরিক ভাড়া")]
        public double TotalYearlyRent { get; set; }
        [DisplayName("মোট বাৎসরিক কর")]
        public double TotalYearlyTax { get; set; }
    }
}
