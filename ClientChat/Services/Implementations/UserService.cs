using ClientChat.Helpers;
using ClientChat.Helpers.MyEventArgs;
using ClientChat.Services.Api;
using Common.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace ClientChat.Services.Implementations
{
    public class UserService : IUserService
    {
        private HubConnection connection;
        private readonly IConnectionService connectionService;
        public List<User> ConnectedUsers { get; set; }
        public User CurrentUser { get; set; }


        private bool isFirstLogin = true;


        #region Events
        public event EventHandler UserRegisterSecceded;
        public event EventHandler UserRegisterFailed;
        public event EventHandler UserLoginSecceded;
        public event EventHandler UserLoginFailed;
        public event EventHandler RecievedUsers;
        public event EventHandler UserConnected;
        public event EventHandler UserDisconnected; 
        #endregion

        public UserService(IConnectionService connectionService)
        {
            this.connectionService = connectionService;
            connection = connectionService.Connection;
            ConnectedUsers = new List<User>();

            #region SignalR registrations
            connection.On("UserConnected", (User user) => { OnUserConnected(user, new UserEventArgs() { User = user }); });

            connection.On("RegisterResult", (User user, bool isName, bool isPass) => { RegisterResult(new UserValidationEventArgs { User = user, IsName = isName, IsPassword = isPass }); });

            connection.On("MakeLogin", (string connectionId) => CheckIfNeedLogin(connectionId));

            connection.On("UserDisconnected", (User user) => { OnUserDisconnected(user); }); 
            #endregion
        }

        private void OnUserDisconnected(User user)
        {
            UserDisconnected?.Invoke(this,new UserEventArgs { User = user });
        }

        private void CheckIfNeedLogin(string connectionId)
        {
            if (!isFirstLogin)
            {
                connection.InvokeAsync("CheckLogin", connectionService.ConnectionId);
            }
            else isFirstLogin = false;
            connectionService.ConnectionId = connectionId;
        }

        private void OnUserConnected(User user, UserEventArgs e)
        {
            ConnectedUsers.Add(user);
            Dispatcher.CurrentDispatcher.BeginInvoke(() => UserConnected?.Invoke(this, e));
        }

        private void RegisterResult(UserValidationEventArgs e)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(() =>
            {
                if (e.User != null && e.IsName && e.IsPassword)
                {
                    CurrentUser = e.User;
                    UserRegisterSecceded?.Invoke(this, e);
                }
                else
                    UserRegisterFailed?.Invoke(this, e);
            });
        }

        public void TryRegister(string name, string password)
        {
            connection.InvokeAsync("Register", new User() { Name = name, Password = password });
        }

        public async void TryLogin(string name, string password)
        {
            User user = await connection.InvokeAsync<User>("Login", name, password);
            await Dispatcher.CurrentDispatcher.BeginInvoke(() =>
            {
                if (user != null)
                {
                    CurrentUser = user;
                    UserLoginSecceded?.Invoke(this, new UserEventArgs { User = user });
                }
                else
                    UserLoginFailed?.Invoke(this, null);
            });
        }

        public bool isNameAndPasswordEmptyOrNull(string name, string password)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(password))
                return false;
            return true;
        }

        public async void GetLoggedUsers()
        {
            IEnumerable<User> users = await connection.InvokeAsync<IEnumerable<User>>("GetConnectedUsers", CurrentUser);
            ConnectedUsers = users.ToList();
            await Dispatcher.CurrentDispatcher.BeginInvoke(() => RecievedUsers?.Invoke(this, new UsersEventArgs { Users = users }));
        }

        public void LogOut()
        {
            connection.InvokeAsync("LogOut", CurrentUser);
        }
    }
}
