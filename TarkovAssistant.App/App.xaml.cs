using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TarkovAssistant.App.ViewModels;
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

            services.AddSingleton<IAppService, AppService>();
            services.AddTransient<IFileMonitor, FileMonitor>();
            services.AddTransient<IWebApiService, WebApiService>();
            services.AddTransient<MainWindowViewModel>();

            ConfigureServices(services, config);
            Services = services.BuildServiceProvider();

            var mainViewModel = Services.GetRequiredService<MainWindowViewModel>();
            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
            mainWindow.Show();
            await mainViewModel.InitializeAsync();
        }

        private void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            var appOptions = config.GetSection("Settings").Get<AppOptions>();

            // Typed HttpClient
            services.AddHttpClient<IWebApiService, WebApiService>(client =>
            {
                client.BaseAddress = new Uri(appOptions?.Server ?? "");
            });

            services.AddTransient<MainWindow>();
        }
    }
}
