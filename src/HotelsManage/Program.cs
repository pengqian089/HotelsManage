using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace HotelsManage
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            ApplicationConfiguration.Initialize();

            Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("logs", ".log"), rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                .CreateLogger();
            
            var host = CreateHostBuilder().Build();
            _serviceProvider = host.Services;
            Log.Information("application starting");
            Application.Run(_serviceProvider.GetRequiredService<Form1>());
        }

        private static IServiceProvider? _serviceProvider;

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .UseSerilog()
                .ConfigureServices((_, services) =>
                {
                    services.AddTransient<Form1>();
                });
        }
    }
}