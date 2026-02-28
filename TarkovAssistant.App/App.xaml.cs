using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Configuration;
using System.IO;
using System.Windows;
using TarkovAssistant.App.ViewModels;
using TarkovAssistant.Data;
using TarkovAssistant.Services;

namespace TarkovAssistant.App
{
    public partial class App : Application
    {
        public static string SettingsFileName { get; private set; } = null!;
        public static ServiceProvider? Services { get; private set; }

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            App.SettingsFileName = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var services = new ServiceCollection();
            services.Configure<AppOptions>(config.GetSection("Settings"));
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(config.GetConnectionString("SQLiteConnection")));

            //var services = new ServiceCollection();
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(config.GetConnectionString("SqlServerConnection")));                                   

            services.AddSingleton<IAppService, AppService>();
            services.AddTransient<IMapService, MapService>();
            services.AddTransient<ILayerService, LayerService>();
            services.AddTransient<IQuestService, QuestService>();
            services.AddTransient<IMarkerService, MarkerService>();
            services.AddTransient<IFileMonitor, FileMonitor>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IMarkerStateService, MarkerStateService>();
            services.AddTransient<IWebApiService, WebApiService>();
            services.AddTransient<MainWindowViewModel>();

            ConfigureServices(services);
            Services = services.BuildServiceProvider();

            var mainViewModel = Services.GetRequiredService<MainWindowViewModel>();
            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            mainWindow.Show();
            await mainViewModel.InitializeAsync();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Typed HttpClient
            services.AddHttpClient<IWebApiService, WebApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7296/");
            });

            services.AddTransient<MainWindow>();
        }
    }
}
