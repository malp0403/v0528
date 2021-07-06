using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v0528.Entities;
using v0528.Helpers;
using v0528.Models;

namespace v0528.Interface
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        Task<PageList<MessageModel>> GetMessagesForUser(MessageParams messageParams);
        Task<IEnumerable<MessageModel>> GetMessageThread(string currentUsername,string recipientUsername);
        Task<bool> SaveAllAsync();

    }
}
