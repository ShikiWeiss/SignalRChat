using ClientChat.Helpers;
using ClientChat.Services.Api;
using Common.Models;
using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClientChat.ViewModels.Views
{
    public class UsersViewModel : ViewModelBase
    {
        private readonly IChatService chatService;
        private readonly IUserService userService;

        public ObservableCollection<User> Users { get; set; }

        private User selectedUser;
        public User SelectedUser { get { return selectedUser; } set { Set(ref selectedUser, value); ContactSelectionChanged(); } }

        public UsersViewModel(IChatService chatService, IUserService userService)
        {
            this.chatService = chatService;
            this.userService = userService;
            Users = new ObservableCollection<User>();
            userService.GetLoggedUsers();

            #region EventsRegistrations
            userService.UserConnected += UserConnectedHandler;
            userService.RecievedUsers += RecivedConnectedUsers;
            userService.UserDisconnected += UserDisconnected;
            #endregion
        }
        private void ContactSelectionChanged()
        {
            if (selectedUser != null)
                chatService.GetUserMessagesByNames(userService.CurrentUser.Name, selectedUser.Name);
            MessengerInstance.Send(selectedUser, "SelectedUserChanged");
        }

        private void UserDisconnected(object sender, EventArgs e)
        {
            if (e is UserEventArgs args)
            {
                Users.Remove(Users.FirstOrDefault(u => u.Name == args.User.Name));
            }
        }

        private void RecivedConnectedUsers(object sender, EventArgs e)
        {
            foreach (User user in userService.ConnectedUsers)
            {
                Users.Add(user);
            }
        }

        private void UserConnectedHandler(object sender, EventArgs e)
        {
            if (e is UserEventArgs args)
            {
                Users.Add(args.User);
            }
        }
    }
}
