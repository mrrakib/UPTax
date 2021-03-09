using System;
using System.Collections.Generic;
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
        public string ControllerName { get; set; }
        public int CategoryId { get; set; }
        public int OrderNo { get; set; }
        [StringLength(500, ErrorMessage = "Maximum length should be 500")]
        public string MenuName { get; set; }


        [ForeignKey("CategoryId")]
        public virtual MenuCategory MenuCategory { get; set; }
    }
}
