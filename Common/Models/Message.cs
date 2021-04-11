using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string SenderName { get; set; }
        public virtual ChatRoom Chat { get; set; }
        public byte[] BytesImage { get; set; }
    }
}
