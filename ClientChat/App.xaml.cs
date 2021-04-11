using ClientChat.Services;
using ClientChat.Services.Api;
using ClientChat.Services.Implementations;
using ClientChat.ViewModels;
using ClientChat.ViewModels.Views;
using ClientChat.ViewModels.Views.LoginViews;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Windows;

namespace ClientChat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Provider { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection()
            .AddSingleton<MainWindow>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<UsersViewModel>()
            .AddSingleton<LoginViewModel>()
            .AddSingleton<ChatViewModel>()

            .AddSingleton<IConnectionService,ConnectionService>()
            .AddSingleton<IChatService, ChatService>()
            .AddSingleton<IUserService, UserService>();

            Provider = services.BuildServiceProvider();
            var mainWin = Provider.GetRequiredService<MainWindow>();
            mainWin.Show();
        }
    }
}
