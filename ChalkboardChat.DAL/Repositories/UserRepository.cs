using System;
using System.Collections.Generic;
using System.Linq;
using ChalkboardChat.DAL.Data;
using Microsoft.AspNetCore.Identity;

namespace ChalkboardChat.DAL.Repositories;

public class UserRepository : IUserRepository {
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context) {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Add(IdentityUser user) {
        if (user is null) throw new ArgumentNullException(nameof(user));
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public IdentityUser? GetById(string id) {
        if (string.IsNullOrWhiteSpace(id)) return null;
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }

    public IdentityUser? GetByUserName(string userName) {
        if (string.IsNullOrWhiteSpace(userName)) return null;
        return _context.Users.FirstOrDefault(u => u.UserName == userName);
    }

    public IEnumerable<IdentityUser> GetAll() {
        return _context.Users.AsEnumerable();
    }

    public void Update(IdentityUser user) {
        if (user is null) throw new ArgumentNullException(nameof(user));
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Delete(string id) {
        var entity = GetById(id);
        if (entity is null) return;
        _context.Users.Remove(entity);
        _context.SaveChanges();
    }
}
