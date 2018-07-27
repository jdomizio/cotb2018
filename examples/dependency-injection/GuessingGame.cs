using System;

namespace dependency_injection
{
    public class GuessingGame
    {
        private readonly RandomNumber _targetNumber;
        private readonly IGuessFactory _guessFactory;

        public GuessingGame(
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

            do
            {
                var guess = _guessFactory.GetNextGuess();

                comparison = guess.CompareTo(_targetNumber.Value);

                if (comparison == 0)
                {
                    Console.WriteLine("You win!");
                }
                else
                {
                    Console.WriteLine(comparison > 0 ? "Lower" : "Higher");
                }
            } while (comparison != 0);
            
            Console.WriteLine("Game Over");
        }
    }
}