using System;
using System.Collections.Generic;
using System.Text;
using ChalkboardChat.BLL.DTOs;
using ChalkboardChat.BLL.Interfaces;
using ChalkboardChat.DAL.Models;
//using ChalkboardChat.DAL.Repositories;
using Microsoft.AspNetCore.Identity;

namespace ChalkboardChat.BLL.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repository;
    private readonly UserManager<IdentityUser> _userManager;

    public MessageService(IMessageRepository repository, UserManager<IdentityUser> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    //posta nytt meddelande
    public async Task<bool> PostMessageAsync(CreateMessageDto dto)
    {
        //validera att meddelandet inte är tomt
        if (string.IsNullOrWhiteSpace(dto.Message))
            return false;

        //validera att användaren finns
        if (string.IsNullOrWhiteSpace(dto.UserId))
            return false;

        //skapa nytt meddelande
        var message = new MessageEntity
        {
            Text = dto.Message,
            UserId = dto.UserId,
            CreatedAt = DateTime.Now
        };

        //spara via repository
        await _repository.AddAsync(message);

        return true;
    }

    //hämta alla meddelanden
    public async Task<List<MessageDto>> GetAllMessagesAsync(string currentUserId)
    {
        //hämta meddelanden från repository
        var messages = await _repository.GetAllAsync();

        //konvertera till dto och sortera nyast först
        var messageDtos = new List<MessageDto>();

        foreach (var msg in messages)
        {
            //hämta användare från identity
            var user = await _userManager.FindByIdAsync(msg.UserId);

            messageDtos.Add(new MessageDto
            {
                Id = msg.Id,
                Date = msg.CreatedAt,
                Message = msg.Text,
                Username = user != null ? user.UserName ?? "(deleted user)" : "(deleted user)",
                IsCurrentUser = msg.UserId == currentUserId
            });
        }

        //sortera nyast först
        return messageDtos.OrderByDescending(m => m.Date).ToList();
    }
}
