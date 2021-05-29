using System;
using System.Collections.Generic;
using UPTax.Model.Models;

namespace UPTax.Model.ViewModels
{
    public class VMMessageInfo
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string ReplyMessage { get; set; }
        public string ToAdminUserName { get; set; }
        public string ToSupperAdminUserName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<MessageReply> MessageReply { get; set; }
    }
}
