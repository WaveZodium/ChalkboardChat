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

    [Fact]
    public void Add_Then_GetById_ReturnsMessage()
    {
        using var context = CreateContext();
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
}
