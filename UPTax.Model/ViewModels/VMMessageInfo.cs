using System;

namespace UPTax.Model.ViewModels
{
    public class VMMessageInfo
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string ToAdminUserName { get; set; }
        public string ToSupperAdminUserName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
