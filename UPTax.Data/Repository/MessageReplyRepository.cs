using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IMessageReplyRepository : IRepository<MessageReply>
    {

    }
    public class MessageReplyRepository : Repository<MessageReply>, IMessageReplyRepository
    {
        AdminContext _context;
        public MessageReplyRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
