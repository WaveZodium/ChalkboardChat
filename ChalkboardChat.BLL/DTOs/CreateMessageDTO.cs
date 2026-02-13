using System;
using System.Collections.Generic;
using System.Text;

namespace ChalkboardChat.BLL.DTOs;

public class CreateMessageDto
{
    public string Message { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}
