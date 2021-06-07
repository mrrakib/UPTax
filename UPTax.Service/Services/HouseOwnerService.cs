using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository;
using UPTax.Model.Models;
using UPTax.Model.ViewModels;

namespace UPTax.Service.Services
{
    public interface IHouseOwnerService
    {
        bool Add(HouseOwner model);
        bool Update(HouseOwner model);
        bool Delete(int id);
        IPagedList GetPagedList(string holdingNo, int wardId, int villageId, int pageNo, int pageSize);
        IEnumerable<HouseOwner> GetAll();
        HouseOwner GetDetails(int id);
        bool IsExistingItem(string holdingNo, int id = 0);
        bool Save();
        int GetIdByHoldingNum(string holdingNum, int unionId);
        double GetTaxRateByHoldingNum(string holdingNum, int unionId);
        IEnumerable GetTubeWellDropdownItemList();
        IEnumerable GetSanitaryDropdownItemList();
        IEnumerable GetLivingTypeDropdownItemList();
        List<SPDailyPostingReport> GetDailyPostingReport(VMDailyPostingReport model);
    }
    public class HouseOwnerService : IHouseOwnerService
    {
        private readonly IHouseOwnerRepository _HouseOwnerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HouseOwnerService(IHouseOwnerRepository HouseOwnerRepository, IUnitOfWork unitOfWork)
        {
            _HouseOwnerRepository = HouseOwnerRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(HouseOwner model)
        {
            _HouseOwnerRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            HouseOwner wardInfo = _HouseOwnerRepository.GetById(id);
            wardInfo.IsDeleted = true;

            _HouseOwnerRepository.Update(wardInfo);
            return Save();
        }
        public bool Update(HouseOwner model)
        {
            _HouseOwnerRepository.Update(model);
            return Save();
        }
        public IEnumerable<HouseOwner> GetAll()
        {
            return _HouseOwnerRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public HouseOwner GetDetails(int id)
        {
            return _HouseOwnerRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public IPagedList GetPagedList(string holdingNo, int wardId, int villageId, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(holdingNo))
                {
                    searchPrm += string.Format(@" WHERE h.IsDeleted=0 AND h.HoldingNo LIKE N'%{0}%'", holdingNo.Trim());
                }
                else
                {
                    searchPrm += string.Format(@" WHERE h.IsDeleted=0");
                }

                if (wardId > 0)
                {
                    searchPrm += string.Format(@" AND h.WardInfoId = {0}", wardId);
                }
                if (villageId > 0)
                {
                    searchPrm += string.Format(@" AND h.VillageInfoId = {0}", villageId);
                }

                string query = string.Format(@"SELECT h.*, w.WardNo WardName, u.[Name] UnionName, v.VillageName, e.Degree EducationName, g.[Name] GenderName, r.[Name] ReligionName, p.ProfessionTitle ProfessionName, sbr.Title SocialBenefitRunningName, sbe.Title SocialBenefitEligibleName, sbb.Title SocialBenefitBeforeName from HouseOwners h 
                                                JOIN WardInfo w ON h.WardInfoId=w.Id  
                                                JOIN UnionParishad u ON w.UnionId=u.Id
                                                JOIN VillageInfo v ON h.VillageInfoId=v.Id
                                                JOIN Genders g ON h.GenderId=g.Id
                                                JOIN Religions r ON h.ReligionId=r.Id
                                                LEFT JOIN EducationInfo e ON h.EducationInfoId=e.Id
                                                LEFT JOIN ProfessionInfo p ON h.ProfessionId=p.Id
                                                LEFT JOIN SocialBenefits sbr ON h.SocialBenefitRunningId=sbr.Id
                                                LEFT JOIN SocialBenefits sbe ON h.SocialBenefitEligibleId=sbe.Id
                                                LEFT JOIN SocialBenefits sbb ON h.SocialBenefitBeforeId=sbb.Id
                                                {0} ORDER BY h.HoldingNo OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(*) FROM HouseOwners h {0}", searchPrm);

                int rowCount = _HouseOwnerRepository.SQLQuery<int>(countQuery);
                List<VHouseOwner> houseOwner = _HouseOwnerRepository.SQLQueryList<VHouseOwner>(query).ToList();
                return new StaticPagedList<VHouseOwner>(houseOwner, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<VHouseOwner>(new List<VHouseOwner> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(string holdingNo, int id = 0)
        {
            var count = 0;
            if (id == 0)
                count = _HouseOwnerRepository.GetCount(a => a.IsDeleted == false && a.HoldingNo == holdingNo.Trim());
            else
                count = _HouseOwnerRepository.GetCount(a => a.IsDeleted == false && a.HoldingNo == holdingNo.Trim() && a.Id != id);
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

        public int GetIdByHoldingNum(string holdingNum, int unionId)
        {
            int ownerId = 0;
            try
            {
                var owner = _HouseOwnerRepository.Get(h => !h.IsDeleted && h.HoldingNo.Equals(holdingNum) && h.VillageInfo.UnionId == unionId);
                ownerId = owner != null ? owner.Id : 0;
                return ownerId;
            }
            catch (Exception ex)
            {
                return ownerId;
            }

        }

        public double GetTaxRateByHoldingNum(string holdingNum, int unionId)
        {
            double ownerTaxRate = 0;
            try
            {
                var owner = _HouseOwnerRepository.Get(h => !h.IsDeleted && h.HoldingNo.Equals(holdingNum) && h.VillageInfo.UnionId == unionId);
                ownerTaxRate = owner != null ? (double)owner.YearlyInterestRate : 0;
                return ownerTaxRate;
            }
            catch (Exception ex)
            {
                return ownerTaxRate;
            }
        }

        public IEnumerable GetTubeWellDropdownItemList()
        {
            return new List<IdNameDropdown>() {
                new IdNameDropdown() { IdStr = "true", Name = "হ্যাঁ" },
                new IdNameDropdown() { IdStr = "false", Name = "না" }
            };
        }

        public IEnumerable GetSanitaryDropdownItemList()
        {
            return new List<IdNameDropdown>() {
                new IdNameDropdown() { IdStr = "পাকা পায়খানা", Name = "পাকা পায়খানা" },
                new IdNameDropdown() { IdStr = "কাঁচা পায়খানা", Name = "কাঁচা পায়খানা" },
                new IdNameDropdown() { IdStr = "নাই", Name = "নাই" }
            };
        }

        public IEnumerable GetLivingTypeDropdownItemList()
        {
            return new List<IdNameDropdown>() {
                new IdNameDropdown() { IdStr = "নিজে বসবাস", Name = "নিজে বসবাস" },
                new IdNameDropdown() { IdStr = "ভাড়া দেওয়া", Name = "ভাড়া দেওয়া" },
                new IdNameDropdown() { IdStr = "উভয়", Name = "উভয়" }
            };
        }

        public List<SPDailyPostingReport> GetDailyPostingReport(VMDailyPostingReport model)
        {
            var query = $"EXEC DailyPostingReport {model.WardId}, {model.FinancialYearId}, '{model.StartDate.ToString("yyyy-MM-dd")}', '{model.EndDate.ToString("yyyy-MM-dd")}'";
            var data = _HouseOwnerRepository.SQLQueryList<SPDailyPostingReport>(query).ToList();
            return data;
        }
    }
}
