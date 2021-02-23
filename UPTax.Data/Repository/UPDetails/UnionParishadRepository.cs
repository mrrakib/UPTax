using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPTax.Data.Infrastructure;
using UPTax.Model.Models.UnionDetails;

namespace UPTax.Data.Repository.UPDetails
{
    public class UnionParishadRepository : Repository<UnionParishad>, IUnionParishadRepository
    {
        AdminContext _context;
        public UnionParishadRepository(DbContext context) 
            : base(context)
        {
            _context = (AdminContext)context;
        }
    }

    public interface IUnionParishadRepository : IRepository<UnionParishad>
    {

    }
}
