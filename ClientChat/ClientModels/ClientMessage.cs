using Common.Models;
using System.Windows.Media.Imaging;

namespace ClientChat.Models
{
    public class ClientMessage : Message
    {
        public BitmapImage Image { get; set; }
    }
}
