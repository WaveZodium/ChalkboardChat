using System;
using System.Collections.Generic;
using System.Text;

namespace ChalkboardChat.DAL.DTOs;

public class MessageDTO {
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Message { get; set; }
    public string Username { get; set; }
}
