using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IInfraStructuralTypeRepository : IRepository<InfraStructuralType>
    {

    }
    public class InfraStructuralTypeRepository : Repository<InfraStructuralType>, IInfraStructuralTypeRepository
    {
        AdminContext _context;
        public InfraStructuralTypeRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
