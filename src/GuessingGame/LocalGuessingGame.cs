using System;

namespace GuessingGame
{
    public class LocalGuessingGame : IGuessingGame
    {
        private readonly RandomNumber _targetNumber;
        private readonly IGuessFactory _guessFactory;

        public LocalGuessingGame(
            RandomNumber targetNumber,
            IGuessFactory guessFactory
        )
        {
            _targetNumber = targetNumber;
            _guessFactory = guessFactory;
        }

        public void RunGame()
        {
            int? comparison = null;

            Console.WriteLine($"Guess a random number between {_targetNumber.Min} and {_targetNumber.Max}.");
            
            do
            {
                var guess = _guessFactory.GetNextGuess();

                comparison = guess.CompareTo(_targetNumber.Value);
                
                if (comparison == 0)
                {
                    Console.WriteLine($"You guessed: {guess}, You win!");
                }
                else
                {
                    Console.WriteLine($"You guessed: {guess}, number is {(comparison > 0 ? "lower" : "higher")}");
                }
            } while (comparison != 0);
            
            Console.WriteLine("Game Over");
        }
    }
}