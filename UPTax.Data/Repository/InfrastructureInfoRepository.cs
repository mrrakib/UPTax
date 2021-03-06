using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IInfrastructureInfoRepository : IRepository<InfrastructureInfo>
    {

    }
    public class InfrastructureInfoRepository : Repository<InfrastructureInfo>, IInfrastructureInfoRepository
    {
        AdminContext _context;
        public InfrastructureInfoRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
