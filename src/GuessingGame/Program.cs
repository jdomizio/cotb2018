using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace GuessingGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            
            var config = serviceCollection.AddConfiguration(args);
            serviceCollection.AddGuessingGame();
            serviceCollection.AddLogging(builder =>
            {
                builder.AddConfiguration(config.GetSection("Logging"));
                builder.AddConsole();
            });

            var provider = serviceCollection.BuildServiceProvider();

            bool keepGoing = true;

            while (keepGoing)
            {
                var game = provider.GetRequiredService<IGuessingGame>();

                game.RunGame();
                
                Console.WriteLine("Another? (Y/N)");
                var answer = Console.ReadLine();
                keepGoing = answer?.ToLower() == "y";
            }            
        }
    }
}
