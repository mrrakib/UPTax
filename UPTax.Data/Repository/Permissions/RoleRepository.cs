using UPTax.Data.Infrastructure;
using UPTax.Model.Models.Account;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Data.Repository
{
    public class RoleRepository : Repository<ApplicationRole>, IRoleRepository
    {
        AdminContext _context;
        public RoleRepository(DbContext context)
            : base(context)
        {
            _context = (AdminContext)context;
        }

    }
    public interface IRoleRepository : IRepository<ApplicationRole>
    {
        
    }
}
