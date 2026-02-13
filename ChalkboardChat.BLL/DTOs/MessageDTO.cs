using System;

namespace ChalkboardChat.BLL.DTOs;

public class MessageDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;

    //används i frontend för att styla meddelanden olika beroende på om de är från den inloggade användaren eller inte
    public bool IsCurrentUser { get; set; }
}
