using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository;
using UPTax.Model.Models;

namespace UPTax.Service.Services
{
    public interface IEducationInfoService
    {
        bool Add(EducationInfo model);
        bool Delete(int id);
        bool Update(EducationInfo model);
        IPagedList GetPagedList(int pageNo, int pageSize);
        IEnumerable<EducationInfo> GetAll();
        EducationInfo GetDetails(int id);
        bool IsExistingItem(string degree);
        bool Save();
    }
    public class EducationInfoService : IEducationInfoService
    {
        private readonly IEducationInfoRepository _educationInfoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EducationInfoService(IEducationInfoRepository educationInfoRepository, IUnitOfWork unitOfWork)
        {
            _educationInfoRepository = educationInfoRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(EducationInfo model)
        {
            _educationInfoRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            EducationInfo wardInfo = _educationInfoRepository.GetById(id);
            wardInfo.IsDeleted = true;

            _educationInfoRepository.Update(wardInfo);
            return Save();
        }
        public bool Update(EducationInfo model)
        {
            _educationInfoRepository.Update(model);
            return Save();
        }
        public IEnumerable<EducationInfo> GetAll()
        {
            return _educationInfoRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public EducationInfo GetDetails(int id)
        {
            return _educationInfoRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public IPagedList GetPagedList(int pageNo, int pageSize)
        {
            throw new NotImplementedException();
        }

        public bool IsExistingItem(string degree)
        {
            return _educationInfoRepository.GetCount(a => a.IsDeleted == false && a.Degree == degree.Trim()) > 0 ? true : false;
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
