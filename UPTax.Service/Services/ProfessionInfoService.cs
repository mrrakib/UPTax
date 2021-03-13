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
    public interface IProfessionInfoService
    {
        bool Add(ProfessionInfo model);
        bool Update(ProfessionInfo model);
        bool Delete(int id);
        IPagedList GetPagedList(string degree, int pageNo, int pageSize);
        IEnumerable<ProfessionInfo> GetAll();
        ProfessionInfo GetDetails(int id);
        bool IsExistingItem(string keyName, int? id);
        bool Save();
        List<IdNameDropdown> GetDropdownItemList();
    }
    public class ProfessionInfoService : IProfessionInfoService
    {
        private readonly IProfessionInfoRepository _professionInfoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProfessionInfoService(IProfessionInfoRepository professionInfoRepository, IUnitOfWork unitOfWork)
        {
            _professionInfoRepository = professionInfoRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(ProfessionInfo model)
        {
            _professionInfoRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            ProfessionInfo wardInfo = _professionInfoRepository.GetById(id);
            wardInfo.IsDeleted = true;

            _professionInfoRepository.Update(wardInfo);
            return Save();
        }
        public bool Update(ProfessionInfo model)
        {
            _professionInfoRepository.Update(model);
            return Save();
        }
        public IEnumerable<ProfessionInfo> GetAll()
        {
            return _professionInfoRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public ProfessionInfo GetDetails(int id)
        {
            return _professionInfoRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public IPagedList GetPagedList(string keyName, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(keyName))
                {
                    searchPrm += string.Format(@" WHERE ProfessionTitle LIKE N'%{0}%'", keyName.Trim());
                }
                string query = string.Format(@"SELECT * FROM ProfessionInfo {0} ORDER BY Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(*) FROM ProfessionInfo WHERE ProfessionTitle LIKE N'%{0}%'", keyName?.Trim());

                int rowCount = _professionInfoRepository.SQLQuery<int>(countQuery);
                List<ProfessionInfo> educations = _professionInfoRepository.SQLQueryList<ProfessionInfo>(query).Where(a => a.IsDeleted == false).ToList();
                return new StaticPagedList<ProfessionInfo>(educations, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<ProfessionInfo>(new List<ProfessionInfo> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(string keyName, int? id)
        {
            var count = 0;
            if (id == null)
                count = _professionInfoRepository.GetCount(a => a.IsDeleted == false && a.ProfessionTitle == keyName.Trim());
            else
                count = _professionInfoRepository.GetCount(a => a.IsDeleted == false && a.ProfessionTitle == keyName.Trim() && a.Id != id);
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
            return _professionInfoRepository.GetMany(w => w.IsDeleted == false).Select(u => new IdNameDropdown
            {
                Id = u.Id,
                Name = u.ProfessionTitle
            }).ToList();
        }

    }
}
