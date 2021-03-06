using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTax.Model.Models
{
    [Table("InstituteInfo")]
    public class InstituteInfo : BaseEntity<int>
    {
        [DisplayName("কলেজ / অফিসের নাম")]
        public string NameOfInstitute { get; set; }

        [DisplayName("কোড")]
        public string Code { get; set; }
        [DisplayName("EIIN নম্বর")]
        public string EiinNumber { get; set; }
        [DisplayName("টিন নম্বর")]
        public string TinNumber { get; set; }
        [DisplayName("বিন নম্বর")]
        public string BinNumber { get; set; }
    }
}
