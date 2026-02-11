using System;
using System.Collections.Generic;
using System.Linq;
using ChalkboardChat.DAL.Data;
using ChalkboardChat.DAL.Models;

namespace ChalkboardChat.DAL.Repositories;

public class MessageRepository : IMessageRepository {
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext db) {
        _context = db;
    }

    public void Add(MessageModel message) {
        if (message is null) throw new ArgumentNullException(nameof(message));
        _context.Messages.Add(message);
        _context.SaveChanges();
    }

    public void Delete(int id) {
        var entity = _context.Messages.Find(id);
        if (entity is null) return;
        _context.Messages.Remove(entity);
        _context.SaveChanges();
    }

    public IEnumerable<MessageModel> GetAll() {
        // Requirement: newest messages first
        return _context.Messages
            .OrderByDescending(m => m.CreatedAt)
            .ToList();
    }

    public MessageModel GetById(int id) {
        var entity = _context.Messages.FirstOrDefault(m => m.Id == id);
        if (entity is null) throw new KeyNotFoundException($"Message {id} not found.");
        return entity;
    }

    public void Update(MessageModel message) {
        if (message is null) throw new ArgumentNullException(nameof(message));
        _context.Messages.Update(message);
        _context.SaveChanges();
    }
}
