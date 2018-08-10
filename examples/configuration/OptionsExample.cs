using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace configuration
{
    public class SettingsOptions
    {
        public string Setting1 { get; set; }
        public string Setting2 { get; set; }
    }
    
    public class OptionsExample
    {
        public static void Run(IServiceCollection services, IConfiguration config)
        {
            services.Configure<SettingsOptions>(opt =>
            {
                config.GetSection("Program").Bind(opt);
            });
            
            // or

            services.Configure<SettingsOptions>(config.GetSection("Program"));
        }

        public static string GetSetting1(IOptions<SettingsOptions> settings)
        {
            // Gets value that was originally set at registration time
            return settings.Value.Setting1;
        }

        public static string GetCurrentSetting1(IOptionsMonitor<SettingsOptions> settings)
        {
            // Gets the value (aware of configuration changes)
            return settings.CurrentValue.Setting1;
        }
    }
}