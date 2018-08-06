using System;
using GuessingGame;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class GuessingGameServiceCollectionExtensions
    {
        public static void AddGuessingGame(this IServiceCollection services,
            Action<GuessingGameOptions> optionsFactory = null)
        {
            var guessingGameOptions = new GuessingGameOptions();

            optionsFactory?.Invoke(guessingGameOptions);

            // Add a random number - everyone gets their own copy
            services.TryAddTransient<RandomNumber>();
            
            // Add a guessing game - shared instance per scope
            services.TryAddScoped<LocalGuessingGame>();
            
            // Add a singleton random number generator, everyone gets the same copy
            services.TryAddSingleton((sp => new Random()));

            // Resolve IGuessFactory with ConsoleGuessFactory
            services.TryAddTransient<IGuessFactory, ConsoleGuessFactory>();
        }
    }
}