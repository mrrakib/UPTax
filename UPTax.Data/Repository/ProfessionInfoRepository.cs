using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IProfessionInfoRepository : IRepository<ProfessionInfo>
    {

    }
    public class ProfessionInfoRepository : Repository<ProfessionInfo>, IProfessionInfoRepository
    {
        AdminContext _context;
        public ProfessionInfoRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
