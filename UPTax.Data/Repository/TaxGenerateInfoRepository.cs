using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;
using UPTax.Model.ViewModels;
using System.Linq;

namespace UPTax.Data.Repository
{
    public interface ITaxGenerateInfoRepository : IRepository<TaxGenerateInfo>
    {

    }
    public class TaxGenerateInfoRepository : Repository<TaxGenerateInfo>, ITaxGenerateInfoRepository
    {
        AdminContext _context;
        public TaxGenerateInfoRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
