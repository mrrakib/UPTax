using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IAdminNoticeRepository : IRepository<AdminNotice>
    {

    }
    public class AdminNoticeRepository : Repository<AdminNotice>, IAdminNoticeRepository
    {
        AdminContext _context;
        public AdminNoticeRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
