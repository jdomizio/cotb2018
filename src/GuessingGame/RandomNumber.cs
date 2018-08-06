using System;
using GuessingGame.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace GuessingGame
{
    public class RandomNumber
    {
        public RandomNumber(Random rng, IOptionsMonitor<GameOptions> configuration)
        {
            Min = configuration.CurrentValue.MinNumber;
            Max = configuration.CurrentValue.MaxNumber;
            Value = rng.Next(Min, Max);
        }

        public readonly int Value;

        public readonly int Min;

        public readonly int Max;
    }
}