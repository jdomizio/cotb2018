using System;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace logging
{
    public class ThingService
    {
        private readonly ThingDoer _doer;
        private readonly ThingProcessor _processor;
        private readonly ILogger _logger;

        public ThingService(ThingDoer doer, ThingProcessor processor, ILogger<ThingService> logger)
        {
            _doer = doer;
            _processor = processor;
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogInformation("Running all the things.");
            
            for (var i = 0; i < 100; ++i)
            {
                _logger.LogTrace("Processing {current} out of {total}", i, 100);
                _doer.DoThing();
                _processor.ProcessThing();

                Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            }
            _logger.LogInformation("Everything is done. bye.");
        }
    }
}