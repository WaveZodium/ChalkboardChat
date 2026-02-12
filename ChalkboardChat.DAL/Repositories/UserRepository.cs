using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChalkboardChat.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChalkboardChat.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(IdentityUser user)
    {
        if (user is null) throw new ArgumentNullException(nameof(user));
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<IdentityUser?> GetByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return null;
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IdentityUser?> GetByUserNameAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName)) return null;
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task<IEnumerable<IdentityUser>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task UpdateAsync(IdentityUser user)
    {
        if (user is null) throw new ArgumentNullException(nameof(user));
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var entity = await GetByIdAsync(id);
        if (entity is null) return;
        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
