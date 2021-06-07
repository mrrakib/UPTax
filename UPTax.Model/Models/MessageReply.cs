using System;
using System.ComponentModel.DataAnnotations.Schema;
using UPTax.Model.Models.Account;

namespace UPTax.Model.Models
{
    [Table("MessageReply")]
    public class MessageReply
    {
        public int Id { get; set; }
        public int MessageInfoId { get; set; }
        public string ReplyMessage { get; set; }
        public string ReplyerUserId { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("ReplyerUserId")]
        public virtual ApplicationUser ReplyerUser { get; set; }
    }
}
