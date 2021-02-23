using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository.UPDetails;
using UPTax.Model.Models.UnionDetails;

namespace UPTax.Service.Services.UPDetails
{
    public interface IUnionParishadService
    {
        IEnumerable<UnionParishad> GetAll();
        UnionParishad GetByName(string unionName);
        UnionParishad GetDetails(int id);
        bool Add(UnionParishad model);
        bool Update(UnionParishad model);
        bool Save();
        bool Delete(int id);
        IPagedList GetPaged(string name, int pageNo, int pageSize);
    }
    public class UnionParishadService : IUnionParishadService
    {
        private readonly IUnionParishadRepository _unionParishadRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UnionParishadService(IUnionParishadRepository unionParishadRepository, IUnitOfWork unitOfWork)
        {
            _unionParishadRepository = unionParishadRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(UnionParishad model)
        {
            _unionParishadRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            UnionParishad unionParishad = _unionParishadRepository.GetById(id);
            _unionParishadRepository.Delete(unionParishad);
            return Save();
        }

        public IEnumerable<UnionParishad> GetAll()
        {
            return _unionParishadRepository.GetAll();
        }

        public UnionParishad GetByName(string unionName)
        {
            return _unionParishadRepository.Get(u => u.Name.Equals(unionName));
        }

        public UnionParishad GetDetails(int id)
        {
            return _unionParishadRepository.Get(u => u.Id == id);
        }

        public bool Save()
        {
            try
            {
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(UnionParishad model)
        {
            _unionParishadRepository.Update(model);
            return Save();
        }

        public IPagedList GetPaged(string name, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrEmpty(name))
                {
                    searchPrm += string.Format(@" WHERE Name LIKE N'%{0}%'", name);
                }
                string query = string.Format(@"SELECT * FROM UnionParishad {0}
                ORDER BY Id
                OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(*) FROM UnionParishad WHERE Name LIKE N'%{0}%'", name);

                int rowCount = _unionParishadRepository.SQLQuery<int>(countQuery);
                List<UnionParishad> unionParishads = _unionParishadRepository.SQLQueryList<UnionParishad>(query).ToList();
                //var result = unionParishads.OrderBy(u => u.Id).Skip((pageNo - 1) * pageSize).Take(pageSize);
                return new StaticPagedList<UnionParishad>(unionParishads, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                return new StaticPagedList<UnionParishad>(new List<UnionParishad> { }, pageNo, pageSize, 0);
            }
            
        }
    }
}
