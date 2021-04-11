using ClientChat.Services.Api;
using Microsoft.AspNetCore.SignalR.Client;

namespace ClientChat.Services
{
    public class ConnectionService : IConnectionService
    {
        public HubConnection Connection { get; set; }
        public string ConnectionId { get; set; }
        public ConnectionService()
        {

            Connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:1419/ChatHub")
                .WithAutomaticReconnect()
                .Build();
        }

        public void StartConnectionAsync() =>  Connection.StartAsync();
    }
}
