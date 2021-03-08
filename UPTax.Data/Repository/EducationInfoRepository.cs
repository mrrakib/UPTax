using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IEducationInfoRepository : IRepository<EducationInfo>
    {

    }
    public class EducationInfoRepository : Repository<EducationInfo>, IEducationInfoRepository
    {
        AdminContext _context;
        public EducationInfoRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
