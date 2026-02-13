using ChalkboardChat.DAL;
using Microsoft.EntityFrameworkCore;

namespace ChalkboardChat.UI.MockComponents
{
    public class MockMessageService
    {
        private readonly MockAppDbContext _appDb;

        public MockMessageService(MockAppDbContext appDb)
        {
            _appDb = appDb;
        }

        public async Task<List<MessageModel>> GetAllMessagesAsync()
        {
            return await _appDb.Messages.OrderByDescending(m => m.Date).ToListAsync();
        }

        public async Task AddMessageAsync(string message, string username)
        {
            var msg = new MessageModel
            {
                Message = message,
                UserName = username,
                Date = DateTime.UtcNow
            };

            _appDb.Messages.Add(msg);
            await _appDb.SaveChangesAsync();
        }
    }
}
