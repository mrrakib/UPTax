using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models.UnionDetails;

namespace UPTax.Data.Repository.UPDetails
{
    public interface IWardInfoRepository : IRepository<WardInfo>
    {

    }
    public class WardInfoRepository : Repository<WardInfo>, IWardInfoRepository
    {
        AdminContext _context;
        public WardInfoRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
