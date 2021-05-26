using System;
using System.Collections.Generic;
using UPTax.Data.Infrastructure;
using UPTax.Data.Repository;
using UPTax.Model.Models;

namespace UPTax.Service.Services
{
    public interface IMessageReplyService
    {
        bool Add(MessageReply model);
        bool Update(MessageReply model);
        bool Delete(int id);
        IEnumerable<MessageReply> GetAllByMessageId(int id = 0);
        MessageReply GetDetails(int id);
        bool Save();
    }

    public class MessageReplyService : IMessageReplyService
    {
        private readonly IMessageReplyRepository _messageReplyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MessageReplyService(IMessageReplyRepository MessageReplyRepository, IUnitOfWork unitOfWork)
        {
            _messageReplyRepository = MessageReplyRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(MessageReply model)
        {
            _messageReplyRepository.Add(model);
            return Save();
        }

        public bool Delete(int id)
        {
            MessageReply MessageReply = _messageReplyRepository.GetById(id);
            _messageReplyRepository.Update(MessageReply);
            return Save();
        }
        public bool Update(MessageReply model)
        {
            _messageReplyRepository.Update(model);
            return Save();
        }
        public IEnumerable<MessageReply> GetAllByMessageId(int id = 0)
        {
            return _messageReplyRepository.GetMany(a => a.MessageInfoId == id);
        }
        public MessageReply GetDetails(int id)
        {
            return _messageReplyRepository.GetById(id);
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
