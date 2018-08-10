using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace configuration
{
    public class ChangeTokenExample
    {
        private string _someValue = null;

        public ChangeTokenExample(IConfiguration configuration)
        {
            // Set it the first time
            _someValue = configuration.GetValue<string>("SomeKey");
            
            ChangeToken.OnChange(configuration.GetReloadToken, config =>
            {
                // Be notified any time it changes
                _someValue = config.GetValue<string>("SomeKey");
            }, configuration);
        }        
    }
}