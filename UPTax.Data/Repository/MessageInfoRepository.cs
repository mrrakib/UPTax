using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IMessageInfoRepository : IRepository<MessageInfo>
    {

    }
    public class MessageInfoRepository : Repository<MessageInfo>, IMessageInfoRepository
    {
        AdminContext _context;
        public MessageInfoRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
