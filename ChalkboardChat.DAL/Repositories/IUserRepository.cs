using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ChalkboardChat.DAL.Repositories;

public interface IUserRepository
{
    Task AddAsync(IdentityUser user);
    Task<IdentityUser?> GetByIdAsync(string id);
    Task<IdentityUser?> GetByUserNameAsync(string userName);
    Task<IEnumerable<IdentityUser>> GetAllAsync();
    Task UpdateAsync(IdentityUser user);
    Task DeleteAsync(string id);
}
