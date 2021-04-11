using ClientChat.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientChat.Helpers.MyEventArgs
{
   public class MessageEventArgs : EventArgs
    {
        public ClientMessage Message { get; set; }
    }
}
