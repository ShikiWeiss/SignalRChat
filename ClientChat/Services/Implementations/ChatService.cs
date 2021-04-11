using ClientChat.Helpers.MyEventArgs;
using ClientChat.Models;
using ClientChat.Services.Api;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ClientChat.Services
{
    public class ChatService : IChatService
    {
        private readonly IConnectionService connectionService;
        private HubConnection connection;

        public event EventHandler MessageRecieved;

        public event EventHandler PastMessagesReceived;

        public ChatService(IConnectionService connectionService)
        {
            this.connectionService = connectionService;
            connection = connectionService.Connection;

            connection.On("ReceiveMessage", (ClientMessage message) => { MessageRecived(new MessageEventArgs { Message = message }); });
        }

        private void MessageRecived(MessageEventArgs e)
        {
            if (e.Message == null) return;
            Dispatcher.CurrentDispatcher.BeginInvoke(() => MessageRecieved.Invoke(this, e));
        }

        public void SendMessage(ClientMessage message, string userName)
        {
            connection.InvokeAsync("SendMessageToUser", message, userName);
        }

        public async void GetUserMessagesByNames(string sender, string receiver)
        {
            IEnumerable<ClientMessage> messages = await connection.InvokeAsync<IEnumerable<ClientMessage>>("GetMessagesByUserNames", sender, receiver);
            await Dispatcher.CurrentDispatcher.BeginInvoke(() => PastMessagesReceived.Invoke(this, new MessagesEventArgs { Messages = messages }));

        }

        public BitmapImage ConvertBytesImageToBitmapImage(byte[] bytesImage)
        {
            using (var stream = new MemoryStream(bytesImage))
            {
                stream.Position = 0;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = stream;
                image.EndInit();
                return image;
            }
        }
    }
}
