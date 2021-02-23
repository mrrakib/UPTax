using UPTax.Data.Infrastructure;
using UPTax.Model.Models.Account;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPTax.Data.Repository.Autofac
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        AdminContext _context;
        public UserRepository(DbContext context)
            : base(context)
        {
            _context = (AdminContext)context;
        }

    }
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        
    }
}
