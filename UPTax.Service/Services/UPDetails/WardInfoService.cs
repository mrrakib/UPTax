using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository;
using UPTax.Data.Repository.UPDetails;
using UPTax.Model.Models;
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
        List<VMTranInfo> GetTranInfo(int unionId);
    }
    public class WardInfoService : IWardInfoService
    {
        private readonly IWardInfoRepository _wardInfoRepository;
        private readonly IHouseOwnerRepository _houseOwnerRepository;
        private readonly IUnitOfWork _unitOfWork;
        public WardInfoService(IWardInfoRepository wardInfoRepository, IUnitOfWork unitOfWork, IHouseOwnerRepository houseOwnerRepository)
        {
            _wardInfoRepository = wardInfoRepository;
            _unitOfWork = unitOfWork;
            _houseOwnerRepository = houseOwnerRepository;
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
                //var culture = new CultureInfo("bn-BD");
                int rowCount = _wardInfoRepository.SQLQuery<int>(countQuery);
                var unionParishads = _wardInfoRepository.SQLQueryList<WardInfo>(query).Where(a => a.IsDeleted == false).ToList();
                foreach (var item in unionParishads)
                {
                    item.WardNoEng = Convert.ToInt32(E2B.SwitchEngBan(item.WardNo));
                }

                //var result = unionParishads.OrderBy(s => s.WardNo, StringComparer.Create(culture, false));
                var result = unionParishads.OrderBy(s => s.WardNoEng);
                return new StaticPagedList<WardInfo>(result, pageNo, pageSize, rowCount);
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

        public List<VMTranInfo> GetTranInfo(int unionId)
        {
            List<VMTranInfo> tranInfos = new List<VMTranInfo>();
            List<WardInfo> allWard = _wardInfoRepository.GetMany(w => w.IsDeleted == false && w.UnionId == unionId).ToList();
            List<HouseOwner> houseOwners = _houseOwnerRepository.GetMany(h => h.IsDeleted == false && h.VillageInfo.UnionId == unionId).ToList();
            if (allWard.Count > 0 && houseOwners.Count > 0)
            {
                foreach (var ward in allWard)
                {
                    
                    var owners = houseOwners.Where(h => h.WardInfoId == ward.Id).ToList();
                    int totalMaleCount = owners.Where(h => h.GenderId == 1).Count();
                    int totalFeMaleCount = owners.Where(h => h.GenderId == 2).Count();
                    int totalBenefitCount = owners.Where(h => h.SocialBenefitRunningId != null).Count();
                    var richPeople = owners.Where(h => h.TotalBuildingHouse != null).ToList();
                    var midPoorPeople = owners.Where(h => h.TotalSemiBuildingHouse != null && !richPeople.Any(r => r.Id == h.Id)).ToList();
                    var poorPeople = owners.Where(h => h.TotalRawHouse != null && !richPeople.Any(r => r.Id == h.Id) && !midPoorPeople.Any(r => r.Id == h.Id)).ToList();


                    int totalPoor = poorPeople.Count;
                    int totalSemiPoor = midPoorPeople.Count;
                    int totalRich = richPeople.Count;
                    int totalPeople = owners.Count;

                    VMTranInfo tranInfo = new VMTranInfo
                    {
                        WardNo = ward.WardNo,
                        TotalHouseOwner = totalPeople,
                        TotalSocialBenefitTakingCount = totalBenefitCount,
                        TotalMale = totalMaleCount,
                        TotalFemale = totalFeMaleCount,
                        TotalPoor = totalPoor,
                        TotalMidPoor = totalSemiPoor,
                        TotalRich = totalRich,
                        TotalPopulation = totalPeople
                    };
                    tranInfos.Add(tranInfo);
                }
            }
            return tranInfos;

        }
    }
}
