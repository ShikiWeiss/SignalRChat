using ClientChat.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientChat.Helpers.MyEventArgs
{
    public class MessagesEventArgs : EventArgs
    {
        public IEnumerable<ClientMessage> Messages { get; set; }
    }
}
