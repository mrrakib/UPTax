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
    public interface IInstituteInfoService
    {
        bool Add(InstituteInfo model);
        bool Update(InstituteInfo model);
        bool Delete(int id);
        IPagedList GetPagedList(string keyName, int pageNo, int pageSize);
        IEnumerable<InstituteInfo> GetAll();
        InstituteInfo GetDetails(int id);
        bool IsExistingItem(string keyName, int? id);
        bool Save();
    }
    public class InstituteInfoService : IInstituteInfoService
    {
        private readonly IInstituteInfoRepository _InstituteInfoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InstituteInfoService(IInstituteInfoRepository InstituteInfoRepository, IUnitOfWork unitOfWork)
        {
            _InstituteInfoRepository = InstituteInfoRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(InstituteInfo model)
        {
            _InstituteInfoRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            InstituteInfo wardInfo = _InstituteInfoRepository.GetById(id);
            wardInfo.IsDeleted = true;

            _InstituteInfoRepository.Update(wardInfo);
            return Save();
        }
        public bool Update(InstituteInfo model)
        {
            _InstituteInfoRepository.Update(model);
            return Save();
        }
        public IEnumerable<InstituteInfo> GetAll()
        {
            return _InstituteInfoRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public InstituteInfo GetDetails(int id)
        {
            return _InstituteInfoRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public IPagedList GetPagedList(string keyName, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(keyName))
                {
                    searchPrm += string.Format(@" WHERE i.HoldingNo LIKE N'%{0}%'", keyName.Trim());
                }
                string query = string.Format(@"SELECT i.Id, i.HoldingNo, i.InstituteType, i.IsTubeWell, i.MobileNo, i.NameOfInstituteBangla, i.NameOfInstituteEnglish, i.PreviousDueAmount,
i.Sanitary, i.TotalBuildingHouse, i.TotalRawHouse, i.TotalSemiBuildingHouse, i.YearlyIncome,i.DateOfEstablishment,
w.WardNo, v.VillageName FROM InstituteInfo i
JOIN WardInfo w ON i.WardInfoId=w.Id
JOIN VillageInfo v ON i.VillageInfoId=v.Id {0} AND i.IsDeleted=0 ORDER BY Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(Id) FROM InstituteInfo WHERE IsDeleted=0 AND HoldingNo LIKE N'%{0}%'", keyName?.Trim());

                int rowCount = _InstituteInfoRepository.SQLQuery<int>(countQuery);
                List<VMInstituteInfo> educations = _InstituteInfoRepository.SQLQueryList<VMInstituteInfo>(query).ToList();
                return new StaticPagedList<VMInstituteInfo>(educations, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<InstituteInfo>(new List<InstituteInfo> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(string keyName, int? id)
        {
            var count = 0;
            if (id == null)
                count = _InstituteInfoRepository.GetCount(a => a.IsDeleted == false && a.HoldingNo == keyName.Trim());
            else
                count = _InstituteInfoRepository.GetCount(a => a.IsDeleted == false && a.HoldingNo == keyName.Trim() && a.Id != id);
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
