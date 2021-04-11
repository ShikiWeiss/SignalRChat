using Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services.Api
{
    public interface IChatService
    {
        ChatRoom GetChatById(int id);
        IEnumerable<Message> GetMessagesByUserNames(string sender, string reciever);
        ChatRoom GetChatByUserNames(string user1, string user2);
        bool AddChatRoom(ChatRoom chat);
        bool AddUser(User user);
        void AddMessageToChatByNames(Message message, string recieverName);
    }
}
