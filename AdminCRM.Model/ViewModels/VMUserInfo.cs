using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMUserInfo
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public bool Status { get; set; }
    }
}
