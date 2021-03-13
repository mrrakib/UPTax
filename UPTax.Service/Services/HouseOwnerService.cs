using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository;
using UPTax.Model.Models;

namespace UPTax.Service.Services
{
    public interface IHouseOwnerService
    {
        bool Add(HouseOwner model);
        bool Update(HouseOwner model);
        bool Delete(int id);
        IPagedList GetPagedList(string degree, int pageNo, int pageSize);
        IEnumerable<HouseOwner> GetAll();
        HouseOwner GetDetails(int id);
        bool IsExistingItem(string holdingNo, int? id);
        bool Save();
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

        public IPagedList GetPagedList(string keyName, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(keyName))
                {
                    searchPrm += string.Format(@" WHERE NameOfInstitute LIKE N'%{0}%'", keyName.Trim());
                }
                string query = string.Format(@"SELECT * FROM HouseOwner {0} ORDER BY Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(*) FROM HouseOwner WHERE NameOfInstitute LIKE N'%{0}%'", keyName?.Trim());

                int rowCount = _HouseOwnerRepository.SQLQuery<int>(countQuery);
                List<HouseOwner> educations = _HouseOwnerRepository.SQLQueryList<HouseOwner>(query).Where(a => a.IsDeleted == false).ToList();
                return new StaticPagedList<HouseOwner>(educations, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<HouseOwner>(new List<HouseOwner> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(string holdingNo, int? id)
        {
            var count = 0;
            if (id == null)
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



    }
}
