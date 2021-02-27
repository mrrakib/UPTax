using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.Models.Account
{
    public class ApplicationUser : IdentityUser
    {
        [Column(TypeName = "NVARCHAR")]
        [StringLength(250)]
        public string FullName { get; set; }
        public int EmployeeId { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [StringLength(250)]
        public string PlainPassword { get; set; }
        [Column(TypeName = "BIT")]
        public bool IsActive { get; set; }
        [Column(TypeName = "INT")]
        public int? UnionId { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public ApplicationRole(string name, string description)
            : base(name)
        {
            this.Description = description;
            this.Priority = 0;
        }

        public ApplicationRole(string name)
        {
            this.Priority = 0;
        }

        public virtual string Description { get; set; }
        public int Priority { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole
    {
        public ApplicationUserRole()
            : base()
        { }

        public ApplicationRole Role { get; set; }

    }
}
