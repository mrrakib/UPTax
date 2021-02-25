using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository.UPDetails;
using UPTax.Model.Models.UnionDetails;

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
        IPagedList GetPagedList(int pageNo, int pageSize);
    }
    public class WardInfoService : IWardInfoService
    {
        private readonly IWardInfoRepository _wardInfoRepository;
        private readonly IUnitOfWork _unitOfWork;
        public WardInfoService(IWardInfoRepository wardInfoRepository, IUnitOfWork unitOfWork)
        {
            _wardInfoRepository = wardInfoRepository;
            _unitOfWork = unitOfWork;
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
            return _wardInfoRepository.GetAll();
        }


        public WardInfo GetDetails(int id)
        {
            return _wardInfoRepository.Get(u => u.Id == id);
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
                return false;
            }
        }

        public bool Update(WardInfo model)
        {
            _wardInfoRepository.Update(model);
            return Save();
        }

        public IPagedList GetPagedList(int pageNo, int pageSize)
        {
            try
            {
                string query = string.Format(@"SELECT * FROM UnionParishad {0}
                ORDER BY Id
                OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(*) FROM UnionParishad WHERE Name LIKE N'%{0}%'");

                int rowCount = _wardInfoRepository.SQLQuery<int>(countQuery);
                List<WardInfo> unionParishads = _wardInfoRepository.SQLQueryList<WardInfo>(query).ToList();
                return new StaticPagedList<WardInfo>(unionParishads, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                return new StaticPagedList<WardInfo>(new List<WardInfo> { }, pageNo, pageSize, 0);
            }
        }
    }
}
