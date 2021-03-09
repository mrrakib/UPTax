using System;
using System.Collections.Generic;
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
        public string Name { get; set; }

        [StringLength(250)]
        public string Icon { get; set; }
        public int OrderNo { get; set; }
    }
}
