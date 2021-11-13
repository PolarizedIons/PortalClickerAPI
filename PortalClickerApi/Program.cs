using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace PortalClickerApi
{
    public class Program
    {
        public static int Main(string[] args)
        {

            try
            {
                SetupLogging();
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Something went wrong");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        private static void SetupLogging()
        {
            var loggerBuilder = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console();

            var seqUrl = Environment.GetEnvironmentVariable("SEQ__Url");
            var seqApiKey = Environment.GetEnvironmentVariable("SEQ__Key");
            if (!string.IsNullOrEmpty(seqUrl))
            {
                loggerBuilder.WriteTo.Seq(seqUrl, apiKey: seqApiKey);
            }

            Log.Logger = loggerBuilder.CreateLogger();
        }
    }
}
