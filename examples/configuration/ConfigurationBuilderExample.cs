using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace configuration
{
    public class ConfigurationBuilderExample
    {
        public static IConfiguration Run()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                .AddEnvironmentVariables()
                .Add(new JsonConfigurationSource()
                {
                    Path = "appsettings.override.json",
                    ReloadOnChange = true,
                    Optional = true
                })
                .Build();

            return configurationRoot;
        }
    }
}