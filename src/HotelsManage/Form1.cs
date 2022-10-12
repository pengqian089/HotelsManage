using HotelsManage.Database;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            services.AddWindowsFormsBlazorWebView();
            services.AddMudServices();
            services.AddScoped(sp => new HttpClient());
            services.AddLogging(builder => builder.AddSerilog());
            
            blazorWebView1.HostPage = "wwwroot\\index.html";
            blazorWebView1.Services = services.BuildServiceProvider();
            

            blazorWebView1.RootComponents.Add<HeadOutlet>("head::after");
            blazorWebView1.RootComponents.Add<App>("#app");

            FormClosed += (_, _) =>
            {
                DbAccess._database.Value.Dispose();
            };
        }
    }
}