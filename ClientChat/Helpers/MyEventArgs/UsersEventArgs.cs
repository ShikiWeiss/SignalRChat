using ClientChat.Models;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientChat.Helpers.MyEventArgs
{
   public class UsersEventArgs : EventArgs
    {
        public IEnumerable<User> Users { get; set; }
    }
}
