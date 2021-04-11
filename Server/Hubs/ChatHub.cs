using Common.Models;
using Microsoft.AspNetCore.SignalR;
using Server.Services;
using Server.Services.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Hubs
{
    public class ChatHub : Hub
    {
        IUserService userService;
        private readonly IChatService chatService;
        private readonly IConnectionService connectionService;

        public ChatHub(IUserService userService, IChatService chatService, IConnectionService connectionService)
        {
            this.userService = userService;
            this.chatService = chatService;
            this.connectionService = connectionService;
        }

        public User Login(string name, string password)
        {
            User user = userService.loginUser(name, password, Context.ConnectionId);
            if (user != null) Clients.Others.SendAsync("UserConnected", user);
            return user;
        }

        public void LogOut(User user)
        {
             userService.LogOut(user);
             Clients.Others.SendAsync("UserDisconnected", user);
        }

        public void Register(User user)
        {
            bool isName =  userService.NameValidation(user.Name);
            bool isPassword =  userService.PasswordValidation(user.Password);
             Clients.Caller.SendAsync("RegisterResult", user, isName, isPassword);
            if (isName && isPassword)
            {
                user = userService.RegisterUser(user, Context.ConnectionId);
                Clients.Others.SendAsync("UserConnected", user);
            }
        }

        public void CheckLogin(string connectionId)
        {
            connectionService.ReplaceUserConnectionIdWithAnother(connectionId, Context.ConnectionId);
        }

        public IEnumerable<User> GetConnectedUsers(User user)
        {
            IEnumerable<User> users =  userService.GetConnectedUsersExceptMe(user);
            return users.Where(u => u.Name != user.Name);
        }

        public IEnumerable<Message> GetMessagesByUserNames(string sender, string reciever)
        {
            var messages = chatService.GetMessagesByUserNames(sender, reciever);
            if (messages == null) return null;
            for (int i = 0; i < messages.ToList().Count; i++)
                messages.ToList()[i].Chat.Messages = null;
            return messages;
        }

        public void SendMessageToUser(Message message, string recieverName)
        {
            Clients.Client(connectionService.GetConnectionIdByName(recieverName))
           .SendAsync("ReceiveMessage", message);
            chatService.AddMessageToChatByNames(message, recieverName);
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("MakeLogin", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string disconnectedUserName = connectionService.GetUserNameByConnectionId(Context.ConnectionId);
            if (disconnectedUserName != null)
                LogOut(new User { Name = connectionService.GetUserNameByConnectionId(Context.ConnectionId) });
            return base.OnDisconnectedAsync(exception);
        }
    }
}
