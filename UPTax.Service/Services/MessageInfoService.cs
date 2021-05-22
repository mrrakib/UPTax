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
        IPagedList GetPagedList(string holdingNo, int pageNo, int pageSize);
        IEnumerable<MessageInfo> GetAll();
        MessageInfo GetDetails(int id);
        bool IsExistingItem(string holdingNo, int? id);
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

        public IPagedList GetPagedList(string holdingNo, int pageNo, int pageSize)
        {
            try
            {
                string searchPrm = string.Empty;
                if (!string.IsNullOrWhiteSpace(holdingNo))
                {
                    searchPrm += string.Format(@" WHERE h.IsDeleted=0 AND h.HoldingNo LIKE N'%{0}%'", holdingNo.Trim());
                }
                else
                {
                    searchPrm += string.Format(@" WHERE h.IsDeleted=0");
                }
                string query = string.Format(@"SELECT h.Id,
                                                h.HoldingNo,
                                                h.MemberNameInBangla,
                                                p.ProfessionTitle Profession,
                                                h.DateOfBirth,
                                                h.BirthRegistrationNumber,
                                                h.NIDNumber,
                                                g.[Name] GenderName, 
                                                r.[Name] Relationship,
                                                e.Degree EducationName, 
                                                sbr.Title SocialBenefitRunningName,
                                                sbe.Title SocialBenefitEligibleName, 
                                                sbb.Title SocialBenefitBeforeName,
                                                h.CreatedDate
                                                FROM Members h 
                                                JOIN Genders g ON h.GenderId=g.Id
                                                JOIN Relationships r ON h.RelationshipId=r.Id
                                                LEFT JOIN EducationInfo e ON h.EducationInfoId=e.Id
                                                LEFT JOIN ProfessionInfo p ON h.ProfessionId=p.Id
                                                LEFT JOIN SocialBenefits sbr ON h.SocialBenefitRunningId=sbr.Id
                                                LEFT JOIN SocialBenefits sbe ON h.SocialBenefitEligibleId=sbe.Id
                                                LEFT JOIN SocialBenefits sbb ON h.SocialBenefitBeforeId=sbb.Id
                                                {0} ORDER BY Id OFFSET (({1} - 1) * {2}) ROWS FETCH NEXT {2} ROWS ONLY", searchPrm, pageNo, pageSize);

                string countQuery = string.Format(@"SELECT COUNT(*) FROM Members WHERE IsDeleted=0 AND HoldingNo LIKE N'%{0}%'", holdingNo?.Trim());

                int rowCount = _messageInfoRepository.SQLQuery<int>(countQuery);
                List<VMember> Member = _messageInfoRepository.SQLQueryList<VMember>(query).OrderByDescending(a => a.CreatedDate).ToList();
                return new StaticPagedList<VMember>(Member, pageNo, pageSize, rowCount);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return new StaticPagedList<VMember>(new List<VMember> { }, pageNo, pageSize, 0);
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
