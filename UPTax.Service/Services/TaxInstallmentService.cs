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
        IEnumerable<TopSheetReportSP> GetTopSheetReport(string financialYear);
    }
    public class TaxInstallmentService : ITaxInstallmentService
    {
        private readonly ITaxInstallmentRepository _taxInstallmentRepository;
        private readonly IHouseOwnerRepository _houseOwnerRepository;
        private readonly ITaxGenerateInfoRepository _taxGenerateInfoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TaxInstallmentService(ITaxInstallmentRepository taxInstallmentRepository, IHouseOwnerRepository houseOwnerRepository, IUnitOfWork unitOfWork, ITaxGenerateInfoRepository taxGenerateInfoRepository)
        {
            _taxInstallmentRepository = taxInstallmentRepository;
            _houseOwnerRepository = houseOwnerRepository;
            _unitOfWork = unitOfWork;
            _taxGenerateInfoRepository = taxGenerateInfoRepository;
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

            if (taxInfo != null)
            {
                resultDetails = new VMTaxInstallmentDetails
                {
                    HouseOwnerId = houseOwner.Id,
                    HouseOwnerName = houseOwner.OwnerNameInBangla,
                    InstallmentAmount = (decimal)taxInfo.TotalTax,
                    DueAmount = ((decimal)taxInfo.TotalTax - OutstandingTaxAmount),
                    InstallmentDate = DateTime.Now,
                    PenaltyAmount = PenaltyAmount
                };
                result = new VMTaxInstallment
                {
                    HoldingNo = houseOwner.HoldingNo,
                    FinancialYearId = finYearId,
                    vMTaxInstallmentDetails = resultDetails
                };
                return result;
            }
            return result;
        }

        public VMTaxInstallment GeteSingleTaxInstallment(string holdingNo, int finYearId)
        {
            VMTaxInstallment result = new VMTaxInstallment();
            VMTaxInstallmentDetails resultDetails = new VMTaxInstallmentDetails();
            HouseOwner houseOwner = _houseOwnerRepository.Get(h => h.HoldingNo.Equals(holdingNo) && h.IsDeleted == false);
            //TaxGenerateInfo taxInfo = _taxGenerateInfoRepository.Get(h => h.HoldingNo.Equals(holdingNo) && h.IsDeleted == false && h.FinancialYearId == finYearId);
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

        public IEnumerable<TopSheetReportSP> GetTopSheetReport(string financialYear)
        {
            var query = $"EXEC TopSheetReport {financialYear}";
            var data = _taxInstallmentRepository.SQLQueryList<TopSheetReportSP>(query).ToList();
            return data;
        }
    }
}
