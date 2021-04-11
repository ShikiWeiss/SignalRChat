using Server.Services.Api;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Server.Services.Implementations
{
    public class ConnectionService : IConnectionService
    {
        public Dictionary<string, string> UserNameToConnectionId { get; set; }
        public Dictionary<string, string> ConnectionIdToUserName { get; set; }


        public ConnectionService()
        {
            UserNameToConnectionId = new Dictionary<string, string>();
            ConnectionIdToUserName = new Dictionary<string, string>();
        }

        public void RemoveUserFromConnectedUsersByConnectionId(string connectionId)
        {
            UserNameToConnectionId.Remove(ConnectionIdToUserName.FirstOrDefault(u => u.Key == connectionId).Value);
            ConnectionIdToUserName.Remove(connectionId);
        }

        public void RemoveUserFromConnectedUsersByName(string name)
        {
            ConnectionIdToUserName.Remove(UserNameToConnectionId.FirstOrDefault(u => u.Key == name).Value);
            UserNameToConnectionId.Remove(name);
        }


        public void ReplaceUserConnectionIdWithAnother(string oldConnectionId, string newConnectionId)
        {
            KeyValuePair<string, string> tempConId = ConnectionIdToUserName.FirstOrDefault(c => c.Key == oldConnectionId);
            if (string.IsNullOrEmpty(tempConId.Key)) return;
            RemoveUserFromConnectedUsersByConnectionId(oldConnectionId);

            AddUserToConnectedUsers(tempConId.Value, newConnectionId);

            Debug.WriteLine($"{tempConId.Value} Connected Again. New connectionId : {newConnectionId}");
        }

        public void AddUserToConnectedUsers(string name, string connectionId)
        {
            ConnectionIdToUserName.Add(connectionId, name);
            UserNameToConnectionId.Add(name, connectionId);
        }

        public List<string> GetConnectedUserNames() => ConnectionIdToUserName.Values.ToList();

        public string GetConnectionIdByName(string recieverName) => UserNameToConnectionId[recieverName];

        public string GetUserNameByConnectionId(string connectionId)
        {
            return ConnectionIdToUserName.GetValueOrDefault(connectionId);
        }

        public bool IsUserAlreadyLoggedIn(string name)
        {
            return UserNameToConnectionId.Any(u => u.Key == name);
        }
    }
}
