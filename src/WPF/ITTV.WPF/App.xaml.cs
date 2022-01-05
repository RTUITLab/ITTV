//------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using ITTV.WPF.DataModel;
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Helpers;
using ITTV.WPF.Interface.Pages;
using ITTV.WPF.Network;
using ITTV.WPF.Services;
using ITTV.WPF.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kinect.Wpf.Controls;

namespace ITTV.WPF
{
    /// <summary>
    /// Interaction logic for App
    /// </summary>
    public partial class App : Application
    {
        public KinectRegion KinectRegion { get; set; }

        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            
            _serviceProvider = services.BuildServiceProvider();
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


            serviceCollection.AddSingleton<Settings>();
            serviceCollection.AddScoped<MireaTimeManager>();
            
            // serviceCollection.AddSingleton<CreateData>();
            // serviceCollection.AddSingleton<DataSource>();
            //
            // serviceCollection.AddSingleton<NewsFromSite>();
            // serviceCollection.AddSingleton<NewsUpdateThread>();
            // serviceCollection.AddSingleton<MireaDateTime>();
            //
            // serviceCollection.AddSingleton<BackgroundVideo>();
            // serviceCollection.AddSingleton<FrameContainer>();
            // serviceCollection.AddSingleton<Menu>();
            // serviceCollection.AddSingleton<TimeTable>();
            // serviceCollection.AddSingleton<EggVideo>();
            // serviceCollection.AddSingleton<Settings>();
        }
    }
}
