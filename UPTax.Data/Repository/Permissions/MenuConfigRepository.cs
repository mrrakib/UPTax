using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IMenuConfigRepository : IRepository<MenuConfig>
    {

    }
    public class MenuConfigRepository : Repository<MenuConfig>, IMenuConfigRepository
    {
        AdminContext _context;
        public MenuConfigRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
