using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTax.Model.Models
{
    [Table("InfrastructureInfo")]
    public class InfrastructureInfo : BaseEntity<int>
    {
        [DisplayName("হোল্ডিং নাম্বার")]
        public string HoldingNo { get; set; }
        [DisplayName("অবকাঠমোর ধরণ")]
        public string TypeOfInfrastructure { get; set; }
        [DisplayName("মোট ঘর")]
        public int TotalNumberOfHouse { get; set; }
    }
}
