using System;
using GuessingGame.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace GuessingGame
{
    public class ConfigurableRandomNumber : RandomNumber
    {
        public ConfigurableRandomNumber(Random rng, IOptionsMonitor<GameOptions> configuration)
            : base(rng)
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