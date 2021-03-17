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
    public interface IVillageInfoService
    {
        bool Add(VillageInfo model);
        bool Update(VillageInfo model);
        bool Delete(int id);
        IPagedList GetPagedList(string keyName, int pageNo, int pageSize);
        IEnumerable<VillageInfo> GetAll();
        VillageInfo GetDetails(int id);
        bool IsExistingItem(string keyName, int? id);
        bool Save();
        List<IdNameDropdown> GetDropdownItemList(int unionId);
        List<VillageInfo> GetByWardId(int wardId);
    }
    public class VillageInfoService : IVillageInfoService
    {
        private readonly IVillageInfoRepository _VillageInfoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VillageInfoService(IVillageInfoRepository VillageInfoRepository, IUnitOfWork unitOfWork)
        {
            _VillageInfoRepository = VillageInfoRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(VillageInfo model)
        {
            _VillageInfoRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            VillageInfo wardInfo = _VillageInfoRepository.GetById(id);
            wardInfo.IsDeleted = true;

            _VillageInfoRepository.Update(wardInfo);
            return Save();
        }
        public bool Update(VillageInfo model)
        {
            _VillageInfoRepository.Update(model);
            return Save();
        }
        public IEnumerable<VillageInfo> GetAll()
        {
            return _VillageInfoRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public VillageInfo GetDetails(int id)
        {
            return _VillageInfoRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public IPagedList GetPagedList(string keyName, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(keyName))
                {
                    searchPrm += string.Format(@" WHERE VillageName LIKE N'%{0}%' AND IsDeleted = 0", keyName.Trim());
                }
                else
                {
                    searchPrm += string.Format(@" WHERE IsDeleted = 0");
                }
                string query = string.Format(@"SELECT v.Id, v.VillageName, v.CreatedDate, v.CreatedBy, v.UpdatedDate, v.UpdatedBy, v.IsDeleted, u.Name as UnionName,v.UnionId FROM VillageInfo v JOIN [dbo].[UnionParishad] u ON v.UnionId=u.Id {0} ORDER BY Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(Id) FROM VillageInfo WHERE VillageName LIKE N'%{0}%'", keyName?.Trim());

                int rowCount = _VillageInfoRepository.SQLQuery<int>(countQuery);
                List<VVillageInfo> villages = _VillageInfoRepository.SQLQueryList<VVillageInfo>(query).ToList();
                return new StaticPagedList<VVillageInfo>(villages, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<VVillageInfo>(new List<VVillageInfo> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(string keyName, int? id)
        {
            var count = 0;
            if (id == null)
                count = _VillageInfoRepository.GetCount(a => a.IsDeleted == false && a.VillageName == keyName.Trim());
            else
                count = _VillageInfoRepository.GetCount(a => a.IsDeleted == false && a.VillageName == keyName.Trim() && a.Id != id);
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

        public List<IdNameDropdown> GetDropdownItemList(int unionId)
        {
            return _VillageInfoRepository.GetMany(w => w.IsDeleted == false && w.UnionId == unionId).Select(u => new IdNameDropdown
            {
                Id = u.Id,
                Name = u.VillageName
            }).ToList();
        }

        public List<VillageInfo> GetByWardId(int wardId)
        {
            return _VillageInfoRepository.GetMany(a => !a.IsDeleted).ToList();
        }
    }
}
