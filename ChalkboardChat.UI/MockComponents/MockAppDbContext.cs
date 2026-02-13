
using ChalkboardChat.DAL;
using Microsoft.EntityFrameworkCore;

namespace ChalkboardChat.UI.MockComponents
{
    // MockDatabas (EF Core) för att testa UI och MockMessageService 
    public class MockAppDbContext : DbContext
    {
        public MockAppDbContext(DbContextOptions<MockAppDbContext> options)
            : base(options)
        {
        }
        public DbSet<MessageModel> Messages { get; set; }
    }
}
