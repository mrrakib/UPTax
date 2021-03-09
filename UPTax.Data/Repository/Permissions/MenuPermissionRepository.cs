using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IMenuPermissionRepository : IRepository<MenuPermission>
    {

    }
    public class MenuPermissionRepository : Repository<MenuPermission>, IMenuPermissionRepository
    {
        AdminContext _context;
        public MenuPermissionRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
