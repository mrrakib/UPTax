using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPTax.Model.Models.Account;

namespace UPTax.Model.Models
{
    [Table("MenuPermission")]
    public class MenuPermission : BaseEntity<int>
    {
        [Required]
        public string RoleId { get; set; }
        [Required]
        public int MenuConfigId { get; set; }
        public bool IsViewPermitted { get; set; }
        public bool IsAddPermitted { get; set; }
        public bool IsEditPermitted { get; set; }
        public bool IsDeletePermitted { get; set; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole ApplicationRole { get; set; }

        [ForeignKey("MenuConfigId")]
        public virtual MenuConfig MenuConfig { get; set; }
    }
}
