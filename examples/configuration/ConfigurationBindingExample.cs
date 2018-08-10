using Microsoft.Extensions.Configuration;

namespace configuration
{
    public class ConfigurationBindingExample
    {
        public class SomePoco
        {
            public bool Setting1 { get; set; }

            public bool Setting2 { get; set; }
        }
        
        public static SomePoco Run(IConfiguration config)
        {
            var result = new SomePoco();
            
            config.GetSection("Program").Bind(result);

            return result; // result now has the values from our configuration.
        }
    }
}