using Microsoft.Extensions.Configuration;

namespace configuration
{
    public class IConfigurationExample
    {
        public static void Run(IConfiguration config)
        {
            var value = config["SomeKey"]; // gets "SomeValue"
            
            // or

            var other = config.GetValue<string>("Program:Setting1"); // using Configuration.Binder
        }
    }
}