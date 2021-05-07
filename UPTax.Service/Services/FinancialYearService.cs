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
    public interface IFinancialYearService
    {
        bool Add(FinancialYear model);
        bool Update(FinancialYear model);
        bool Delete(int id);
        IPagedList GetPagedList(string keyName, int pageNo, int pageSize);
        IEnumerable<FinancialYear> GetAll();
        FinancialYear GetDetails(int id);
        bool IsExistingItem(string keyName, int? id);
        bool Save();
        int? GetFinancialYearIdByDate(DateTime date);
        List<IdNameDropdown> GetAllForDropdown();
        string GetNameById(int id);
    }
    public class FinancialYearService : IFinancialYearService
    {
        private readonly IFinancialYearRepository _financialYearRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FinancialYearService(IFinancialYearRepository financialYearRepository, IUnitOfWork unitOfWork)
        {
            _financialYearRepository = financialYearRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(FinancialYear model)
        {
            _financialYearRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            FinancialYear model = _financialYearRepository.GetById(id);
            model.IsDeleted = true;

            _financialYearRepository.Update(model);
            return Save();
        }
        public bool Update(FinancialYear model)
        {
            _financialYearRepository.Update(model);
            return Save();
        }
        public IEnumerable<FinancialYear> GetAll()
        {
            return _financialYearRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public FinancialYear GetDetails(int id)
        {
            return _financialYearRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public IPagedList GetPagedList(string keyName, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(keyName))
                {
                    searchPrm += string.Format(@" WHERE TypeName LIKE N'%{0}%' AND IsDeleted = 0", keyName.Trim());
                }
                else
                {
                    searchPrm += string.Format(@" WHERE IsDeleted = 0");
                }
                string query = string.Format(@"SELECT * FROM InfraStructuralType {0} ORDER BY Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(Id) FROM InfraStructuralType {0}", searchPrm);

                int rowCount = _financialYearRepository.SQLQuery<int>(countQuery);
                List<FinancialYear> list = _financialYearRepository.SQLQueryList<FinancialYear>(query).ToList();
                return new StaticPagedList<FinancialYear>(list, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<FinancialYear>(new List<FinancialYear> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(string keyName, int? id)
        {
            var count = 0;
            if (id == null)
                count = _financialYearRepository.GetCount(a => a.IsDeleted == false && a.YearName == keyName.Trim());
            else
                count = _financialYearRepository.GetCount(a => a.IsDeleted == false && a.YearName == keyName.Trim() && a.Id != id);
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


        public int? GetFinancialYearIdByDate(DateTime date)
        {
            int? result = null;
            try
            {
                result = _financialYearRepository.Get(u => u.StartDate.CompareTo(date) < 0 && u.EndDate.CompareTo(date) > 0).Id;
                return result;
            }
            catch
            {
                return result;
            }

        }

        public List<IdNameDropdown> GetAllForDropdown()
        {
            return _financialYearRepository.GetMany(f => !f.IsDeleted).Select(u => new IdNameDropdown
            {
                Id = u.Id,
                Name = u.YearName
            }).OrderByDescending(a => a.Id).ToList();
        }

        public string GetNameById(int id)
        {
            FinancialYear year = _financialYearRepository.Get(f => f.Id == id);
            return year != null ? year.YearName : "";
        }
    }
}
