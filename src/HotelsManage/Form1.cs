using HotelsManage.Database;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor;
using MudBlazor.Services;
using Serilog;

namespace HotelsManage
{
    public partial class Form1 : Form
    {
        public Form1(ILogger<Form1> logger)
        {
            InitializeComponent();

            base.Text = AppExtension.AppName;

            logger.LogInformation("open main");
            
            var services = new ServiceCollection();
#if DEBUG
            services.AddBlazorWebViewDeveloperTools();
#endif
            services.AddWindowsFormsBlazorWebView();
            services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true; ;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });
            services.AddScoped(sp => new HttpClient());
            services.AddLogging(builder => builder.AddSerilog());
            
            blazorWebView1.HostPage = "wwwroot\\index.html";
            blazorWebView1.Services = services.BuildServiceProvider();



            blazorWebView1.RootComponents.Add<HeadOutlet>("head::after");
            blazorWebView1.RootComponents.Add<App>("#app");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DbAccess._database.Value.Dispose();
            Environment.Exit(0);
        }
    }
}