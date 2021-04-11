using ClientChat.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace ClientChat.Services.Api
{
    public interface IChatService
    {
        event EventHandler MessageRecieved;
        event EventHandler PastMessagesReceived;
        void GetUserMessagesByNames(string sender,string receiver);
        void SendMessage(ClientMessage message, string userName);
        BitmapImage ConvertBytesImageToBitmapImage(byte[] bytesImage);
    }
}
