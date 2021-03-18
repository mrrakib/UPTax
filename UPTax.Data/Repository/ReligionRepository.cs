using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IReligionRepository : IRepository<Religion>
    {

    }
    public class ReligionRepository : Repository<Religion>, IReligionRepository
    {
        AdminContext _context;
        public ReligionRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
