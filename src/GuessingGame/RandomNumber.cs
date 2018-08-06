using System;

namespace GuessingGame
{
    public class RandomNumber
    {
        public RandomNumber(Random rng)
        {
            Value = rng.Next(0, 100);
        }

        public readonly int Value;
    }
}