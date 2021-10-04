using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace ApiDPSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Serilog
            var config = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("appsettings.json")
                         .Build();

            Log.Logger = new LoggerConfiguration()
                         .Enrich.WithProperty("ApplicationName", typeof(Program).Namespace)
                         .ReadFrom.Configuration(config)
                         .CreateLogger();
            #endregion

            Log.Information("Starting up");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}