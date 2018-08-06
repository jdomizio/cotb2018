using System;
using Microsoft.Extensions.Logging;

namespace GuessingGame
{
    public class LocalGuessingGame : IGuessingGame
    {
        private readonly RandomNumber _targetNumber;
        private readonly IGuessFactory _guessFactory;
        private readonly ILogger<LocalGuessingGame> _logger;

        public LocalGuessingGame(
            RandomNumber targetNumber,
            IGuessFactory guessFactory,
            ILogger<LocalGuessingGame> logger
        )
        {
            _targetNumber = targetNumber;
            _guessFactory = guessFactory;
            _logger = logger;
            GameId = Guid.NewGuid().ToString();
        }
        
        public string GameId { get; private set; }

        public void RunGame()
        {
            _logger.LogInformation("Guessing game {gameId} starting", GameId);
            
            int? comparison = null;

            Console.WriteLine($"Guess a random number between {_targetNumber.Min} and {_targetNumber.Max}.");
            
            do
            {
                _logger.LogDebug("Getting a guess");
                var guess = _guessFactory.GetNextGuess();

                _logger.LogDebug("Comparing guess {guess} to target {target}", guess, _targetNumber.Value);
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

            _logger.LogInformation("Game {gameId} ending", GameId);
            Console.WriteLine("Game Over");
        }
    }
}