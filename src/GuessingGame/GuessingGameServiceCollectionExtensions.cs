using System;
using System.Collections.Generic;
using System.IO;
using GuessingGame;
using GuessingGame.GuessFactories;
using GuessingGame.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class GuessingGameServiceCollectionExtensions
    {
        public static void AddGuessingGame(this IServiceCollection services)
        {
            // Add a random number - everyone gets their own copy
            services.TryAddTransient<RandomNumber, ConfigurableRandomNumber>();
            
            // Add a guessing game - shared instance per scope
            services.TryAddTransient<LocalGuessingGame>();
            services.TryAddTransient<DistributedGuessingGame>();
            services.TryAddTransient<IGuessingGame>(provider =>
            {
                var options = provider.GetRequiredService<IOptionsMonitor<GameOptions>>();
                switch (options.CurrentValue?.GameType)
                {
                    case "local":
                        return provider.GetRequiredService<LocalGuessingGame>();
                    case "distributed":
                        return provider.GetRequiredService<DistributedGuessingGame>();
                    default:
                        return provider.GetRequiredService<LocalGuessingGame>();
                }
            });
            
            // Add a singleton random number generator, everyone gets the same copy
            services.TryAddSingleton((sp => new Random()));

            // Resolve IGuessFactory with ConsoleGuessFactory
            services.TryAddTransient<ConsoleGuessFactory>();
            services.TryAddTransient<RandomGuessFactory>();
            services.TryAddTransient<IGuessFactory>(provider =>
            {
                var options = provider.GetRequiredService<IOptionsMonitor<GameOptions>>();
                switch (options.CurrentValue?.GuessFactory)
                {
                    case "console":
                        return provider.GetRequiredService<ConsoleGuessFactory>();
                    case "random":
                        return provider.GetRequiredService<RandomGuessFactory>();
                    default:
                        return provider.GetRequiredService<ConsoleGuessFactory>();
                }                
            });
            
        }

        public static IConfigurationRoot AddConfiguration(this IServiceCollection services, string[] args)
        {
            var configurationRoot = new ConfigurationBuilder()
                .AddInMemoryCollection(new []
                {
                    new KeyValuePair<string, string>("game:minNumber", "0"),
                    new KeyValuePair<string, string>("game:maxNumber", "100"), 
                })
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            
            services.TryAddSingleton<IConfiguration>(configurationRoot);
            services.Configure<GameOptions>(configurationRoot.GetSection("game"));
            services.Configure<HubOptions>(configurationRoot.GetSection("hub"));
            
            return configurationRoot;
        }
    }
}