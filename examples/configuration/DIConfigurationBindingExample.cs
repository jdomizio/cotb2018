using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace configuration
{
    public class DIConfigurationBindingExample
    {
        public static void Run(IServiceCollection services, IConfiguration config)
        {
            services.AddAntiforgery(options =>
            {
                // This allows configuration to drive DI options easily
                config.GetSection("AntiForgery").Bind(options); 
            });
        }
    }
}