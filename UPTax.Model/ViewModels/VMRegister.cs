using UPTax.Model.Models.Account;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMRegister
    {
        public string UserId { get; set; }


        [RegularExpression("[A-Za-z0-9 _.-]*", ErrorMessage = "User name must contain only characters, number, underscore, dot, hyphen")]
        [Required(ErrorMessage = "User name is required field")]
        [Display(Name = "Login Name")]
        [StringLength(100, ErrorMessage = "Maximum length 100 character")]
        public string UserName { get; set; }

        [RegularExpression("[A-Za-z .()-]*", ErrorMessage = "Name must contain only characters, dot, (), hyphen.")]
        // [Required(ErrorMessage = "Name is required field")]
        [StringLength(100, ErrorMessage = "Maximum length 100 character")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password is required field")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required field")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "User Role")]
        public string RoleName { get; set; }

        public IEnumerable<IdentityRole> roles { get; set; }

        public int EmployeeId { get; set; }

        public string Email { get; set; }

        public string ContactNo { get; set; }
        public bool IsActive { get; set; }

        public ApplicationUser GetUser()
        {
            var user = new ApplicationUser()
            {
                UserName = this.UserName,
                FullName = this.Name
            };
            return user;
        }
    }
}
