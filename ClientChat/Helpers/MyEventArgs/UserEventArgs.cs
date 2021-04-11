using ClientChat.Models;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientChat.Helpers
{
    public class UserEventArgs : EventArgs
    {
        public User User { get; set; }
    }
}
