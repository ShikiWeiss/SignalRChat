using Common.Models;
using Server.Dal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Repository
{
    public class ChatRepository
    {
        private readonly ChatDB chatDB;

        public ChatRepository(ChatDB chatDB) => this.chatDB = chatDB;

        public User GetUserByName(string name)
        {
            return chatDB.Users.FirstOrDefault(u => u.Name.Equals(name));
        }

        public bool AddUser(User user)
        {
            try
            {
                chatDB.Users.Add(user);
                chatDB.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public ChatRoom GetChatByUserNames(string user1, string user2)
        {
            if (chatDB.Chats.Count() < 1) return null;
            ChatRoom filterChats = chatDB.Chats.FirstOrDefault(c => c.User1Name == user1 && c.User2Name == user2 || c.User1Name == user2 && c.User2Name == user1);
            return filterChats;
        }

        public IEnumerable<Message> GetMessagesByChatId(int id)
        {
            ChatRoom chat = chatDB.Chats.FirstOrDefault(c => c.Id == id);
            return chat == null ? null : chat.Messages;
        }

        public bool AddChatRoom(ChatRoom chat)
        {
            try
            {
                chatDB.Chats.Add(chat);
                chatDB.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public bool AddMessage(Message message)
        {
            try
            {
                chatDB.Messages.Add(message);
                chatDB.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        public ChatRoom GetChatByChatId(int id) => chatDB.Chats.FirstOrDefault(c => c.Id == id);
    }
}
