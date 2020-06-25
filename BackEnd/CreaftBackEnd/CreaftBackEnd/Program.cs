using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace CraftBackEnd
{
    public class Program
    {
        public static void Main(string[] args) {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logger => {
                    // clear default logging providers
                    logger.ClearProviders();

                    // add built-in providers manually, as needed 
                    logger.AddConsole();
                    logger.AddDebug();
                    //logger.AddEventLog();
                    logger.AddEventSourceLogger();
                    //logger.AddTraceSource(sourceSwitchName);
                }).UseNLog();
                //.UseNLog();
    }
}
