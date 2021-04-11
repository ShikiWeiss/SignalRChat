using ClientChat.Helpers;
using ClientChat.Helpers.MyEventArgs;
using ClientChat.Models;
using ClientChat.Services.Api;
using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace ClientChat.ViewModels.Views
{
    public class ChatViewModel : ViewModelBase
    {
        private User contactUser; public User ContactUser { get { return contactUser; } set { Set(ref contactUser, value); } }
        public User MyUser { get; set; }
        private string fileName; public string FileName { get { return fileName; } set { Set(ref fileName, value); } }
        private bool isContact; public bool IsContact { get { return isContact; } set { Set(ref isContact, value); } }
        private ClientMessage message; public ClientMessage Message { get { return message; } set { message = value; } }
        private ObservableCollection<ClientMessage> messages; public ObservableCollection<ClientMessage> Messages { get { return messages; } set { Set(ref messages, value); } }


        private readonly IChatService chatService;
        private readonly IUserService userService;

        #region Commands
        public RelayCommand LogOutCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }
        public RelayCommand UploadeImageCommand { get; set; } 
        #endregion

        public ChatViewModel(IChatService chatService, IUserService userService)
        {
            this.chatService = chatService;
            this.userService = userService;
            MessengerInstance.Register<User>(this, "SelectedUserChanged", ContactUserChanged);

            MyUser = userService.CurrentUser;
            #region PropertiesInit
            ContactUser = new User();
            Messages = new ObservableCollection<ClientMessage>();
            Message = new ClientMessage(); 
            #endregion

            #region CommandsInit
            LogOutCommand = new RelayCommand(LogOut);
            SendMessageCommand = new RelayCommand(SendMessage);
            UploadeImageCommand = new RelayCommand(UploadeImage);
            #endregion

            #region EventsRegistrations
            chatService.PastMessagesReceived += MessagesReceived;
            chatService.MessageRecieved += MessageRecieved;
            userService.UserDisconnected += UserDisconnected; 
            #endregion
        }

        private void UploadeImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.SafeFileName;
                Message.BytesImage = File.ReadAllBytes(openFileDialog.FileName);
            }
        }

        private void UserDisconnected(object sender, EventArgs e)
        {
            if (e is UserEventArgs args)
            {
                if (ContactUser == null || args.User.Name == ContactUser.Name)
                    Messages.Clear();
            }
        }

        private void LogOut()
        {
            userService.LogOut();
            MessengerInstance.Send(userService.CurrentUser.Name, "GoToLogin");
            Messages.Clear();
        }

        private void MessageRecieved(object sender, EventArgs e)
        {
            if (e is MessageEventArgs args && args.Message != null)
                if (args.Message.SenderName.Equals(ContactUser.Name))
                {
                    if (args.Message.BytesImage != null)
                        args.Message.Image = chatService.ConvertBytesImageToBitmapImage(args.Message.BytesImage);
                    else
                        args.Message.Image = new BitmapImage();
                    Messages.Add(args.Message);
                }
        }

        private void SendMessage()
        {
            if (string.IsNullOrEmpty(Message.Content) && Message.BytesImage == null) return;
            ClientMessage message = new ClientMessage { Content = Message.Content, Time = DateTime.Now, SenderName = userService.CurrentUser.Name };
            if (Message.BytesImage != null)
            {
                message.BytesImage = new byte[Message.BytesImage.Length];
                Message.BytesImage.CopyTo(new Memory<byte>(message.BytesImage));
            }
            chatService.SendMessage(message, ContactUser.Name);
            if (Message.BytesImage != null)
                message.Image = chatService.ConvertBytesImageToBitmapImage(Message.BytesImage);
            message.SenderName = "Me";
            Messages.Add(message);
            Message.BytesImage = null;
            FileName = "";
        }

        private void ContactUserChanged(User user)
        {
            if (user == null)
                IsContact = false;
            else
                IsContact = true;
            ContactUser = user;
            Messages.Clear();
        }

        private void MessagesReceived(object sender, EventArgs e)
        {
            if (e is MessagesEventArgs args && args.Messages != null)
            {
                foreach (ClientMessage message in args.Messages)
                {
                    if (message.BytesImage != null)
                        message.Image = chatService.ConvertBytesImageToBitmapImage(message.BytesImage);
                    if (message.SenderName == userService.CurrentUser.Name)
                        message.SenderName = "Me";
                    Messages.Add(message);
                }
            }
        }
    }
}
