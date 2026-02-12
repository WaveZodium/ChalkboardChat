using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ChalkboardChat.DAL.Repositories;

public interface IUserRepository {
    // Basic CRUD for Identity users
    void Add(IdentityUser user);
    IdentityUser? GetById(string id);
    IdentityUser? GetByUserName(string userName);
    IEnumerable<IdentityUser> GetAll();
    void Update(IdentityUser user);
    void Delete(string id);
}
