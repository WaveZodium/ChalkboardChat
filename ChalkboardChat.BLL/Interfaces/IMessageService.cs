using System;
using System.Collections.Generic;
using System.Text;
using ChalkboardChat.BLL.DTOs;

namespace ChalkboardChat.BLL.Interfaces;

//kontrakt för ui - ui använder detta
public interface IMessageService
{
    //posta nytt meddelande
    Task<bool> PostMessageAsync(CreateMessageDto dto);
    
    //hämta alla meddelanden för visning
    Task<List<MessageDto>> GetAllMessagesAsync(string currentUserId);
}
