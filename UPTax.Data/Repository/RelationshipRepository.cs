using System.Data.Entity;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models;

namespace UPTax.Data.Repository
{
    public interface IRelationshipRepository : IRepository<Relationship>
    {

    }
    public class RelationshipRepository : Repository<Relationship>, IRelationshipRepository
    {
        AdminContext _context;
        public RelationshipRepository(DbContext context) : base(context)
        {
            _context = (AdminContext)context;
        }
    }
}
