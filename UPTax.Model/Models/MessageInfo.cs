using System.ComponentModel.DataAnnotations.Schema;

namespace UPTax.Model.Models
{
    [Table("MessageInfo")]
    public class MessageInfo : BaseEntity<int>
    {
        public string Message { get; set; }
        public string ToAdminUserId { get; set; }
        public string ToSupperAdminUserId { get; set; }
    }
}
