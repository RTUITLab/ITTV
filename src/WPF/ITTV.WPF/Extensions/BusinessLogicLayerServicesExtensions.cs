using ITTV.WPF.BackgroundServices.Cache;
using ITTV.WPF.BackgroundServices.Tracking;
using ITTV.WPF.Commands;
using ITTV.WPF.Commands.BackgroundVideos;
using ITTV.WPF.Core.Providers.MireaApi;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Services.ApiClient;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.Utilities.Tracking;
using ITTV.WPF.ViewModels;
using ITTV.WPF.ViewModels.Games;
using ITTV.WPF.ViewModels.News;
using ITTV.WPF.ViewModels.Schedule;
using ITTV.WPF.ViewModels.Videos;
using ITTV.WPF.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ITTV.WPF.Extensions
{
    public static class BusinessLogicLayerServicesExtensions
    {
        public static void AddBusinessLogicLayerServicesExtensions(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<MainWindow>();
            
            AddViewModels(serviceCollection);
            AddStores(serviceCollection);
            AddCommands(serviceCollection);
            AddServices(serviceCollection);
            AddKinectServices(serviceCollection);
        }

        private static void AddStores(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<NavigationStore>();
            serviceCollection.AddSingleton<NotificationStore>();
        }
        
        private static void AddServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<BackgroundVideoPlaylistService>();
            serviceCollection.AddScoped<BackgroundVideoEndedCommand>();
            serviceCollection.AddSingleton<UserInterfaceManager>();

            serviceCollection.AddSingleton<IMireaApiClient, MireaApiClient>();
            serviceCollection.AddScoped<MireaApiProvider>();
            serviceCollection.AddScoped<ScheduleManager>();
            
            serviceCollection.AddSingleton<NavigationService<TimeTableViewModel>>();
            serviceCollection.AddSingleton<NavigationService<NewsElementViewModel>>();
            serviceCollection.AddSingleton<NavigationService<NewsViewModel>>();
            serviceCollection.AddSingleton<NavigationService<VideosViewModel>>();
            serviceCollection.AddSingleton<NavigationService<GamesViewModel>>();
            serviceCollection.AddSingleton<NavigationService<MenuViewModel>>();
            serviceCollection.AddSingleton<NavigationService<MainViewModel>>();
            serviceCollection.AddSingleton<NavigationService<BackgroundVideoViewModel>>();
        }

        private static void AddCommands(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<NavigateCommand<TimeTableViewModel>>();
            serviceCollection.AddScoped<NavigateCommand<NewsElementViewModel>>();
            serviceCollection.AddScoped<NavigateCommand<NewsViewModel>>();
            serviceCollection.AddScoped<NavigateCommand<VideosViewModel>>();
            serviceCollection.AddScoped<NavigateCommand<GamesViewModel>>();
            serviceCollection.AddScoped<NavigateCommand<MenuViewModel>>();
            serviceCollection.AddSingleton<BackgroundVideoViewModel>();
            serviceCollection.AddScoped<NavigateBackgroundVideoAndClearHistoryCommand>();
        }

        private static void AddViewModels(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<MainViewModel>();
            serviceCollection.AddSingleton<MenuViewModel>();
            serviceCollection.AddSingleton<GamesViewModel>();
            serviceCollection.AddSingleton<VideosViewModel>();
            serviceCollection.AddSingleton<NewsViewModel>();
            serviceCollection.AddSingleton<NewsElementViewModel>();
            serviceCollection.AddSingleton<TimeTableViewModel>();
            serviceCollection.AddSingleton<FooterViewModel>();
            serviceCollection.AddSingleton<NotificationViewModel>();
        }

        private static void AddKinectServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<KinectTrackingUtility>();

            serviceCollection.AddSingleton<KinectTrackingHostedService>();
            serviceCollection.AddHostedService(x => x.GetRequiredService<KinectTrackingHostedService>());

            serviceCollection.AddSingleton<NewsCacheUpdateHostedService>();
            serviceCollection.AddHostedService(x => x.GetRequiredService<NewsCacheUpdateHostedService>());
        }
    }
}