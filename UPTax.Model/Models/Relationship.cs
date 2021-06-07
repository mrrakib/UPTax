using System.ComponentModel.DataAnnotations.Schema;

namespace UPTax.Model.Models
{
    [Table("Relationships")]
    public class Relationship : BaseEntity<int>
    {
        public string Name { get; set; }
    }
}
