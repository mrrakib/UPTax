using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface ISocialBenefitRepository : IRepository<SocialBenefit>
    {

    }
    public class SocialBenefitRepository : Repository<SocialBenefit>, ISocialBenefitRepository
    {
        AdminContext _context;
        public SocialBenefitRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
