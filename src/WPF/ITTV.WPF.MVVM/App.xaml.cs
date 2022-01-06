using System;
using System.Windows;
using ITTV.WPF.MVVM.Services;
using ITTV.WPF.MVVM.Stores;
using ITTV.WPF.MVVM.ViewModels;
using ITTV.WPF.MVVM.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ITTV.WPF.MVVM
{
    public partial class App
    {
        private readonly IServiceProvider _serviceProvider;
        
        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<MainWindow>();
            
            serviceCollection.AddSingleton<BackgroundVideoViewModel>();
            serviceCollection.AddSingleton<NavigationService<BackgroundVideoViewModel>>();
            
            serviceCollection.AddSingleton<MainViewModel>();
            serviceCollection.AddSingleton<NavigationService<MainViewModel>>();
            
            serviceCollection.AddSingleton<MenuViewModel>();
            serviceCollection.AddSingleton<NavigationService<MenuViewModel>>();
            
            serviceCollection.AddSingleton<GamesViewModel>();
            serviceCollection.AddSingleton<NavigationService<GamesViewModel>>();
            
            serviceCollection.AddSingleton<VideosViewModel>();
            serviceCollection.AddSingleton<NavigationService<VideosViewModel>>();
            
            serviceCollection.AddSingleton<NewsViewModel>();
            serviceCollection.AddSingleton<NavigationService<NewsViewModel>>();
            
            serviceCollection.AddSingleton<TimeTableViewModel>();
            serviceCollection.AddSingleton<NavigationService<TimeTableViewModel>>();

            serviceCollection.AddSingleton<FooterViewModel>();
            serviceCollection.AddSingleton<NavigationService<FooterViewModel>>();
            
            serviceCollection.AddSingleton<NavigationStore>();
        }
    }
}