using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTax.Model.Models
{
    [Table("EducationInfo")]
    public class EducationInfo : BaseEntity<int>
    {
        [Required]
        [DisplayName("শিক্ষাগত যোগ্যতা")]
        public string Degree { get; set; }
    }
}
