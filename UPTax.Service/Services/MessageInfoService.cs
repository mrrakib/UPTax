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
    public interface IMessageInfoService
    {
        bool Add(MessageInfo model);
        bool Update(MessageInfo model);
        bool Delete(int id);
        IPagedList GetPagedList(string searchItem, int pageNo, int pageSize);
        IPagedList GetPagedList(string adminUserId, string searchItem, int pageNo, int pageSize);
        IEnumerable<MessageInfo> GetAll();
        MessageInfo GetDetails(int id);
        bool IsExistingItem(string searchItem, int? id);
        bool Save();
    }

    public class MessageInfoService : IMessageInfoService
    {
        private readonly IMessageInfoRepository _messageInfoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MessageInfoService(IMessageInfoRepository messageInfoRepository, IUnitOfWork unitOfWork)
        {
            _messageInfoRepository = messageInfoRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(MessageInfo model)
        {
            _messageInfoRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            MessageInfo member = _messageInfoRepository.GetById(id);
            member.IsDeleted = true;

            _messageInfoRepository.Update(member);
            return Save();
        }
        public bool Update(MessageInfo model)
        {
            _messageInfoRepository.Update(model);
            return Save();
        }
        public IEnumerable<MessageInfo> GetAll()
        {
            return _messageInfoRepository.GetMany(a => a.IsDeleted == false).ToList();
        }

        public MessageInfo GetDetails(int id)
        {
            return _messageInfoRepository.Get(u => u.Id == id && u.IsDeleted == false);
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

                int rowCount = _messageInfoRepository.SQLQuery<int>(countQuery);
                var data = _messageInfoRepository.SQLQueryList<VMMessageInfo>(query).OrderByDescending(a => a.CreatedDate).ToList();
                return new StaticPagedList<VMMessageInfo>(data, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<VMMessageInfo>(new List<VMMessageInfo> { }, pageNo, pageSize, 0);
            }
        }
        public IPagedList GetPagedList(string adminUserId, string searchItem, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(searchItem))
                {
                    searchPrm += string.Format(@" WHERE m.IsDeleted=0 AND au.FullName LIKE N'%{0}%' AND (au.Id='{1}' OR au.Id IS NULL)", searchItem?.Trim(), adminUserId);
                }
                else
                {
                    searchPrm += string.Format(@" WHERE m.IsDeleted=0 AND (au.Id='{0}' OR au.Id IS NULL)", adminUserId);
                }
                string query = string.Format(@"SELECT m.Id, m.[Message], ISNULL(au.FullName,'All Admin') ToAdminUserName,sau.FullName ToSupperAdminUserName, m.CreatedDate  FROM MessageInfo m
                                               JOIN Users sau ON m.ToSupperAdminUserId=sau.Id
                                               LEFT JOIN Users au ON m.ToAdminUserId=au.Id
                                               {0} ORDER BY m.Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(m.Id)  FROM MessageInfo m
                                                    JOIN Users sau ON m.ToSupperAdminUserId=sau.Id 
                                                    LEFT JOIN Users au ON m.ToAdminUserId=au.Id
                                                    {0}", searchPrm);

                int rowCount = _messageInfoRepository.SQLQuery<int>(countQuery);
                var data = _messageInfoRepository.SQLQueryList<VMMessageInfo>(query).OrderByDescending(a => a.CreatedDate).ToList();
                return new StaticPagedList<VMMessageInfo>(data, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<VMMessageInfo>(new List<VMMessageInfo> { }, pageNo, pageSize, 0);
            }
        }

        public bool IsExistingItem(string holdingNo, int? id)
        {
            var count = 0;
            if (id == null)
                count = _messageInfoRepository.GetCount(a => a.IsDeleted == false);
            else
                count = _messageInfoRepository.GetCount(a => a.IsDeleted == false && a.Id != id);
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
