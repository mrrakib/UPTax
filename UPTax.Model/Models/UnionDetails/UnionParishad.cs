using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.Models.UnionDetails
{
    [Table("UnionParishad")]
    public class UnionParishad
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "নাম দেয়া বাধ্যতামূলক")]
        [StringLength(250, ErrorMessage = "সর্বোচ্চ 10 শব্দ দিতে পারবেন")]
        [DisplayName("নাম")]
        public string Name { get; set; }

        [StringLength(14, ErrorMessage = "সর্বোচ্চ ১৪ সংখ্যা দিতে পারবেন")]
        [DisplayName("মোবাইল নাম্বার")]
        public string PhoneNo { get; set; }
        [StringLength(150, ErrorMessage = "সর্বোচ্চ ১৫০ লেটার দিতে পারবেন")]
        [DisplayName("ইমেইল")]
        public string Email { get; set; }
        [StringLength(350, ErrorMessage = "সর্বোচ্চ ৫০ শব্দ দিতে পারবেন")]
        [DisplayName("বর্ণনা")]
        public string Description { get; set; }
    }
}
