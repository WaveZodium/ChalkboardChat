using System.Collections.Generic;
using System.Threading.Tasks;
using ChalkboardChat.DAL.Models;

namespace ChalkboardChat.DAL.Repositories;

public interface IMessageRepository
{
    Task AddAsync(MessageEntity message);
    Task<MessageEntity> GetByIdAsync(int id);
    Task<IEnumerable<MessageEntity>> GetAllAsync();
    Task UpdateAsync(MessageEntity message);
    Task DeleteAsync(int id);
}
