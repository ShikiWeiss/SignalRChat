using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Api
{
    public interface IConnectionService
    {
        void RemoveUserFromConnectedUsersByConnectionId(string connectionId);
        void RemoveUserFromConnectedUsersByName(string name);
        void ReplaceUserConnectionIdWithAnother(string oldConnectionId, string newConnectionId);
        void AddUserToConnectedUsers(string name, string connectionId);
        List<string> GetConnectedUserNames();
        string GetConnectionIdByName(string recieverName);
        bool IsUserAlreadyLoggedIn(string name);
        string GetUserNameByConnectionId(string connectionId);
    }
}
