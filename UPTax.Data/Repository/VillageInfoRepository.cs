using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IVillageInfoRepository : IRepository<VillageInfo>
    {

    }
    public class VillageInfoRepository : Repository<VillageInfo>, IVillageInfoRepository
    {
        AdminContext _context;
        public VillageInfoRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
