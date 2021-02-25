using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UPTax.Model.Models.Account;

namespace UPTax.Model
{
    public abstract class BaseEntity<T>
    {
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
            IsDeleted = false;
        }

        [Key]
        public T Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual ApplicationUser CreatedUser { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual ApplicationUser UpdatedUser { get; set; }
    }
}