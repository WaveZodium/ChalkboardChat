using ChalkboardChat.DAL.Data;
using ChalkboardChat.DAL.Models;
using ChalkboardChat.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChalkboardChat.DAL.Tests;

public class MessageRepositoryTests {
    private static AppDbContext CreateContext() {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = config.GetConnectionString("AppConnection");

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new AppDbContext(options);
    }

    private static void ResetDatabase(AppDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    [Fact]
    public void Add_Then_GetById_ReturnsMessage()
    {
        using var context = CreateContext();
        ResetDatabase(context);
        var repo = new MessageRepository(context);

        var message = new MessageModel
        {
            Text = "Hello",
            UserId = "user-1"
        };

        repo.Add(message);

        var saved = repo.GetById(message.Id);

        Assert.Equal("Hello", saved.Text);
        Assert.Equal("user-1", saved.UserId);
        Assert.True(saved.Id > 0);
    }

    [Fact]
    public void GetAll_ReturnsMessages_NewestFirst()
    {
        using var context = CreateContext();
        ResetDatabase(context);
        var repo = new MessageRepository(context);

        repo.Add(new MessageModel { Text = "First", UserId = "user-1", CreatedAt = DateTime.UtcNow.AddMinutes(-10) });
        repo.Add(new MessageModel { Text = "Second", UserId = "user-1", CreatedAt = DateTime.UtcNow });

        var all = repo.GetAll().ToList();

        Assert.Equal(2, all.Count);
        Assert.Equal("Second", all[0].Text);
        Assert.Equal("First", all[1].Text);
    }
}
