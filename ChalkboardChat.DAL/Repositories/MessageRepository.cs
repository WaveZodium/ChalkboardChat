using ChalkboardChat.DAL.Data;
using ChalkboardChat.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace ChalkboardChat.DAL.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly AppDbContext _context;

    public MessageRepository(AppDbContext db)
    {
        _context = db;
    }

    public async Task AddAsync(MessageEntity message)
    {
        if (message is null) throw new ArgumentNullException(nameof(message));
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<MessageEntity>> GetAllAsync()
    {
        return await _context.Messages
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Messages.FindAsync(id);
        if (entity is null) return;
        _context.Messages.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<MessageEntity> GetByIdAsync(int id)
    {
        var entity = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        if (entity is null) throw new KeyNotFoundException($"Message {id} not found.");
        return entity;
    }

    public async Task UpdateAsync(MessageEntity message)
    {
        if (message is null) throw new ArgumentNullException(nameof(message));
        _context.Messages.Update(message);
        await _context.SaveChangesAsync();
    }
}
