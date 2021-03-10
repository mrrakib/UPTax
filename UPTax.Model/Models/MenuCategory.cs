using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.Models
{
    public class MenuCategory : BaseEntity<int>
    {
        [Required]
        [StringLength(250)]
        [DisplayName("ক্যাটাগরি নাম")]
        public string Name { get; set; }

        [StringLength(250)]
        [DisplayName("আইকন")]
        public string Icon { get; set; }
        [DisplayName("ক্যাটাগরি অর্ডার")]
        public int OrderNo { get; set; }
    }
}
