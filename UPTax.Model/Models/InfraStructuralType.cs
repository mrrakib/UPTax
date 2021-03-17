using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPTax.Model.Models.UnionDetails;

namespace UPTax.Model.Models
{
    [Table("InfraStructuralType")]
    public class InfraStructuralType : BaseEntity<int>
    {
        [Required(ErrorMessage = "নাম দেয়া বাধ্যতামূলক")]
        [StringLength(150, ErrorMessage = "সর্বোচ্চ ৮ শব্দ প্রযোজ্য")]
        [DisplayName("অবকাঠামোর ধরন")]
        public string TypeName { get; set; }
        [Required(ErrorMessage = "ভাড়ার পরিমান দেয়া বাধ্যতামূলক")]
        [DisplayName("বাৎসরিক ভাড়া")]
        public double YearlyRent { get; set; }
        [Required(ErrorMessage = "মুনাফার পরিমান দেয়া বাধ্যতামূলক")]
        [DisplayName("বাৎসরিক মুনাফার হার")]
        public double InterestRate { get; set; }

    }
}
