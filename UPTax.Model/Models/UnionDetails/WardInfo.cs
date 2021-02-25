using System.ComponentModel.DataAnnotations.Schema;

namespace UPTax.Model.Models.UnionDetails
{
    [Table("WardInfo")]
    public class WardInfo : BaseEntity<int>
    {
        public string WardNo { get; set; }
        public int UnionId { get; set; }
        [ForeignKey("UnionId")]
        public virtual UnionParishad UnionParishad { get; set; }

    }
}
