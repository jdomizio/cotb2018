using System;

namespace GuessingGame.GuessFactories
{
    public class ConsoleGuessFactory : IGuessFactory
    {
        public int GetNextGuess()
        {
            Console.WriteLine("What is your guess?");

            int guess;
            while (!int.TryParse(Console.ReadLine(), out guess))
            {
                Console.WriteLine("Must enter a number...");
            }

            return guess;
        }
    }
}