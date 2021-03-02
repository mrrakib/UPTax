using UPTax.Model.Models.Account;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace UPTax.Model.ViewModels
{
    public class VMRegister
    {
        public string UserId { get; set; }

        [RegularExpression("[A-Za-z0-9 _.-]*", ErrorMessage = "User name must contain only characters, number, underscore, dot, hyphen")]
        [Required(ErrorMessage = "User name is required field")]
        [DisplayName("ইউজার নেম")]
        [StringLength(100, ErrorMessage = "Maximum length 100 character")]

        public string UserName { get; set; }

        [RegularExpression("[A-Za-z .()-]*", ErrorMessage = "Name must contain only characters, dot, (), hyphen.")]
        // [Required(ErrorMessage = "Name is required field")]
        [StringLength(100, ErrorMessage = "Maximum length 100 character")]
        [DisplayName("নাম")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required field")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("পাসওয়ার্ড")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required field")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DisplayName("কনফার্ম পাসওয়ার্ড")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("ইউজার রোল")]
        public string RoleName { get; set; }

        public IEnumerable<IdentityRole> roles { get; set; }

        public int EmployeeId { get; set; }

        public string Email { get; set; }

        public string ContactNo { get; set; }
        [DisplayName("এক্টিভ?")]
        public bool IsActive { get; set; }
        [DisplayName("ইউনিয়ন")]
        public int? UnionId { get; set; }

        public ApplicationUser GetUser()
        {
            var user = new ApplicationUser()
            {
                UserName = this.UserName.Trim(),
                FullName = this.Name.Trim()
            };
            return user;
        }
    }


    public class VMEditUser
    {
        public string UserId { get; set; }

        [RegularExpression("[A-Za-z0-9 _.-]*", ErrorMessage = "User name must contain only characters, number, underscore, dot, hyphen")]
        [Required(ErrorMessage = "User name is required field")]
        [DisplayName("ইউজার নেম")]
        [StringLength(100, ErrorMessage = "Maximum length 100 character")]

        public string UserName { get; set; }

        [RegularExpression("[A-Za-z .()-]*", ErrorMessage = "Name must contain only characters, dot, (), hyphen.")]
        // [Required(ErrorMessage = "Name is required field")]
        [StringLength(100, ErrorMessage = "Maximum length 100 character")]
        [DisplayName("নাম")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required field")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("পাসওয়ার্ড")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required field")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DisplayName("কনফার্ম পাসওয়ার্ড")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("ইউজার রোল")]
        public string RoleName { get; set; }

        public IEnumerable<string> roles { get; set; }

        public int EmployeeId { get; set; }

        public string Email { get; set; }

        public string ContactNo { get; set; }
        [DisplayName("এক্টিভ?")]
        public bool IsActive { get; set; }
        [DisplayName("ইউনিয়ন")]
        public int? UnionId { get; set; }

        public ApplicationUser GetUser()
        {
            var user = new ApplicationUser()
            {
                UserName = this.UserName.Trim(),
                FullName = this.Name.Trim()
            };
            return user;
        }
    }
}
