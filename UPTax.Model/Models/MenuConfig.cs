using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.Models
{
    public class MenuConfig : BaseEntity<int>
    {
        [Required(ErrorMessage = "Controller name required!")]
        [StringLength(250, ErrorMessage = "Maximum length should be 250")]
        [DisplayName("কন্ট্রোলার নাম")]
        public string ControllerName { get; set; }
        [DisplayName("মেনু ক্যাটাগরি")]
        public int CategoryId { get; set; }
        [DisplayName("মেনু অর্ডার")]
        public int OrderNo { get; set; }
        [StringLength(500, ErrorMessage = "Maximum length should be 500")]
        [DisplayName("মেনুর নাম")]
        public string MenuName { get; set; }


        [ForeignKey("CategoryId")]
        public virtual MenuCategory MenuCategory { get; set; }
    }
}
