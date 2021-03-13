using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IHouseOwnerRepository : IRepository<HouseOwner>
    {

    }
    public class HouseOwnerRepository : Repository<HouseOwner>, IHouseOwnerRepository
    {
        AdminContext _context;
        public HouseOwnerRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
