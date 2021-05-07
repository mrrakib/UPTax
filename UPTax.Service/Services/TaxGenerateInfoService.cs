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
    public interface ITaxGenerateInfoService
    {
        bool Add(TaxGenerateInfo model);
        bool Update(TaxGenerateInfo model);
        bool Delete(int id);
        IEnumerable<TaxGenerateInfo> GetAll();
        TaxGenerateInfo GetDetails(int id);
        bool IsExistingItem(int financialYearId, int houseOwnerId, int? id);
        bool Save();
        VMTaxGenerator GenerateSingleTax(string holdingNo);
    }
    public class TaxGenerateInfoService : ITaxGenerateInfoService
    {
        private readonly ITaxGenerateInfoRepository _taxGenerateInfoRepository;
        private readonly IHouseOwnerRepository _houseOwnerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TaxGenerateInfoService(ITaxGenerateInfoRepository taxGenerateInfoRepository, IUnitOfWork unitOfWork, IHouseOwnerRepository houseOwnerRepository)
        {
            _taxGenerateInfoRepository = taxGenerateInfoRepository;
            _unitOfWork = unitOfWork;
            _houseOwnerRepository = houseOwnerRepository;
        }
        public bool Add(TaxGenerateInfo model)
        {
            _taxGenerateInfoRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            TaxGenerateInfo model = _taxGenerateInfoRepository.GetById(id);
            model.IsDeleted = true;

            _taxGenerateInfoRepository.Update(model);
            return Save();
        }
        public bool Update(TaxGenerateInfo model)
        {
            _taxGenerateInfoRepository.Update(model);
            return Save();
        }
        public IEnumerable<TaxGenerateInfo> GetAll()
        {
            return _taxGenerateInfoRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public TaxGenerateInfo GetDetails(int id)
        {
            return _taxGenerateInfoRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public bool IsExistingItem(int financialYearId, int houseOwnerId, int? id)
        {
            var count = 0;
            if (id == null)
                count = _taxGenerateInfoRepository.GetCount(a => a.IsDeleted == false && a.FinancialYearId == financialYearId && a.HouseOwnerId == houseOwnerId);
            else
                count = _taxGenerateInfoRepository.GetCount(a => a.IsDeleted == false && a.FinancialYearId == financialYearId && a.HouseOwnerId == houseOwnerId && a.Id != id);

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

        public VMTaxGenerator GenerateSingleTax(string holdingNo)
        {
            VMTaxGenerator result = new VMTaxGenerator();
            VMTaxGeneratorDetails resultDetails = new VMTaxGeneratorDetails();
            HouseOwner houseOwner = _houseOwnerRepository.Get(h => h.HoldingNo.Equals(holdingNo) && h.IsDeleted == false);

            if (houseOwner != null)
            {
                double yearlyRent = houseOwner.YearlyRentAmount ?? 0;
                resultDetails = new VMTaxGeneratorDetails
                {
                    HouseOwnerId = houseOwner.Id,
                    HouseOwnerName = houseOwner.OwnerNameInBangla,
                    TotalYearlyRent = yearlyRent,
                    TotalYearlyTax = ((yearlyRent * (houseOwner.YearlyInterestRate ?? 0)) / 100)
                };
                result = new VMTaxGenerator
                {
                    HoldingNo = houseOwner.HoldingNo,
                    YearlyTaxRate = (houseOwner.YearlyInterestRate ?? 0),
                    VMTaxGeneratorDetails = resultDetails
                };
                return result;
            }
            return result;
        }

        public List<VMAllTaxGenerator> GenerateAllTax(int villId, int wardId, int unionId)
        {
            List<VMAllTaxGenerator> result = new List<VMAllTaxGenerator>();
            List<HouseOwner> houseOwnerList = _houseOwnerRepository.GetMany(h => h.VillageInfoId == villId && h.WardInfoId == wardId && h.VillageInfo.UnionId == unionId && h.IsDeleted == false).ToList();

            if (houseOwnerList.Count > 0)
            {
                foreach (var houseOwner in houseOwnerList)
                {
                    double yearlyRent = houseOwner.YearlyRentAmount ?? 0;
                    VMAllTaxGenerator singleResult = new VMAllTaxGenerator
                    {
                        HoldingNo = houseOwner.HoldingNo,
                        HouseOwnerId = houseOwner.Id,
                        TotalYearlyRent = yearlyRent,
                        TotalYearlyTax = ((yearlyRent * (houseOwner.YearlyInterestRate ?? 0)) / 100)
                    };
                    result.Add(singleResult);
                }
                return result;
            }
            return result;
        }
    }
}
