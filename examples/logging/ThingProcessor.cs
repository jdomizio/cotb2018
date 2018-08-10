using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace logging
{
    public class ThingProcessor
    {
        private readonly ILogger _logger;

        public ThingProcessor(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(ThingProcessor));
        }

        public void ProcessThing()
        {
            var randomNumber = new Random().Next(1, 1000) * 10;

            using (_logger.BeginScope("thing level {level}", randomNumber))
            {
                _logger.LogDebug("A thing is being processed");

                if (randomNumber > 9000)
                {
                    _logger.LogError("The thing is over 9000!");
                }
            }
        }
    }
}