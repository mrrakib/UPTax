using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository;
using UPTax.Model.Models;

namespace UPTax.Service.Services
{
    public interface IInfrastructureInfoService
    {
        bool Add(InfrastructureInfo model);
        bool Update(InfrastructureInfo model);
        bool Delete(int id);
        IPagedList GetPagedList(string degree, int pageNo, int pageSize);
        IEnumerable<InfrastructureInfo> GetAll();
        InfrastructureInfo GetDetails(int id);
        bool IsExistingItem(string keyName);
        bool Save();
    }
    public class InfrastructureInfoService : IInfrastructureInfoService
    {
        private readonly IInfrastructureInfoRepository _InfrastructureInfoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InfrastructureInfoService(IInfrastructureInfoRepository InfrastructureInfoRepository, IUnitOfWork unitOfWork)
        {
            _InfrastructureInfoRepository = InfrastructureInfoRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(InfrastructureInfo model)
        {
            _InfrastructureInfoRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            InfrastructureInfo wardInfo = _InfrastructureInfoRepository.GetById(id);
            wardInfo.IsDeleted = true;

            _InfrastructureInfoRepository.Update(wardInfo);
            return Save();
        }
        public bool Update(InfrastructureInfo model)
        {
            _InfrastructureInfoRepository.Update(model);
            return Save();
        }
        public IEnumerable<InfrastructureInfo> GetAll()
        {
            return _InfrastructureInfoRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public InfrastructureInfo GetDetails(int id)
        {
            return _InfrastructureInfoRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public IPagedList GetPagedList(string keyName, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(keyName))
                {
                    searchPrm += string.Format(@" WHERE HoldingNo LIKE N'%{0}%'", keyName.Trim());
                }
                string query = string.Format(@"SELECT * FROM InfrastructureInfo {0} ORDER BY Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(*) FROM InfrastructureInfo WHERE HoldingNo LIKE N'%{0}%'", keyName?.Trim());

                int rowCount = _InfrastructureInfoRepository.SQLQuery<int>(countQuery);
                List<InfrastructureInfo> educations = _InfrastructureInfoRepository.SQLQueryList<InfrastructureInfo>(query).Where(a => a.IsDeleted == false).ToList();
                return new StaticPagedList<InfrastructureInfo>(educations, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<InfrastructureInfo>(new List<InfrastructureInfo> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(string keyName)
        {
            return _InfrastructureInfoRepository.GetCount(a => a.IsDeleted == false && a.HoldingNo == keyName.Trim()) > 0 ? true : false;
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
