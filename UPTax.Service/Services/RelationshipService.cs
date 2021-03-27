using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository;
using UPTax.Model.Models;

namespace UPTax.Service.Services
{
    public interface IRelationshipService
    {
        bool Add(Relationship model);
        bool Update(Relationship model);
        bool Delete(int id);
        IPagedList GetPagedList(string name, int pageNo, int pageSize);
        IEnumerable<Relationship> GetAll();
        Relationship GetDetails(int id);
        bool IsExistingItem(string name, int? id);
        bool Save();
    }
    public class RelationshipService : IRelationshipService
    {
        private readonly IRelationshipRepository _RelationshipRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RelationshipService(IRelationshipRepository RelationshipRepository, IUnitOfWork unitOfWork)
        {
            _RelationshipRepository = RelationshipRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(Relationship model)
        {
            _RelationshipRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            Relationship wardInfo = _RelationshipRepository.GetById(id);
            wardInfo.IsDeleted = true;

            _RelationshipRepository.Update(wardInfo);
            return Save();
        }
        public bool Update(Relationship model)
        {
            _RelationshipRepository.Update(model);
            return Save();
        }
        public IEnumerable<Relationship> GetAll()
        {
            return _RelationshipRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public Relationship GetDetails(int id)
        {
            return _RelationshipRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public IPagedList GetPagedList(string name, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    searchPrm += string.Format(@" WHERE IsDeleted=0 AND Name LIKE N'%{0}%'", name.Trim());
                }
                else
                {
                    searchPrm += string.Format(@" WHERE IsDeleted=0");
                }
                string query = string.Format(@"SELECT * FROM Relationships {0} ORDER BY Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(*) FROM Relationships WHERE IsDeleted=0 AND Name LIKE N'%{0}%'", name?.Trim());

                int rowCount = _RelationshipRepository.SQLQuery<int>(countQuery);
                List<Relationship> relationship = _RelationshipRepository.SQLQueryList<Relationship>(query).OrderByDescending(a => a.CreatedDate).ToList();
                return new StaticPagedList<Relationship>(relationship, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<Relationship>(new List<Relationship> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(string name, int? id)
        {
            var count = 0;
            if (id == null)
                count = _RelationshipRepository.GetCount(a => a.IsDeleted == false && a.Name == name.Trim());
            else
                count = _RelationshipRepository.GetCount(a => a.IsDeleted == false && a.Name == name.Trim() && a.Id != id);
            return count > 0 ? true : false;
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
                var errorMessage = ex.Message;
                return false;
            }
        }
    }
}
