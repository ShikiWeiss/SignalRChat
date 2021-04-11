using Common.Models;
using System;

namespace ClientChat.Helpers.MyEventArgs
{
    public class UserValidationEventArgs : EventArgs
    {
        public bool IsName { get; set; }
        public bool IsPassword { get; set; }
        public User User { get; set; }
    }
}
