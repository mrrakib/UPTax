using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository;
using UPTax.Model.Models;
using UPTax.Model.ViewModels;

namespace UPTax.Service.Services
{
    public interface ITaxInstallmentService
    {
        bool Add(TaxInstallment model);
        bool Update(TaxInstallment model);
        bool Delete(int id);
        IEnumerable<TaxInstallment> GetAll();
        TaxInstallment GetDetails(int id);
        bool IsExistingItem(string keyName, int finYearId, int? id);
        bool Save();
        VMTaxInstallment GenerateSingleTaxInstallment(string holdingNo, int finYearId);
        VMRPTTaxCollectionSingle GetMRPTTaxCollectionSingle(string holdingNo, int finYearId, int unionId);
        VMTaxInstallment GeteSingleTaxInstallment(string holdingNo, int finYearId);
        IEnumerable<SPTopSheetReport> GetTopSheetReport(string financialYear);
        List<VMRPTTaxReceipt> GetRPTTaxReceipt(int villageId, int wardId, int finYearId, int unionId);
        List<VMWordOrVillWiseTaxReport> GetRPTTaxInfoByWord(int villageId, int wardId, int finYearId, int unionId);
        List<VMWordOrVillWiseTaxReport> GetRPTTaxInfoByVillage(int villageId, int finYearId, int unionId);
        VMPersonalShortReport GetPersonalShortReport(int unionId, string holdingNo);
        VMPersonalStatement GetPersonalStatementReport(int unionId, string holdinNo);
        List<VMRPTTaxCollectionSingle> GetMRPTTaxCollectionAll(int wardId, int villageId, int finYearId, int unionId);
        VMRPTTaxCollectionSingle GetRPTTaxDueNotice(string holdingNo, int finYearId, int unionId);
    }
    public class TaxInstallmentService : ITaxInstallmentService
    {
        private readonly ITaxInstallmentRepository _taxInstallmentRepository;
        private readonly IHouseOwnerRepository _houseOwnerRepository;
        private readonly ITaxGenerateInfoRepository _taxGenerateInfoRepository;
        private readonly IFinancialYearRepository _financialYearRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TaxInstallmentService(ITaxInstallmentRepository taxInstallmentRepository, IHouseOwnerRepository houseOwnerRepository, IUnitOfWork unitOfWork, ITaxGenerateInfoRepository taxGenerateInfoRepository, IFinancialYearRepository financialYearRepository)
        {
            _taxInstallmentRepository = taxInstallmentRepository;
            _houseOwnerRepository = houseOwnerRepository;
            _unitOfWork = unitOfWork;
            _taxGenerateInfoRepository = taxGenerateInfoRepository;
            _financialYearRepository = financialYearRepository;
        }
        public bool Add(TaxInstallment model)
        {
            _taxInstallmentRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            TaxInstallment model = _taxInstallmentRepository.GetById(id);
            model.IsDeleted = true;

            _taxInstallmentRepository.Update(model);
            return Save();
        }
        public bool Update(TaxInstallment model)
        {
            _taxInstallmentRepository.Update(model);
            return Save();
        }
        public IEnumerable<TaxInstallment> GetAll()
        {
            return _taxInstallmentRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public TaxInstallment GetDetails(int id)
        {
            return _taxInstallmentRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public bool IsExistingItem(string keyName, int finYearId, int? id)
        {
            var count = 0;
            if (id == null)
                count = _taxInstallmentRepository.GetCount(a => a.IsDeleted == false && a.HoldingNo == keyName.Trim() && a.FinancialYearId == finYearId && a.IsPaid == true);
            else
                count = _taxInstallmentRepository.GetCount(a => a.IsDeleted == false && a.HoldingNo == keyName.Trim() && a.FinancialYearId == finYearId && a.IsPaid == true && a.Id != id);
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

        public VMTaxInstallment GenerateSingleTaxInstallment(string holdingNo, int finYearId)
        {
            VMTaxInstallment result = new VMTaxInstallment();
            VMTaxInstallmentDetails resultDetails = new VMTaxInstallmentDetails();
            HouseOwner houseOwner = _houseOwnerRepository.Get(h => h.HoldingNo.Equals(holdingNo) && h.IsDeleted == false);
            TaxGenerateInfo taxInfo = _taxGenerateInfoRepository.Get(h => h.HoldingNo.Equals(holdingNo) && h.IsDeleted == false && h.FinancialYearId == finYearId);
            TaxInstallment taxInstallment = _taxInstallmentRepository.Get(t => t.HoldingNo.Equals(holdingNo) && t.IsPaid == false && t.FinancialYearId == finYearId && t.IsDeleted == false);
            decimal OutstandingTaxAmount = taxInstallment != null ? taxInstallment.TaxAmount : 0;
            decimal PenaltyAmount = taxInstallment != null ? taxInstallment.PenaltyAmount : 0;

            if (taxInstallment != null)
            {
                resultDetails = new VMTaxInstallmentDetails
                {
                    HouseOwnerId = houseOwner.Id,
                    HouseOwnerName = houseOwner.OwnerNameInBangla,
                    InstallmentAmount = taxInstallment.TaxAmount,
                    DueAmount = taxInstallment.TaxAmount - OutstandingTaxAmount,
                    InstallmentDate = taxInstallment.TaxPaymentDate,
                    PenaltyAmount = taxInstallment.PenaltyAmount
                };
                result = new VMTaxInstallment
                {
                    Id = taxInstallment != null ? taxInstallment.Id : 0,
                    HoldingNo = houseOwner.HoldingNo,
                    FinancialYearId = finYearId,
                    vMTaxInstallmentDetails = resultDetails
                };
                return result;
            }
            else
            {
                if (taxInfo != null)
                {
                    resultDetails = new VMTaxInstallmentDetails
                    {
                        HouseOwnerId = houseOwner.Id,
                        HouseOwnerName = houseOwner.OwnerNameInBangla,
                        InstallmentAmount = (decimal)taxInfo.TotalTax,
                        DueAmount = 0,
                        InstallmentDate = DateTime.Now,
                        PenaltyAmount = PenaltyAmount
                    };
                    result = new VMTaxInstallment
                    {
                        Id = taxInstallment != null ? taxInstallment.Id : 0,
                        HoldingNo = houseOwner.HoldingNo,
                        FinancialYearId = finYearId,
                        vMTaxInstallmentDetails = resultDetails
                    };
                    return result;
                }
            }

            
            return result;
        }

        public VMTaxInstallment GeteSingleTaxInstallment(string holdingNo, int finYearId)
        {
            VMTaxInstallment result = new VMTaxInstallment();
            VMTaxInstallmentDetails resultDetails = new VMTaxInstallmentDetails();
            HouseOwner houseOwner = _houseOwnerRepository.Get(h => h.HoldingNo.Equals(holdingNo) && h.IsDeleted == false);
            TaxGenerateInfo taxInfo = _taxGenerateInfoRepository.Get(h => h.HoldingNo.Equals(holdingNo) && h.IsDeleted == false && h.FinancialYearId == finYearId);
            TaxInstallment taxInstallment = _taxInstallmentRepository.Get(t => t.HoldingNo.Equals(holdingNo) && t.FinancialYearId == finYearId && t.IsDeleted == false);
            decimal OutstandingTaxAmount = taxInstallment != null ? taxInstallment.OutstandingAmount : 0;
            decimal PenaltyAmount = taxInstallment != null ? taxInstallment.PenaltyAmount : 0;

            if (taxInstallment != null)
            {
                resultDetails = new VMTaxInstallmentDetails
                {
                    HouseOwnerId = houseOwner.Id,
                    HouseOwnerName = houseOwner.OwnerNameInBangla,
                    InstallmentAmount = taxInstallment.TaxAmount,
                    DueAmount = (taxInstallment.TaxAmount - OutstandingTaxAmount),
                    InstallmentDate = taxInstallment.TaxPaymentDate,
                    PenaltyAmount = taxInstallment.PenaltyAmount
                };
                result = new VMTaxInstallment
                {
                    Id = taxInstallment.Id,
                    HoldingNo = taxInstallment.HoldingNo,
                    FinancialYearId = finYearId,
                    vMTaxInstallmentDetails = resultDetails
                };
                return result;
            }
            //new busienss
            else
            {
                if (taxInfo != null)
                {
                    resultDetails = new VMTaxInstallmentDetails
                    {
                        HouseOwnerId = houseOwner.Id,
                        HouseOwnerName = houseOwner.OwnerNameInBangla,
                        InstallmentAmount = (decimal)taxInfo.TotalTax,
                        DueAmount = 0,
                        InstallmentDate = DateTime.Now,
                        PenaltyAmount = 0
                    };
                    result = new VMTaxInstallment
                    {
                        TaxGenerateId = taxInfo.Id,
                        HoldingNo = houseOwner.HoldingNo,
                        FinancialYearId = finYearId,
                        vMTaxInstallmentDetails = resultDetails
                    };
                    return result;
                }
            }
            return result;
        }

        public VMRPTTaxCollectionSingle GetMRPTTaxCollectionSingle(string holdingNo, int finYearId, int unionId)
        {
            var result = _taxInstallmentRepository.ExecStoreProcedure<VMRPTTaxCollectionSingle>("GetSingleTaxDetails @holdingNo,@finYearId, @unionId",

                new SqlParameter("holdingNo", SqlDbType.NVarChar) { Value = holdingNo },
                new SqlParameter("finYearId", SqlDbType.Int) { Value = finYearId },
                new SqlParameter("unionId", SqlDbType.Int) { Value = unionId }
                ).FirstOrDefault();

            return result;
        }

        public VMRPTTaxCollectionSingle GetRPTTaxDueNotice(string holdingNo, int finYearId, int unionId)
        {
            var result = _taxInstallmentRepository.ExecStoreProcedure<VMRPTTaxCollectionSingle>("GetSingleTaxNotice @holdingNo,@finYearId, @unionId",

                new SqlParameter("holdingNo", SqlDbType.NVarChar) { Value = holdingNo },
                new SqlParameter("finYearId", SqlDbType.Int) { Value = finYearId },
                new SqlParameter("unionId", SqlDbType.Int) { Value = unionId }
                ).FirstOrDefault();

            return result;
        }

        public List<VMRPTTaxCollectionSingle> GetMRPTTaxCollectionAll(int wardId, int villageId, int finYearId, int unionId)
        {
            var result = _taxInstallmentRepository.ExecStoreProcedure<VMRPTTaxCollectionSingle>("GetAllTaxDetails @finYearId, @unionId, @wardId, @villageId",

                new SqlParameter("finYearId", SqlDbType.Int) { Value = finYearId },
                new SqlParameter("unionId", SqlDbType.Int) { Value = unionId },
                new SqlParameter("wardId", SqlDbType.Int) { Value = wardId },
                new SqlParameter("villageId", SqlDbType.Int) { Value = villageId }
                ).ToList();

            return result;
        }

        public List<VMRPTTaxReceipt> GetRPTTaxReceipt(int villageId, int wardId, int finYearId, int unionId)
        {
            var result = _taxInstallmentRepository.ExecStoreProcedure<VMRPTTaxReceipt>("GetAllTaxReceipt @villageInfoId, @wardInfoId, @finYearId, @unionId",

                new SqlParameter("villageInfoId", SqlDbType.Int) { Value = villageId },
                new SqlParameter("wardInfoId", SqlDbType.Int) { Value = wardId },
                new SqlParameter("finYearId", SqlDbType.Int) { Value = finYearId },
                new SqlParameter("unionId", SqlDbType.Int) { Value = unionId }
                ).ToList();

            return result;
        }

        public IEnumerable<SPTopSheetReport> GetTopSheetReport(string financialYear)
        {
            var query = $"EXEC TopSheetReport {financialYear}";
            var data = _taxInstallmentRepository.SQLQueryList<SPTopSheetReport>(query).ToList();
            return data;
        }

        public List<VMWordOrVillWiseTaxReport> GetRPTTaxInfoByWord(int villageId, int wardId, int finYearId, int unionId)
        {
            var result = _taxInstallmentRepository.ExecStoreProcedure<VMWordOrVillWiseTaxReport>("GetTaxDetailsByWord @wardId, @villageId, @finYearId, @unionId",

                new SqlParameter("wardId", SqlDbType.Int) { Value = wardId },
                new SqlParameter("villageId", SqlDbType.Int) { Value = villageId },
                new SqlParameter("finYearId", SqlDbType.Int) { Value = finYearId },
                new SqlParameter("unionId", SqlDbType.Int) { Value = unionId }
                ).ToList();

            return result;
        }

        public List<VMWordOrVillWiseTaxReport> GetRPTTaxInfoByVillage(int villageId, int finYearId, int unionId)
        {
            var result = _taxInstallmentRepository.ExecStoreProcedure<VMWordOrVillWiseTaxReport>("GetTaxDetailsByVillage @villageId, @finYearId, @unionId",

                new SqlParameter("villageId", SqlDbType.Int) { Value = villageId },
                new SqlParameter("finYearId", SqlDbType.Int) { Value = finYearId },
                new SqlParameter("unionId", SqlDbType.Int) { Value = unionId }
                ).ToList();

            return result;
        }

        public VMPersonalShortReport GetPersonalShortReport(int unionId, string holdingNo)
        {
            DateTime date = DateTime.Now;
            var currentFinYear = _financialYearRepository.Get(u => u.StartDate.CompareTo(date) < 0 && u.EndDate.CompareTo(date) > 0);
            int finYearId = currentFinYear != null ? currentFinYear.Id : 0;
            VMPersonalShortReport report = new VMPersonalShortReport();
            TaxGenerateInfo currentYearTax = _taxGenerateInfoRepository.Get(t => t.IsDeleted == false && t.HoldingNo.Equals(holdingNo) && t.UnionId == unionId);
            TaxInstallment installment = _taxInstallmentRepository.Get(t => t.IsDeleted == false && t.HoldingNo.Equals(holdingNo) && t.UnionId == unionId);
            if (currentYearTax != null)
            {
                report.HoldingNo = currentYearTax.HoldingNo;
                report.TotalYearlyTax = installment != null ? installment.TaxAmount : (decimal)currentYearTax.TotalTax;
                report.TotalYearlyPaidTax = installment != null ? installment.OutstandingAmount : 0;
            }
            return report;
        }

        public VMPersonalStatement GetPersonalStatementReport(int unionId, string holdingNo)
        {
            VMPersonalStatement statement = new VMPersonalStatement();
            HouseOwner houseOwnerInfo = _houseOwnerRepository.Get(h => h.IsDeleted == false && h.HoldingNo.Equals(holdingNo) && h.WardInfo.UnionId == unionId);
            if (houseOwnerInfo != null)
            {
                statement.HoldingNo = houseOwnerInfo.HoldingNo;
                statement.FullName = houseOwnerInfo.OwnerNameInBangla;
                statement.ParentName = houseOwnerInfo.FatherHusbandName;
                statement.WordNo = houseOwnerInfo.WardInfo.WardNo;
                statement.Village = houseOwnerInfo.VillageInfo.VillageName;
                statement.MobileNo = houseOwnerInfo.MobileNo;
            }

            DateTime date = DateTime.Now;
            var currentFinYear = _financialYearRepository.Get(u => u.StartDate.CompareTo(date) < 0 && u.EndDate.CompareTo(date) > 0);
            int finYearId = currentFinYear != null ? currentFinYear.Id : 0;

            TaxGenerateInfo taxInfo = _taxGenerateInfoRepository.Get(t => t.IsDeleted == false && t.FinancialYearId == finYearId && t.UnionId == unionId && t.HoldingNo.Equals(holdingNo));
            if (taxInfo != null && houseOwnerInfo != null)
            {
                VMPersonalTaxDetails taxDetails = new VMPersonalTaxDetails
                {
                    FinYearName = taxInfo.FinancialYear.YearName,
                    TotalBuildingHouse = houseOwnerInfo.TotalBuildingHouse,
                    TotalRawHouse = houseOwnerInfo.TotalRawHouse,
                    TotalSemiBuildingHouse = houseOwnerInfo.TotalSemiBuildingHouse,
                    DateOfTaxCreation = houseOwnerInfo.CreatedDate,
                    TotalTax = (decimal)taxInfo.TotalTax
                };
                statement.PersonalTaxDetails = taxDetails;
            }

            TaxInstallment installment = _taxInstallmentRepository.Get(t => t.IsDeleted == false && t.UnionId == unionId && t.HoldingNo.Equals(holdingNo));

            if (installment != null)
            {
                VMPersonalTaxCollectionInfo collectionInfo = new VMPersonalTaxCollectionInfo
                {
                    CollectionDate = installment.TaxPaymentDate,
                    CollectedTaxAmount = installment.OutstandingAmount,
                    CollectedDueAmount = 0
                };
                statement.PersonalTaxCollectionInfo = collectionInfo;
            }
            return statement;

        }
    }
}
