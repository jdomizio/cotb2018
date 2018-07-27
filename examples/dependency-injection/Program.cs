using System;
using Microsoft.Extensions.DependencyInjection;

namespace dependency_injection
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            
            // Add a random number - everyone gets their own copy
            serviceCollection.AddTransient<RandomNumber>();
            
            // Add a guessing game - shared instance per scope
            serviceCollection.AddScoped<GuessingGame>();
            
            // Add a singleton random number generator, everyone gets the same copy
            serviceCollection.AddSingleton((sp => new Random()));

            // Resolve IGuessFactory with ConsoleGuessFactory
            serviceCollection.AddTransient<IGuessFactory, ConsoleGuessFactory>();
            
            // Now that we have configured our service collection, let's build it
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Use the service provider (service locator) to resolve a guessing game.
            var game = serviceProvider.GetRequiredService<GuessingGame>();

            game.RunGame();           
        }
    }
}
