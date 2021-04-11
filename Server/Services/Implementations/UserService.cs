using Common.Models;
using Server.Dal;
using Server.Repository;
using Server.Services.Api;
using System.Collections.Generic;

namespace Server.Services
{
    public class UserService : IUserService
    {
        private readonly IConnectionService connectionService;
        private readonly ChatRepository repository;
        public UserService(IConnectionService connectionService, ChatRepository repository)
        {
            this.connectionService = connectionService;
            this.repository = repository;
        }

        public User RegisterUser(User user, string connectionId)
        {
            connectionService.AddUserToConnectedUsers(user.Name, connectionId);
            return repository.AddUser(user) ? user : null;
        }

        private User GetUserByNameAndPassword(string name, string password)
        {
            User user = repository.GetUserByName(name);
            if (user == null) return null;
            if (!user.Password.Equals(password)) return null;
            return user;
        }

        public User loginUser(string name, string password, string connectionId)
        {
            if (connectionService.IsUserAlreadyLoggedIn(name)) return null;
            User user = GetUserByNameAndPassword(name, password);
            if (user != null)
                connectionService.AddUserToConnectedUsers(name, connectionId);
            return user;
        }

        public bool NameValidation(string name)
        {
            User user = repository.GetUserByName(name);
            if (user == null)
                return true;
            return false;
        }

        public bool PasswordValidation(string password)
        {
            if (password.Length < 4)
                return false;
            return true;
        }

        public void LogOut(User user)
        {
            connectionService.RemoveUserFromConnectedUsersByName(user.Name);
        }

        public IEnumerable<User> GetConnectedUsersExceptMe(User user)
        {
            List<string> names = connectionService.GetConnectedUserNames();

            List<User> users = new List<User>();
            foreach (string name in names)
            {
                if (name == user.Name) continue;
                users.Add(repository.GetUserByName(name));
            }
            return users;
        }
    }
}
