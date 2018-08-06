using System;
using Microsoft.Extensions.DependencyInjection;

namespace GuessingGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddGuessingGame();

            var provider = serviceCollection.BuildServiceProvider();

            var game = provider.GetRequiredService<LocalGuessingGame>();
            
            game.RunGame();
        }
    }
}
