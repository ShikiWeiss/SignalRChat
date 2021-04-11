using Common.Models;
using Server.Dal;
using Server.Repository;
using Server.Services.Api;
using System.Collections.Generic;

namespace Server.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatRepository repository;

        public ChatService(ChatRepository repository) => this.repository = repository;

        public bool AddChatRoom(ChatRoom chat) => repository.AddChatRoom(chat);

        private bool AddMessage(Message message) => repository.AddMessage(message);

        public void AddMessageToChatByNames(Message message, string recieverName)
        {
            ChatRoom chat = repository.GetChatByUserNames(message.SenderName, recieverName);
            if (chat == null)
            {
                repository.AddChatRoom(new ChatRoom
                {
                    Messages = new List<Message>
                    {
                        message
                    },
                    User1Name = message.SenderName,
                    User2Name = recieverName
                });
                return;
            }
            message.Chat = chat;
            AddMessage(message);

        }

        public bool AddUser(User user) => repository.AddUser(user);

        public ChatRoom GetChatById(int id) => repository.GetChatByChatId(id);

        public ChatRoom GetChatByUserNames(string user1, string user2) => repository.GetChatByUserNames(user1, user2);

        public IEnumerable<Message> GetMessagesByUserNames(string sender, string reciever)
        {
            ChatRoom chat = repository.GetChatByUserNames(sender, reciever);
            return chat == null ? null : chat.Messages;
        }

    }
}
