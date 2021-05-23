using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTax.Model.Models
{
    [Table("MessageInfo")]
    public class MessageInfo : BaseEntity<int>
    {
        [Required]
        public string Message { get; set; }
        public string ToAdminUserId { get; set; }
        public string ToSupperAdminUserId { get; set; }
        [NotMapped]
        public int UnionId { get; set; }
        [NotMapped]
        public string AdminOrUserId { get; set; }
    }
}
