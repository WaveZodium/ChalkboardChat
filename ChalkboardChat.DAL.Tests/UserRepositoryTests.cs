using ChalkboardChat.DAL.Data;
using ChalkboardChat.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ChalkboardChat.DAL.Tests;

public class UserRepositoryTests
{
    private static AuthDbContext CreateContext()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = config.GetConnectionString("AuthConnection");

        var options = new DbContextOptionsBuilder<AuthDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new AuthDbContext(options);
    }

    private static async Task ResetDatabaseAsync(AuthDbContext context)
    {
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    [Fact]
    public async Task Add_Then_GetById_ReturnsUser()
    {
        await using var context = CreateContext();
        await ResetDatabaseAsync(context);
        var repo = new UserRepository(context);

        var user = new IdentityUser { UserName = "alice" };
        await repo.AddAsync(user);

        var saved = await repo.GetByIdAsync(user.Id);

        Assert.NotNull(saved);
        Assert.Equal("alice", saved!.UserName);
    }

    [Fact]
    public async Task GetByUserName_ReturnsUser()
    {
        await using var context = CreateContext();
        await ResetDatabaseAsync(context);
        var repo = new UserRepository(context);

        var user = new IdentityUser { UserName = "bob" };
        await repo.AddAsync(user);

        var saved = await repo.GetByUserNameAsync("bob");

        Assert.NotNull(saved);
        Assert.Equal(user.Id, saved!.Id);
    }

    [Fact]
    public async Task GetAll_ReturnsUsers()
    {
        await using var context = CreateContext();
        await ResetDatabaseAsync(context);
        var repo = new UserRepository(context);

        await repo.AddAsync(new IdentityUser { UserName = "user1" });
        await repo.AddAsync(new IdentityUser { UserName = "user2" });

        var all = (await repo.GetAllAsync()).ToList();

        Assert.Equal(2, all.Count);
    }

    [Fact]
    public async Task Update_ChangesUserName()
    {
        await using var context = CreateContext();
        await ResetDatabaseAsync(context);
        var repo = new UserRepository(context);

        var user = new IdentityUser { UserName = "oldname" };
        await repo.AddAsync(user);

        user.UserName = "newname";
        await repo.UpdateAsync(user);

        var updated = await repo.GetByIdAsync(user.Id);

        Assert.Equal("newname", updated!.UserName);
    }

    [Fact]
    public async Task Delete_RemovesUser()
    {
        await using var context = CreateContext();
        await ResetDatabaseAsync(context);
        var repo = new UserRepository(context);

        var user = new IdentityUser { UserName = "todelete" };
        await repo.AddAsync(user);

        await repo.DeleteAsync(user.Id);

        Assert.Empty(await repo.GetAllAsync());
    }
}