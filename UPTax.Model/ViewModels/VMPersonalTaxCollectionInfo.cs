using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMPersonalTaxCollectionInfo
    {
        public DateTime? CollectionDate { get; set; }
        public decimal CollectedTaxAmount { get; set; }
        public decimal CollectedDueAmount { get; set; }
    }
}
