using System;
using System.Collections.Generic;
using System.Text;
using ChalkboardChat.DAL.Models;

namespace ChalkboardChat.DAL.Repositories;

public interface IMessageRepository {
    public void AddMessage(MessageModel message);
    public MessageModel GetMessageById(int id);
    public IEnumerable<MessageModel> GetAllMessages();
    public void UpdateMessage(MessageModel message);
    public void DeleteMessage(int id);
    public void DeleteMessageById(int id);
}
