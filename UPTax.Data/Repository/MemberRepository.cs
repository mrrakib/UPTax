using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IMemberRepository : IRepository<Member>
    {

    }
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        AdminContext _context;
        public MemberRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
