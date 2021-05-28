using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UPTax.Model.Models.UnionDetails;

namespace UPTax.Model.Models
{
    [Table("AdminNotice")]
    public class AdminNotice : BaseEntity<int>
    {
        [Required]
        public string Message { get; set; }
        public int UnionId { get; set; }
        [ForeignKey("UnionId")]
        public virtual UnionParishad Union { get; set; }
    }
}
