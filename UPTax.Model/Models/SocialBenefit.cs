using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTax.Model.Models
{
    [Table("SocialBenefits")]
    public class SocialBenefit : BaseEntity<int>
    {
        [Required(ErrorMessage = "সামাজিক সুযোগ-সুবিধা দেয়া বাধ্যতামূলক")]
        [DisplayName("সামাজিক সুযোগ-সুবিধা")]
        public string Title { get; set; }
    }
}
