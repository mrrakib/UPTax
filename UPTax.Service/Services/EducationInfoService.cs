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
    public interface IEducationInfoService
    {
        bool Add(EducationInfo model);
        bool Update(EducationInfo model);
        bool Delete(int id);
        IPagedList GetPagedList(string degree, int pageNo, int pageSize);
        IEnumerable<EducationInfo> GetAll();
        EducationInfo GetDetails(int id);
        bool IsExistingItem(string degree, int? id);
        bool Save();
        List<IdNameDropdown> GetDropdownItemList();
    }
    public class EducationInfoService : IEducationInfoService
    {
        private readonly IEducationInfoRepository _educationInfoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EducationInfoService(IEducationInfoRepository educationInfoRepository, IUnitOfWork unitOfWork)
        {
            _educationInfoRepository = educationInfoRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(EducationInfo model)
        {
            _educationInfoRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            EducationInfo wardInfo = _educationInfoRepository.GetById(id);
            wardInfo.IsDeleted = true;

            _educationInfoRepository.Update(wardInfo);
            return Save();
        }
        public bool Update(EducationInfo model)
        {
            _educationInfoRepository.Update(model);
            return Save();
        }
        public IEnumerable<EducationInfo> GetAll()
        {
            return _educationInfoRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public EducationInfo GetDetails(int id)
        {
            return _educationInfoRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public IPagedList GetPagedList(string degree, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(degree))
                {
                    searchPrm += string.Format(@" WHERE Degree LIKE N'%{0}%'", degree.Trim());
                }
                string query = string.Format(@"SELECT * FROM EducationInfo {0} ORDER BY Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(*) FROM EducationInfo WHERE Degree LIKE N'%{0}%'", degree?.Trim());

                int rowCount = _educationInfoRepository.SQLQuery<int>(countQuery);
                List<EducationInfo> educations = _educationInfoRepository.SQLQueryList<EducationInfo>(query).Where(a => a.IsDeleted == false).ToList();
                return new StaticPagedList<EducationInfo>(educations, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<EducationInfo>(new List<EducationInfo> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(string degree, int? id)
        {
            var count = 0;
            if (id == null)
                count = _educationInfoRepository.GetCount(a => a.IsDeleted == false && a.Degree == degree.Trim());
            else
                count = _educationInfoRepository.GetCount(a => a.IsDeleted == false && a.Degree == degree.Trim() && a.Id != id);

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

        public List<IdNameDropdown> GetDropdownItemList()
        {
            return _educationInfoRepository.GetMany(w => w.IsDeleted == false).Select(u => new IdNameDropdown
            {
                Id = u.Id,
                Name = u.Degree
            }).ToList();
        }

    }
}
