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
    public interface IAdminNoticeService
    {
        bool Add(AdminNotice model);
        bool Update(AdminNotice model);
        bool Delete(int id);
        IPagedList GetPagedList(string searchItem, int pageNo, int pageSize);
        IEnumerable<AdminNotice> GetAll();
        AdminNotice GetDetails(int id);
        bool IsExistingItem(int? id);
        bool Save();
    }

    public class AdminNoticeService : IAdminNoticeService
    {
        private readonly IAdminNoticeRepository _adminNoticeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminNoticeService(IAdminNoticeRepository adminNoticeRepository, IUnitOfWork unitOfWork)
        {
            _adminNoticeRepository = adminNoticeRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(AdminNotice model)
        {
            _adminNoticeRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            AdminNotice member = _adminNoticeRepository.GetById(id);
            member.IsDeleted = true;

            _adminNoticeRepository.Update(member);
            return Save();
        }
        public bool Update(AdminNotice model)
        {
            _adminNoticeRepository.Update(model);
            return Save();
        }
        public IEnumerable<AdminNotice> GetAll()
        {
            return _adminNoticeRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public AdminNotice GetDetails(int id)
        {
            return _adminNoticeRepository.Get(u => u.Id == id && u.IsDeleted == false);
        }

        public IPagedList GetPagedList(string searchItem, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(searchItem))
                {
                    searchPrm += string.Format(@" WHERE m.IsDeleted=0 AND au.FullName LIKE N'%{0}%'", searchItem?.Trim());
                }
                else
                {
                    searchPrm += string.Format(@" WHERE m.IsDeleted=0");
                }
                string query = string.Format(@"SELECT m.Id, m.[Message], ISNULL(au.FullName,'All Admin') ToAdminUserName,sau.FullName ToSupperAdminUserName, m.CreatedDate  FROM MessageInfo m
                                               JOIN Users sau ON m.ToSupperAdminUserId=sau.Id
                                               LEFT JOIN Users au ON m.ToAdminUserId=au.Id
                                               {0} ORDER BY m.Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(m.Id)  FROM MessageInfo m
                                                    JOIN Users sau ON m.ToSupperAdminUserId=sau.Id 
                                                    LEFT JOIN Users au ON m.ToAdminUserId=au.Id
                                                    {0}", searchPrm);

                int rowCount = _adminNoticeRepository.SQLQuery<int>(countQuery);
                var data = _adminNoticeRepository.SQLQueryList<VMMessageInfo>(query).OrderByDescending(a => a.CreatedDate).ToList();
                return new StaticPagedList<VMMessageInfo>(data, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<VMMessageInfo>(new List<VMMessageInfo> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(int? id)
        {
            var count = 0;
            if (id == null)
                count = _adminNoticeRepository.GetCount(a => a.IsDeleted == false);
            else
                count = _adminNoticeRepository.GetCount(a => a.IsDeleted == false && a.Id != id);
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
