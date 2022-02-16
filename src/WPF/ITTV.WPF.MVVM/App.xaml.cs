using System;
using System.Diagnostics;
using System.Windows;
using ITTV.WPF.Core.Helpers;
using ITTV.WPF.Core.Models;
using ITTV.WPF.Core.Services;
using ITTV.WPF.MVVM.Extensions;
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
        private Settings _settings => _serviceProvider.GetRequiredService<IOptions<Settings>>().Value;
        
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
            ConfigureReOpenApp();

            var navigationCommand = _serviceProvider.GetRequiredService<NavigationService<BackgroundVideoViewModel>>();
            navigationCommand.Navigate();
            
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Log.Logger.Error(e.Exception, "Not handled exception");
        }

        private void ReOpenApp()
        {
            var assemblyName = typeof(App).Assembly.GetName().Name;
            Process.Start($"{assemblyName}.exe");
        }

        private void ConfigureReOpenApp()
        {
            if (!_settings.IsAdminMode)
            {
                AppDomain.CurrentDomain.ProcessExit += (_,_) => ReOpenApp();
                AppDomain.CurrentDomain.UnhandledException += (_,_) => ReOpenApp();
            }
        }
    }
}