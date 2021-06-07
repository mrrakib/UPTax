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
    [Table("FinancialYear")]
    public class FinancialYear
    {
        [Key]
        public int Id { get; set; }
        [StringLength(200)]
        [DisplayName("অর্থবছর")]
        public string YearName { get; set; }
        [DisplayName("শুরুর তারিখ")]
        public DateTime StartDate { get; set; }
        [DisplayName("শেষের তারিখ")]
        public DateTime EndDate { get; set; }
        public bool IsDeleted { get; set; }
        [DisplayName("মন্তব্য")]
        public string Remarks { get; set; }
    }
}
