using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;


namespace Subsription.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfigurationBuilder config = new ConfigurationBuilder();
            var configBuild = config
            .AddJsonFile("appsettings.json", optional: true)
            .AddCommandLine(args)
            .Build();

            var logConnStr = configBuild.GetConnectionString("LogConnectionString");
            string logPath = configBuild.GetSection("LogFolder").Value;
            Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Debug()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
             .MinimumLevel.Verbose()
             .Enrich.FromLogContext()
             .WriteTo.File(logPath, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day)

             .CreateLogger();

            Log.Information("Program is starting...");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
