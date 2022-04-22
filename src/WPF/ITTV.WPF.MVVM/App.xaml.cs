using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using ITTV.WPF.Abstractions.Attributes;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Models;
using ITTV.WPF.Core.Services;
using ITTV.WPF.MVVM.BackgroundServices.Cache;
using ITTV.WPF.MVVM.BackgroundServices.Tracking;
using ITTV.WPF.MVVM.Commands.BackgroundVideos;
using ITTV.WPF.MVVM.Extensions;
using ITTV.WPF.MVVM.Utilities.Tracking;
using ITTV.WPF.MVVM.ViewModels;
using ITTV.WPF.MVVM.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace ITTV.WPF.MVVM
{
    public partial class App
    {
        private readonly IServiceProvider _serviceProvider;
        public App()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(PathHelper.FileConfigurationPath)
                .AddJsonFile(PathHelper.FileLoggingConfigurationPath, optional: true)
                .AddUserSecrets<App>()
                .Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.Configure<Settings>(configuration.GetSection(nameof(Settings)));

            var seqLoggerSettings = configuration.GetSection(nameof(ApiSeqLoggerSettings))
                .Get<ApiSeqLoggerSettings>();

            var logger = new LoggerConfiguration()
                .Enrich.WithProperty("APP","ITTV")
                .WriteTo.File(PathHelper.FileLogsPath)
                .WriteTo.Console();

            if (seqLoggerSettings != null && seqLoggerSettings.IsValid())
            {
                logger.WriteTo.Seq(seqLoggerSettings.Uri, apiKey: seqLoggerSettings.ApiKey);

                var version =
                    Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyRTUITLabVersionAttribute>()?.Version;
                if (!string.IsNullOrWhiteSpace(version))
                {
                    logger.Enrich.WithProperty("VERSION", version);
                }
            }

            Log.Logger = logger.CreateLogger();

            serviceCollection.AddLogging(l => l.AddSerilog());

            serviceCollection.AddBusinessLogicLayerServicesExtensions();
            
            _serviceProvider = serviceCollection.BuildServiceProvider();

            ConfigureServices();
        }

        private void ConfigureServices()
        {
            var kinectHostedService = _serviceProvider.GetRequiredService<KinectTrackingHostedService>();
            kinectHostedService.StartAsync(default);
            
            var newsCacheUpdateHostedService = _serviceProvider.GetRequiredService<NewsCacheUpdateHostedService>();
            newsCacheUpdateHostedService.StartAsync(default);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                Log.Logger.Information("Starting application");

                ConfigureReOpenApp();

                var navigationCommand = _serviceProvider.GetRequiredService<NavigationService<BackgroundVideoViewModel>>();
                navigationCommand.Navigate();
            
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
                
                var kinectUtility = _serviceProvider.GetRequiredService<KinectTrackingUtility>();
                kinectUtility.SetKinectRegion(mainWindow.KinectRegion);
                
                Log.Logger.Information("Application has been started");
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex, "Failed to launch the application");
            }
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Logger.Error(e.Exception, "Not handled exception");
            var navigationStore = _serviceProvider.GetRequiredService<NavigateBackgroundVideoAndClearHistoryCommand>();
            navigationStore.Execute(null);
            
            e.Handled = true;
        }

        private void ReOpenApp()
        {
            var exeFilePath = Assembly.GetExecutingAssembly().Location;
            Process.Start(exeFilePath);
        }

        private void ConfigureReOpenApp()
        {
            var settings = _serviceProvider.GetRequiredService<IOptions<Settings>>().Value;
            if (!settings.IsAdminMode)
            {
                AppDomain.CurrentDomain.ProcessExit += (_,_) => ReOpenApp();
                AppDomain.CurrentDomain.UnhandledException += (_,_) => ReOpenApp();
            }
        }
    }
}