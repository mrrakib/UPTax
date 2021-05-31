using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Model.ViewModels
{
    public class VMPersonalStatement
    {
        public string HoldingNo { get; set; }
        public string FullName { get; set; }
        public string ParentName { get; set; }
        public string WordNo { get; set; }
        public string Village { get; set; }
        public string MobileNo { get; set; }
        public virtual VMPersonalTaxDetails PersonalTaxDetails { get; set; }
        public virtual VMPersonalTaxCollectionInfo PersonalTaxCollectionInfo { get; set; }
    }
}
