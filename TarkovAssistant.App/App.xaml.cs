using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarkovAssistant.App.ViewModels;
using TarkovAssistant.Data;
using TarkovAssistant.Services;

namespace TarkovAssistant.App
{
    public partial class App : Application
    {
        public static ServiceProvider? Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var services = new ServiceCollection();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(config.GetConnectionString("SQLiteConnection")));

            services.AddTransient<IMapService, MapService>();
            services.AddTransient<ILayerService, LayerService>();
            services.AddTransient<MainWindowViewModel>();

            Services = services.BuildServiceProvider();

            var mainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainWindowViewModel>()
            };
            mainWindow.Show();
        }
    }
}
