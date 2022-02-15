using System;
using System.Windows;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Models;
using ITTV.WPF.Core.Providers.MireaApi;
using ITTV.WPF.Core.Services;
using ITTV.WPF.Core.Services.ApiClient;
using ITTV.WPF.Core.Stores;
using ITTV.WPF.MVVM.Commands;
using ITTV.WPF.MVVM.Commands.BackgroundVideos;
using ITTV.WPF.MVVM.Extensions;
using ITTV.WPF.MVVM.ViewModels;
using ITTV.WPF.MVVM.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;

namespace ITTV.WPF.MVVM
{
    public partial class App
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            const string configurationFile = "configuration.json";
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(configurationFile)
                .Build();
            
            
            var serviceCollection = new ServiceCollection();
            serviceCollection.Configure<Settings>(configuration.GetSection(nameof(Settings)));

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("APP","ITTV")
                .WriteTo.File(PathHelper.FileLogsPath)
                .CreateLogger();

            serviceCollection.AddLogging(l => l.AddSerilog());
            
            serviceCollection.AddBusinessLogicLayerServicesExtensions();
            
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var navigationCommand = _serviceProvider.GetRequiredService<NavigationService<BackgroundVideoViewModel>>();
            navigationCommand.Navigate();
            
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Logger.Error(e.Exception, "Not handled exception");
        }
    }
}