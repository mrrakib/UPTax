using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMTranInfo
    {
        public string WardNo { get; set; }
        public int TotalHouseOwner { get; set; }
        public string TotalHouseOwnerStr { get; set; }
        public string GrandTotalHouseOwnerStr { get; set; }
        public int TotalMale { get; set; }
        public string TotalMaleStr { get; set; }
        public string GrandTotalMaleStr { get; set; }
        public int TotalFemale { get; set; }
        public string TotalFemaleStr { get; set; }
        public string GrandTotalFemaleStr { get; set; }
        public int TotalSocialBenefitTakingCount { get; set; }
        public string TotalSocialBenefitTakingCountStr { get; set; }
        public string GrandTotalSocialBenefitTakingCountStr { get; set; }
        public int TotalPoor { get; set; }
        public string TotalPoorStr { get; set; }
        public string GrandTotalPoorStr { get; set; }
        public int TotalMidPoor { get; set; }
        public string TotalMidPoorStr { get; set; }
        public string GrandTotalMidPoorStr { get; set; }
        public int TotalRich { get; set; }
        public string TotalRichStr { get; set; }
        public string GrandTotalRichStr { get; set; }
        public int TotalPopulation { get; set; }
        public string TotalPopulationStr { get; set; }
        public string GrandTotalPopulationStr { get; set; }

    }
}
