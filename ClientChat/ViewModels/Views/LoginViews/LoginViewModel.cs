using ClientChat.Helpers;
using ClientChat.Helpers.MyEventArgs;
using ClientChat.Services.Api;
using Common.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;

namespace ClientChat.ViewModels.Views.LoginViews
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IChatService chatService;
        private readonly IUserService userService;
        private readonly IConnectionService connectionService;

        private string name; public string Name { get { return name; } set { Set(ref name, value); } }
        private bool isButtonsEnabled; public bool IsButtonsEnabled { get { return isButtonsEnabled; } set { Set(ref isButtonsEnabled, value); } }
        private string password; public string Password { get { return password; } set { Set(ref password, value); } }
        private string errorMsg; public string ErrorMsg { get { return errorMsg; } set { Set(ref errorMsg, value); } }

        public RelayCommand LoginCommand { get; set; }
        public RelayCommand RegisterCommand { get; set; }

        public LoginViewModel(IChatService chatService, IUserService userService, IConnectionService connectionService)
        {
            this.chatService = chatService;
            this.userService = userService;
            this.connectionService = connectionService;

            RegisterCommand = new RelayCommand(Register);
            LoginCommand = new RelayCommand(Login);

            #region UserService events registrations
            userService.UserLoginFailed += LoginFailed;
            userService.UserLoginSecceded += LoginSucceded;
            userService.UserRegisterFailed += RegisterFailed;
            userService.UserRegisterSecceded += RegisterSucceded; 
            #endregion
            IsButtonsEnabled = true;
        }

        private void RegisterSucceded(object sender, EventArgs e)
        {
            if (e is UserValidationEventArgs args)
                GoToChat(args.User);
            ResetUi();
        }

        private void RegisterFailed(object sender, EventArgs e)
        {
            ErrorMsg = "";
            if (e is UserValidationEventArgs args)
            {
                if (!args.IsName)
                    ErrorMsg = "Name already exist!";
                if (!args.IsPassword)
                    ErrorMsg += "\nThe password have to be at least 4 chars!";
                else if (args.IsName && args.IsPassword)
                    ErrorMsg = "There was a problem with the registration process";
            }
            IsButtonsEnabled = true;
        }

        private void LoginSucceded(object sender, EventArgs e)
        {
            if (e is UserEventArgs args)
                GoToChat(args.User);
            ResetUi();
        }

        private void LoginFailed(object sender, EventArgs e)
        {
            ErrorMsg = "Name or password is incorrcet! Or user is already logged in";
            IsButtonsEnabled = true;
        }

        private void GoToChat(User user)
        {
            MessengerInstance.Send(user, "GoToChat");
        }

        private void Login()
        {
            connectionService.StartConnectionAsync();
            if (!userService.isNameAndPasswordEmptyOrNull(Name, Password))
            {
                IsButtonsEnabled = false;
                userService.TryLogin(Name, Password);
            }
        }

        private void Register()
        {
            connectionService.StartConnectionAsync();
            if (!userService.isNameAndPasswordEmptyOrNull(Name, Password))
            {
                IsButtonsEnabled = false;
                userService.TryRegister(Name, Password);
            }
        }

        private void ResetUi()
        {
            ErrorMsg = "";
            Name = "";
            Password = "";
            IsButtonsEnabled = true;
        }

    }
}
