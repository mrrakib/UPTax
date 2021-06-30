using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTax.Model.Models.UnionDetails
{
    [Table("WardInfo")]
    public class WardInfo : BaseEntity<int>
    {
        public WardInfo()
        {
            UnionId = 1;
        }
        [DisplayName("ওয়ার্ড নং")]
        public string WardNo { get; set; }
        public int UnionId { get; set; }
        [NotMapped]
        public int WardNoEng { get; set; }
        [ForeignKey("UnionId")]
        public virtual UnionParishad UnionParishad { get; set; }

    }
}
