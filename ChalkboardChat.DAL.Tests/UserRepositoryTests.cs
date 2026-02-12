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

    private static void ResetDatabase(AuthDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    [Fact]
    public void Add_Then_GetById_ReturnsUser()
    {
        using var context = CreateContext();
        ResetDatabase(context);
        var repo = new UserRepository(context);

        var user = new IdentityUser { UserName = "alice" };
        repo.Add(user);

        var saved = repo.GetById(user.Id);

        Assert.NotNull(saved);
        Assert.Equal("alice", saved!.UserName);
    }

    [Fact]
    public void GetByUserName_ReturnsUser()
    {
        using var context = CreateContext();
        ResetDatabase(context);
        var repo = new UserRepository(context);

        var user = new IdentityUser { UserName = "bob" };
        repo.Add(user);

        var saved = repo.GetByUserName("bob");

        Assert.NotNull(saved);
        Assert.Equal(user.Id, saved!.Id);
    }

    [Fact]
    public void GetAll_ReturnsUsers()
    {
        using var context = CreateContext();
        ResetDatabase(context);
        var repo = new UserRepository(context);

        repo.Add(new IdentityUser { UserName = "user1" });
        repo.Add(new IdentityUser { UserName = "user2" });

        var all = repo.GetAll().ToList();

        Assert.Equal(2, all.Count);
    }

    [Fact]
    public void Update_ChangesUserName()
    {
        using var context = CreateContext();
        ResetDatabase(context);
        var repo = new UserRepository(context);

        var user = new IdentityUser { UserName = "oldname" };
        repo.Add(user);

        user.UserName = "newname";
        repo.Update(user);

        var updated = repo.GetById(user.Id);

        Assert.Equal("newname", updated!.UserName);
    }

    [Fact]
    public void Delete_RemovesUser()
    {
        using var context = CreateContext();
        ResetDatabase(context);
        var repo = new UserRepository(context);

        var user = new IdentityUser { UserName = "todelete" };
        repo.Add(user);

        repo.Delete(user.Id);

        Assert.Empty(repo.GetAll());
    }
}