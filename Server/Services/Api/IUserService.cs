using Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface IUserService
    {
        User RegisterUser(User user,string connectionId);
        User loginUser(string name, string password, string connectionId);
        bool NameValidation(string name);
        bool PasswordValidation(string password);
        void LogOut(User user);
        IEnumerable<User> GetConnectedUsersExceptMe(User user);

        //IEnumerable<Message> GetMessagesByChatId(int id);
    }
}