using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface ITaxInstallmentRepository : IRepository<TaxInstallment>
    {

    }
    public class TaxInstallmentRepository : Repository<TaxInstallment>, ITaxInstallmentRepository
    {
        AdminContext _context;
        public TaxInstallmentRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
