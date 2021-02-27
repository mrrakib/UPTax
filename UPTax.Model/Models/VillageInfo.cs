using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTax.Model.Models
{
    [Table("VillageInfo")]
    public class VillageInfo : BaseEntity<int>
    {
        [DisplayName("গ্রামের নাম")]
        [Required(ErrorMessage = "নাম দেয়া বাধ্যতামূলক")]
        public string VillageName { get; set; }

        [DisplayName("ইউনিয়ন পরিশদের নাম")]
        [Required(ErrorMessage = "নাম দেয়া বাধ্যতামূলক")]
        public int UnionId { get; set; }

        [ForeignKey("UnionId")]
        public virtual UnionDetails.UnionParishad UnionParishad { get; set; }
    }
}
