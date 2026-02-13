using System;
using System.Collections.Generic;
using System.Text;

namespace ChalkboardChat.DAL
{
    public class MessageModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
    }
}
