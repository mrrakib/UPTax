using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IMenuCategoryRepository : IRepository<MenuCategory>
    {

    }
    public class MenuCategoryRepository : Repository<MenuCategory>, IMenuCategoryRepository
    {
        AdminContext _context;
        public MenuCategoryRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
