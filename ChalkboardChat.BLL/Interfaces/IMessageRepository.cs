using System;
using System.Collections.Generic;
using System.Text;
using ChalkboardChat.DAL.Models;

namespace ChalkboardChat.BLL.Interfaces
{
    //kontrakt som dal måste implementera
    public interface IMessageRepository
    {
        Task AddAsync(MessageEntity message);
        Task<List<MessageEntity>> GetAllAsync();
    }
}
