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

    private static async Task ResetDatabaseAsync(AppDbContext context)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    [Fact]
    public async Task Add_Then_GetById_ReturnsMessage()
    {
        await using var context = CreateContext();
        await ResetDatabaseAsync(context);
        var repo = new MessageRepository(context);

        var message = new MessageEntity
        {
            Text = "Hello",
            UserId = "user-1"
        };

        await repo.AddAsync(message);

        var saved = await repo.GetByIdAsync(message.Id);

        Assert.Equal("Hello", saved.Text);
        Assert.Equal("user-1", saved.UserId);
        Assert.True(saved.Id > 0);
    }

    [Fact]
    public async Task GetAll_ReturnsMessages_NewestFirst()
    {
        await using var context = CreateContext();
        await ResetDatabaseAsync(context);
        var repo = new MessageRepository(context);

        await repo.AddAsync(new MessageEntity { Text = "First", UserId = "user-1", CreatedAt = DateTime.UtcNow.AddMinutes(-10) });
        await repo.AddAsync(new MessageEntity { Text = "Second", UserId = "user-1", CreatedAt = DateTime.UtcNow });

        var all = (await repo.GetAllAsync()).ToList();

        Assert.Equal(2, all.Count);
        Assert.Equal("Second", all[0].Text);
        Assert.Equal("First", all[1].Text);
    }

    [Fact]
    public async Task Update_ChangesMessage()
    {
        await using var context = CreateContext();
        await ResetDatabaseAsync(context);
        var repo = new MessageRepository(context);

        var message = new MessageEntity { Text = "Old", UserId = "user-1" };
        await repo.AddAsync(message);

        message.Text = "New";
        await repo.UpdateAsync(message);

        var updated = await repo.GetByIdAsync(message.Id);
        Assert.Equal("New", updated.Text);
    }

    [Fact]
    public async Task Delete_RemovesMessage()
    {
        await using var context = CreateContext();
        await ResetDatabaseAsync(context);
        var repo = new MessageRepository(context);

        var message = new MessageEntity { Text = "ToDelete", UserId = "user-1" };
        await repo.AddAsync(message);

        await repo.DeleteAsync(message.Id);

        Assert.Empty(await repo.GetAllAsync());
    }
}
