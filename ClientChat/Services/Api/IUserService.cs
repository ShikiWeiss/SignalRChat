using Common.Models;
using System;
using System.Collections.Generic;

namespace ClientChat.Services.Api
{
    public interface IUserService
    {
        void TryRegister(string name, string password);
        void TryLogin(string name, string password);
        bool isNameAndPasswordEmptyOrNull(string name, string password);

        List<User> ConnectedUsers { get; set; }
        User CurrentUser { get; set; }

        event EventHandler UserRegisterSecceded;
        event EventHandler UserRegisterFailed;
        event EventHandler UserLoginSecceded;
        event EventHandler UserLoginFailed;
        event EventHandler RecievedUsers;
        event EventHandler UserConnected;
        event EventHandler UserDisconnected;

        void GetLoggedUsers();
        void LogOut();
    }
}
