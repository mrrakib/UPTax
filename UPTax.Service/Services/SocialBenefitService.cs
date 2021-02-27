using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository;
using UPTax.Model.Models;

namespace UPTax.Service.Services
{
    public interface ISocialBenefitService
    {
        bool Add(SocialBenefit model);
        bool Update(SocialBenefit model);
        bool Delete(int id);
        IPagedList GetPagedList(string keyName, int pageNo, int pageSize);
        IEnumerable<SocialBenefit> GetAll();
        SocialBenefit GetDetails(int id);
        bool IsExistingItem(string keyName);
        bool Save();
    }
    public class SocialBenefitService : ISocialBenefitService
    {
        private readonly ISocialBenefitRepository _SocialBenefitRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SocialBenefitService(ISocialBenefitRepository SocialBenefitRepository, IUnitOfWork unitOfWork)
        {
            _SocialBenefitRepository = SocialBenefitRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(SocialBenefit model)
        {
            _SocialBenefitRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            SocialBenefit wardInfo = _SocialBenefitRepository.GetById(id);
            wardInfo.IsDeleted = true;

            _SocialBenefitRepository.Update(wardInfo);
            return Save();
        }
        public bool Update(SocialBenefit model)
        {
            _SocialBenefitRepository.Update(model);
            return Save();
        }
        public IEnumerable<SocialBenefit> GetAll()
        {
            return _SocialBenefitRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public SocialBenefit GetDetails(int id)
        {
            return _SocialBenefitRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public IPagedList GetPagedList(string keyName, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(keyName))
                {
                    searchPrm += string.Format(@" WHERE Title LIKE N'%{0}%'", keyName.Trim());
                }
                string query = string.Format(@"SELECT * FROM SocialBenefits {0} ORDER BY Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(*) FROM SocialBenefits WHERE Title LIKE N'%{0}%'", keyName?.Trim());

                int rowCount = _SocialBenefitRepository.SQLQuery<int>(countQuery);
                List<SocialBenefit> educations = _SocialBenefitRepository.SQLQueryList<SocialBenefit>(query).Where(a => a.IsDeleted == false).ToList();
                return new StaticPagedList<SocialBenefit>(educations, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<SocialBenefit>(new List<SocialBenefit> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(string keyName)
        {
            return _SocialBenefitRepository.GetCount(a => a.IsDeleted == false && a.Title == keyName.Trim()) > 0 ? true : false;
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
