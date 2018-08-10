using System;
using Microsoft.Extensions.Logging;

namespace logging
{
    public class ThingDoer
    {
        private readonly ILogger _logger;

        public ThingDoer(ILogger<ThingDoer> logger)
        {
            _logger = logger;
        }

        public void DoThing()
        {
            var guid = Guid.NewGuid();
            
            _logger.LogDebug("Doing thing {guid}", guid);
        }
    }
}