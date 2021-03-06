using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IInstituteInfoRepository : IRepository<InstituteInfo>
    {

    }
    public class InstituteInfoRepository : Repository<InstituteInfo>, IInstituteInfoRepository
    {
        AdminContext _context;
        public InstituteInfoRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
