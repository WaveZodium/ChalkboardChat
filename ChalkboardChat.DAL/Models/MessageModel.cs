namespace ChalkboardChat.DAL.Models;

public class MessageModel
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Text { get; set; } = string.Empty;

    // Identity link (required for rename/delete rules)
    public string UserId { get; set; } = string.Empty;
}