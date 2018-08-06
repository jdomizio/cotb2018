using System;
using GuessingGame.Options;
using Microsoft.Extensions.Options;

namespace GuessingGame.GuessFactories
{
    public class RandomGuessFactory : IGuessFactory
    {
        private readonly IOptionsMonitor<GameOptions> _options;
        private readonly Random _rng;
        
        public RandomGuessFactory(IOptionsMonitor<GameOptions> options, Random rng)
        {
            _options = options;
            _rng = rng;
        }
        
        public int GetNextGuess()
        {
            return _rng.Next(_options.CurrentValue.MinNumber, _options.CurrentValue.MaxNumber);
        }
    }
}