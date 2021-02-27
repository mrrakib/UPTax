using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTax.Model.Models
{
    [Table("ProfessionInfo")]
    public class ProfessionInfo : BaseEntity<int>
    {
        [Required(ErrorMessage = "পেশা দেয়া বাধ্যতামূলক")]
        [DisplayName("পেশা")]
        public string ProfessionTitle { get; set; }
    }
}
