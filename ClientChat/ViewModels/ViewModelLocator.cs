using ClientChat.ViewModels.Views;
using ClientChat.ViewModels.Views.LoginViews;
using Microsoft.Extensions.DependencyInjection;

namespace ClientChat.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel Main => App.Provider.GetRequiredService<MainViewModel>();

        public UsersViewModel Users => App.Provider.GetRequiredService<UsersViewModel>();

        public ChatViewModel Chat => App.Provider.GetRequiredService<ChatViewModel>();

        public LoginViewModel Login => App.Provider.GetRequiredService<LoginViewModel>();

    }
}
