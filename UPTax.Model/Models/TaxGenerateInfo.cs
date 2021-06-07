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
    [Table("TaxGenerateInfo")]
    public class TaxGenerateInfo : BaseEntity<int>
    {
        [StringLength(200)]
        [DisplayName("হোল্ডিং নাম্বার")]
        public string HoldingNo { get; set; }
        [DisplayName("খানা প্রধান")]
        public int HouseOwnerId { get; set; }
        [DisplayName("ইউনিয়ন পরিষদ")]
        public int? UnionId { get; set; }
        [DisplayName("অর্থ বছর")]
        public int FinancialYearId { get; set; }
        [DisplayName("বাৎসরিক মুনাফার হার")]
        public double TaxPercentage { get; set; }
        [DisplayName("বাৎসরিক সর্বমোট কর")]
        public double TotalTax { get; set; }

        [ForeignKey("UnionId")]
        public virtual UnionParishad UnionParishad { get; set; }
        [ForeignKey("FinancialYearId")]
        public virtual FinancialYear FinancialYear { get; set; }
        [ForeignKey("HouseOwnerId")]
        public virtual HouseOwner HouseOwner { get; set; }

    }
}
