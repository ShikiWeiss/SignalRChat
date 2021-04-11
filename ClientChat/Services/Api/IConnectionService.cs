using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientChat.Services.Api
{
    public interface IConnectionService
    {
        HubConnection Connection { get; set; }
        string ConnectionId { get; set; }
        void StartConnectionAsync();
    }
}
