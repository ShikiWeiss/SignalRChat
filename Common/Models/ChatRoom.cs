using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class ChatRoom
    {
        public string User1Name { get; set; }
        public string User2Name { get; set; }
        public int Id { get; set; }
        public virtual IEnumerable<Message> Messages { get; set; }
    }
}
