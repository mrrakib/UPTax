using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IGenderRepository : IRepository<Gender>
    {

    }
    public class GenderRepository : Repository<Gender>, IGenderRepository
    {
        AdminContext _context;
        public GenderRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
