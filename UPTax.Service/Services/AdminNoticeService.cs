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
        string GetAllNoticeByToday(int unionId);
        VMDashboard GetDashboardInfo();
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
        public string GetAllNoticeByToday(int unionId)
        {
            DateTime today = DateTime.Now;
            var date = today.Date;
            string query = string.Format(@"SELECT an.Message AdminMessage FROM AdminNotice an WHERE an.UnionId = {0} AND an.IsDeleted = 0
AND '{1}' BETWEEN an.FromDate AND an.ToDate", unionId, date);
            List<VMNotice> notices = new List<VMNotice>();
            notices = _adminNoticeRepository.SQLQueryList<VMNotice>(query).ToList();
            string noticeMessage = string.Empty;
            if (notices.Count > 0)
            {
                foreach (var item in notices)
                {
                    noticeMessage += "   *** " + item.AdminMessage + " ***   ";
                }
            }
            return noticeMessage;
        }


        public IPagedList GetPagedList(string searchItem, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(searchItem))
                {
                    searchPrm += string.Format(@" WHERE an.IsDeleted=0 AND up.Name LIKE N'%{0}%'", searchItem?.Trim());
                }
                else
                {
                    searchPrm += string.Format(@" WHERE an.IsDeleted=0");
                }
                string query = string.Format(@"SELECT an.Id, up.Name, an.FromDate, an.ToDate FROM AdminNotice an
LEFT JOIN UnionParishad up ON an.UnionId = up.Id
                                               {0} ORDER BY an.Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(an.Id)  FROM AdminNotice an
                                                    LEFT JOIN UnionParishad up ON an.UnionId = up.Id
                                                    {0}", searchPrm);

                int rowCount = _adminNoticeRepository.SQLQuery<int>(countQuery);
                var data = _adminNoticeRepository.SQLQueryList<VMAdminNotice>(query).ToList();
                return new StaticPagedList<VMAdminNotice>(data, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<VMAdminNotice>(new List<VMAdminNotice> { }, pageNo, pageSize, 0);
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
        public VMDashboard GetDashboardInfo()
        {
            var query = @"EXEC GetDashboardInfo";
            var data = _adminNoticeRepository.SQLQuery<VMDashboard>(query);
            return data;
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
