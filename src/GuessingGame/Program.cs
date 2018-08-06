using System;
using Microsoft.Extensions.DependencyInjection;

namespace GuessingGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddConfiguration(args);
            serviceCollection.AddGuessingGame();

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
