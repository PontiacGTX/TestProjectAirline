using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProjectAirline
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var _config = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json")
                    .Build();
            var fileName = $@"logfile_{DateTime.Now.ToString("yyyy/MM/dd")}.log";
            while (fileName.Contains('/'))
            {
                fileName = fileName.Replace('/', '_');
            }

            Log.Logger = new LoggerConfiguration()
                       .ReadFrom.Configuration(_config).WriteTo.File(fileName,
             fileSizeLimitBytes: 100 * 1024 * 1024,
             flushToDiskInterval: TimeSpan.FromMilliseconds(300),
             rollOnFileSizeLimit: true
           ).CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.SetMinimumLevel(LogLevel.Trace);
                    logging.AddEventSourceLogger();
                });
    }
}
