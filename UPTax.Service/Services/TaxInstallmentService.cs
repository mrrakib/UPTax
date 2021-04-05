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
    public interface ITaxInstallmentService
    {
        bool Add(TaxInstallment model);
        bool Update(TaxInstallment model);
        bool Delete(int id);
        IEnumerable<TaxInstallment> GetAll();
        TaxInstallment GetDetails(int id);
        bool IsExistingItem(string keyName, int? id);
        bool Save();
    }
    public class TaxInstallmentService : ITaxInstallmentService
    {
        private readonly ITaxInstallmentRepository _taxInstallmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TaxInstallmentService(ITaxInstallmentRepository taxInstallmentRepository, IUnitOfWork unitOfWork)
        {
            _taxInstallmentRepository = taxInstallmentRepository;
            _unitOfWork = unitOfWork;
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

        public bool IsExistingItem(string keyName, int? id)
        {
            var count = 0;
            if (id == null)
                count = _taxInstallmentRepository.GetCount(a => a.IsDeleted == false && a.HoldingNo == keyName.Trim());
            else
                count = _taxInstallmentRepository.GetCount(a => a.IsDeleted == false && a.HoldingNo == keyName.Trim() && a.Id != id);
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
