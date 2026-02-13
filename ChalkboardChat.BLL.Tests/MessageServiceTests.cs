using ChalkboardChat.BLL.DTOs;
using ChalkboardChat.BLL.Interfaces;
using ChalkboardChat.BLL.Services;
using ChalkboardChat.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace ChalkboardChat.BLL.Tests;

public class MessageServiceTests
{
    [Fact]
    public async Task GetAllMessageAsync_SorterarNyastFörst()
    {
        //arrange
        var fakeRepo = new FakeMessageRepository();
        var fakeUserManager = new FakeUserManager();
        fakeUserManager.AddUser("user1", "testuser");
        
        fakeRepo.Messages.Add(new MessageEntity 
        { 
            Id = 1, 
            Text = "äldre", 
            UserId = "user1", 
            CreatedAt = DateTime.Now.AddHours(-2) 
        });
        fakeRepo.Messages.Add(new MessageEntity 
        { 
            Id = 2, 
            Text = "nyare", 
            UserId = "user1", 
            CreatedAt = DateTime.Now.AddHours(-1) 
        });

        var service = new MessageService(fakeRepo, fakeUserManager);

        //act
        var result = await service.GetAllMessagesAsync("user1");

        //assert
        Assert.Equal("nyare", result[0].Message);
        Assert.Equal("äldre", result[1].Message);
    }

    [Fact]
    public async Task GetAllMessagesAsync_AnvändareDeleted_VisarDeletedUser()
    {
        //arrange
        var fakeRepo = new FakeMessageRepository();
        var fakeUserManager = new FakeUserManager();
        
        fakeRepo.Messages.Add(new MessageEntity 
        { 
            Id = 1, 
            Text = "meddelande", 
            UserId = "deletedUser", 
            CreatedAt = DateTime.Now 
        });

        var service = new MessageService(fakeRepo, fakeUserManager);

        //act
        var result = await service.GetAllMessagesAsync("user1");

        //assert
        Assert.Equal("(deleted user)", result[0].Username);
    }

    [Fact]
    public async Task PostMessageAsync_TomtMeddelande_ReturnerarFalse()
    {
        //arrange
        var fakeRepo = new FakeMessageRepository();
        var fakeUserManager = new FakeUserManager();
        var service = new MessageService(fakeRepo, fakeUserManager);
        
        var dto = new CreateMessageDto
        {
            Message = "",
            UserId = "user123"
        };

        //act
        var result = await service.PostMessageAsync(dto);

        //assert
        Assert.False(result);
        Assert.Empty(fakeRepo.Messages);
    }
}

//fake repository
public class FakeMessageRepository : IMessageRepository
{
    public List<MessageEntity> Messages { get; } = new();

    public Task AddAsync(MessageEntity message)
    {
        Messages.Add(message);
        return Task.CompletedTask;
    }

    public Task<List<MessageEntity>> GetAllAsync()
    {
        return Task.FromResult(Messages);
    }
}

//fake usermanager
public class FakeUserManager : UserManager<IdentityUser>
{
    private readonly Dictionary<string, IdentityUser> _users = new();

    public FakeUserManager() : base(
        new FakeUserStore(), null, null, null, null, null, null, null, null)
    {
    }

    public void AddUser(string id, string username)
    {
        _users[id] = new IdentityUser { Id = id, UserName = username };
    }

    public override Task<IdentityUser?> FindByIdAsync(string userId)
    {
        _users.TryGetValue(userId, out var user);
        return Task.FromResult(user);
    }
}

//fake userstore
public class FakeUserStore : IUserStore<IdentityUser>
{
    public void Dispose() { }
    
    public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken) 
        => Task.FromResult(user.Id);
    
    public Task<string?> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken) 
        => Task.FromResult(user.UserName);
    
    public Task SetUserNameAsync(IdentityUser user, string? userName, CancellationToken cancellationToken) 
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }
    
    public Task<string?> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken) 
        => Task.FromResult(user.NormalizedUserName);
    
    public Task SetNormalizedUserNameAsync(IdentityUser user, string? normalizedName, CancellationToken cancellationToken) 
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }
    
    public Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken) 
        => Task.FromResult(IdentityResult.Success);
    
    public Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken) 
        => Task.FromResult(IdentityResult.Success);
    
    public Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken) 
        => Task.FromResult(IdentityResult.Success);
    
    public Task<IdentityUser?> FindByIdAsync(string userId, CancellationToken cancellationToken) 
        => Task.FromResult<IdentityUser?>(null);
    
    public Task<IdentityUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) 
        => Task.FromResult<IdentityUser?>(null);
}
