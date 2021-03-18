using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMMenuPermission
    {
        [Required(ErrorMessage = "Role is required!")]
        [DisplayName("রোল")]
        public string RoleId { get; set; }
        [Required(ErrorMessage = "Category is required!")]
        [DisplayName("মেনু ক্যাটাগরি")]
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

    public class Menu
    {
        public string CategoryName { get; set; }
        public string Icon { get; set; }
        public int OrderNo { get; set; }
        public List<MenuItem> MenuList { get; set; }
    }
    public class MenuItem
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string MenuName { get; set; }
        public int OrderNo { get; set; }
    }
}
