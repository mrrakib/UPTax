using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository.UPDetails;
using UPTax.Model.Models.UnionDetails;
using UPTax.Model.ViewModels;

namespace UPTax.Service.Services.UPDetails
{
    public interface IWardInfoService
    {
        IEnumerable<WardInfo> GetAll();
        WardInfo GetDetails(int id);
        bool Add(WardInfo model);
        bool Update(WardInfo model);
        bool Save();
        bool Delete(int id);
        IPagedList GetPagedList(string wardNo, int unionId, int pageNo, int pageSize);
        bool IsExistingWard(string wardNo, int unionId, int? wardId);
        List<IdNameDropdown> GetDropdownItemList(int unionId);
        List<SPWardVillageWiseDueReport> GetWardVillageWiseReport(int wardId, int villageId, string infrastructureType, int financialYearId);
    }
    public class WardInfoService : IWardInfoService
    {
        private readonly IWardInfoRepository _wardInfoRepository;
        private readonly IUnitOfWork _unitOfWork;
        public WardInfoService(IWardInfoRepository wardInfoRepository, IUnitOfWork unitOfWork)
        {
            _wardInfoRepository = wardInfoRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(WardInfo model)
        {
            _wardInfoRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            WardInfo wardInfo = _wardInfoRepository.GetById(id);
            wardInfo.IsDeleted = true;

            _wardInfoRepository.Update(wardInfo);
            return Save();
        }

        public IEnumerable<WardInfo> GetAll()
        {
            return _wardInfoRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public WardInfo GetDetails(int id)
        {
            return _wardInfoRepository.Get(u => u.Id == id && u.IsDeleted == false);
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

        public bool Update(WardInfo model)
        {
            _wardInfoRepository.Update(model);
            return Save();
        }

        public IPagedList GetPagedList(string wardNo, int unionId, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;

                if (!string.IsNullOrEmpty(wardNo))
                {
                    searchPrm += string.Format(@" WHERE UnionId={0} AND WardNo LIKE N'%{1}%' AND IsDeleted = 0", unionId, wardNo);
                }
                else
                {
                    searchPrm += string.Format(@" WHERE UnionId={0} AND IsDeleted = 0", unionId, unionId);
                }
                string query = string.Format(@"SELECT * FROM WardInfo {0} ORDER BY Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(*) FROM WardInfo WHERE UnionId={0} AND WardNo LIKE N'%{1}%'", unionId, wardNo);

                int rowCount = _wardInfoRepository.SQLQuery<int>(countQuery);
                List<WardInfo> unionParishads = _wardInfoRepository.SQLQueryList<WardInfo>(query).Where(a => a.IsDeleted == false).ToList();
                return new StaticPagedList<WardInfo>(unionParishads, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<WardInfo>(new List<WardInfo> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingWard(string wardNo, int unionId, int? wardId)
        {
            if (wardId == null || wardId == 0)
                return _wardInfoRepository.GetCount(a => a.WardNo.Equals(wardNo) && a.UnionId == unionId && a.IsDeleted == false) > 0 ? true : false;

            return _wardInfoRepository.GetCount(a => a.WardNo.Equals(wardNo) && a.UnionId == unionId && a.IsDeleted == false && a.Id != wardId) > 0 ? true : false;
        }

        public List<IdNameDropdown> GetDropdownItemList(int unionId)
        {
            return _wardInfoRepository.GetMany(w => w.IsDeleted == false && w.UnionId == unionId).Select(u => new IdNameDropdown
            {
                Id = u.Id,
                Name = u.WardNo
            }).ToList();
        }

        public List<SPWardVillageWiseDueReport> GetWardVillageWiseReport(int wardId, int villageId, string infrastructureType, int financialYearId)
        {
            var query = $"EXEC WardVillageWiseDueReport {wardId}, {villageId}, '{infrastructureType}', {financialYearId}";
            var data = _wardInfoRepository.SQLQueryList<SPWardVillageWiseDueReport>(query).ToList();
            return data;
        }
    }
}
