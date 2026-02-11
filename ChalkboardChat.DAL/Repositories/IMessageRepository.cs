using System;
using System.Collections.Generic;
using System.Text;
using ChalkboardChat.DAL.Models;

namespace ChalkboardChat.DAL.Repositories;

public interface IMessageRepository {
    public void Add(MessageModel message);
    public MessageModel GetById(int id);
    public IEnumerable<MessageModel> GetAll();
    public void Update(MessageModel message);
    public void Delete(int id);
}
