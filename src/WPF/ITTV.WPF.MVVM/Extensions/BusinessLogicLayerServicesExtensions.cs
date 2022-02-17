using ITTV.WPF.Core.Providers.MireaApi;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Services.ApiClient;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.BackgroundServices.Tracking;
using ITTV.WPF.MVVM.Commands;
using ITTV.WPF.MVVM.Commands.BackgroundVideos;
using ITTV.WPF.MVVM.Utilities.Tracking;
using ITTV.WPF.MVVM.ViewModels;
using ITTV.WPF.MVVM.ViewModels.Games;
using ITTV.WPF.MVVM.ViewModels.News;
using ITTV.WPF.MVVM.ViewModels.Schedule;
using ITTV.WPF.MVVM.ViewModels.Videos;
using ITTV.WPF.MVVM.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kinect.Wpf.Controls;

namespace ITTV.WPF.MVVM.Extensions
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
            serviceCollection.AddSingleton<VideosManager>();

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
            var kinectRegion = new KinectRegion();

            serviceCollection.AddSingleton<KinectTrackingUtility>();

            serviceCollection.AddSingleton<KinectTrackingHostedService>();
            serviceCollection.AddHostedService(x => x.GetRequiredService<KinectTrackingHostedService>());
        }
    }
}