using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMMenuPermission
    {
        [Required(ErrorMessage = "Role is required!")]
        public string RoleId { get; set; }
        [Required(ErrorMessage = "Category is required!")]
        public int CategoryId { get; set; }
        public List<VMMenuPermissionDetails> MenuPermissionDetails { get; set; }
    }

    public class VMMenuPermissionDetails
    {
        public int MenuConfigId { get; set; }
        public string MenuName { get; set; }
        public bool IsViewPermit { get; set; }
        public bool IsAddPermit { get; set; }
        public bool IsEditPermit { get; set; }
        public bool IsDeletePermit { get; set; }
    }
}
