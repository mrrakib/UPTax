using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository;
using UPTax.Model.Models;
using UPTax.Model.ViewModels;

namespace UPTax.Service.Services
{
    public interface IInfraStructuralTypeService
    {
        bool Add(InfraStructuralType model);
        bool Update(InfraStructuralType model);
        bool Delete(int id);
        IPagedList GetPagedList(string keyName, int pageNo, int pageSize);
        IEnumerable<InfraStructuralType> GetAll();
        InfraStructuralType GetDetails(int id);
        bool IsExistingItem(string keyName, int? id);
        bool Save();
        InfraStructuralType GetByStaticId(int staticId);
        List<IdNameDropdown> GetAllForDropdown();
    }
    public class InfraStructuralTypeService : IInfraStructuralTypeService
    {
        private readonly IInfraStructuralTypeRepository _infraStructuralTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InfraStructuralTypeService(IInfraStructuralTypeRepository infraStructuralTypeRepository, IUnitOfWork unitOfWork)
        {
            _infraStructuralTypeRepository = infraStructuralTypeRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(InfraStructuralType model)
        {
            _infraStructuralTypeRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            InfraStructuralType model = _infraStructuralTypeRepository.GetById(id);
            model.IsDeleted = true;

            _infraStructuralTypeRepository.Update(model);
            return Save();
        }
        public bool Update(InfraStructuralType model)
        {
            _infraStructuralTypeRepository.Update(model);
            return Save();
        }
        public IEnumerable<InfraStructuralType> GetAll()
        {
            return _infraStructuralTypeRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public InfraStructuralType GetDetails(int id)
        {
            return _infraStructuralTypeRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public IPagedList GetPagedList(string keyName, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(keyName))
                {
                    searchPrm += string.Format(@" WHERE TypeName LIKE N'%{0}%' AND IsDeleted = 0", keyName.Trim());
                }
                else
                {
                    searchPrm += string.Format(@" WHERE IsDeleted = 0");
                }
                string query = string.Format(@"SELECT * FROM InfraStructuralType {0} ORDER BY Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(Id) FROM InfraStructuralType {0}", searchPrm);

                int rowCount = _infraStructuralTypeRepository.SQLQuery<int>(countQuery);
                List<InfraStructuralType> list = _infraStructuralTypeRepository.SQLQueryList<InfraStructuralType>(query).ToList();
                return new StaticPagedList<InfraStructuralType>(list, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<InfraStructuralType>(new List<InfraStructuralType> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(string keyName, int? id)
        {
            var count = 0;
            if (id == null)
                count = _infraStructuralTypeRepository.GetCount(a => a.IsDeleted == false && a.TypeName == keyName.Trim());
            else
                count = _infraStructuralTypeRepository.GetCount(a => a.IsDeleted == false && a.TypeName == keyName.Trim() && a.Id != id);
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

        public InfraStructuralType GetByStaticId(int staticId)
        {
            return _infraStructuralTypeRepository.Get(a => !a.IsDeleted && a.StaticId == staticId);
        }

        public List<IdNameDropdown> GetAllForDropdown()
        {
            var data = new List<IdNameDropdown>()
            {
                new IdNameDropdown(){ IdStr="আবাসিক", Name="আবাসিক"},
                new IdNameDropdown(){ IdStr="বাণিজ্যিক", Name="বাণিজ্যিক"}
            };
            return data;
        }
    }
}
