using ClientChat.Services.Api;
using ClientChat.Views;
using ClientChat.Views.LoginViews;
using Common.Models;
using GalaSoft.MvvmLight;
using System.Windows;
using System.Windows.Controls;

namespace ClientChat.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IChatService service;

        private Grid mainGrid;
        public Grid MainGrid { get { return mainGrid; } set { Set(ref mainGrid, value); } }


        public MainViewModel(IChatService service)
        {
            this.service = service;
            MessengerInstance.Register<User>(this, "GoToChat", GoToChat);
            MessengerInstance.Register<string>(this, "GoToLogin", GoToLogin);

            MainGrid = new Grid();
            MainGrid.Children.Add(new LoginView());
            RaisePropertyChanged(() => MainGrid);
        }

        private void GoToLogin(string name)
        {
            MainGrid = new Grid();
            MainGrid.Children.Add(new LoginView());
        }

        private void GoToChat(User user)
        {
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition());

            MainGrid.RowDefinitions.Add(new RowDefinition());

            MainGrid.ColumnDefinitions[0].Width = new GridLength(200);

            UsersView usersView = new UsersView();
            MainGrid.Children.Add(usersView);

            ChatView chatView = new ChatView();
            Grid.SetColumn(chatView, 1);
            MainGrid.Children.Add(chatView);
        }
    }
}
