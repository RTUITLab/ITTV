﻿using System;
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
            
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<NavigationStore>();
            serviceCollection.AddSingleton<NavigationService<MainWindowViewModel>>();

        }
    }
}