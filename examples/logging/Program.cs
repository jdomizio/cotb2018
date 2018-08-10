using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace logging
{
    class Program
    {
        static void Main(string[] args)
        {
            GetServiceProvider().GetRequiredService<ThingService>().Run();            
        }

        static IServiceProvider GetServiceProvider()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                .Build();
            
            var services = new ServiceCollection();
            services.AddLogging(logging => 
                logging.AddConfiguration(config.GetSection("Logging"))
                    .AddConsole());

            services.AddTransient<ThingDoer>();
            services.AddTransient<ThingProcessor>();
            services.AddTransient<ThingService>();
            
            return services.BuildServiceProvider();
        }
    }
}
