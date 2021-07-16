using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface ITaxDueWarningRepository : IRepository<TaxDueWarning>
    {

    }
    public class TaxDueWarningRepository : Repository<TaxDueWarning>, ITaxDueWarningRepository
    {
        AdminContext _context;
        public TaxDueWarningRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
