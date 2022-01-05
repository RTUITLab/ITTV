using System.Windows;
using ITTV.WPF.DataModel.Models;
using ITTV.WPF.Services;
using ITTV.WPF.Services.Kinect;
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
            ConfigureKinectServices(services);
            
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
            serviceCollection.AddSingleton<MainWindow>();


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

        private void ConfigureKinectServices(IServiceCollection serviceCollection)
        {
            var kinectRegion = new KinectRegion();
            
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();

            KinectRegion.SetKinectRegion(mainWindow, kinectRegion);

            serviceCollection.AddSingleton<KinectRegion>(kinectRegion);
            serviceCollection.AddSingleton<HandOverManager>();
        }
    }
}
