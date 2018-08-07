using System;
using System.Threading;
using System.Threading.Tasks;
using GuessingGame.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.SignalR.Client;

namespace GuessingGame
{
    public class DistributedGuessingGame : IGuessingGame
    {
        private readonly ILogger _logger;
        private readonly RandomNumber _targetNumber;
        private readonly IOptions<HubOptions> _hubOptions;
        private readonly IOptionsMonitor<GameOptions> _gameOptions;
        private TaskCompletionSource<string> _gameComplete;
        private GameInfo _currentGame;
        
        public DistributedGuessingGame(
            ILogger<DistributedGuessingGame> logger,
            RandomNumber targetNumber,
            IOptionsMonitor<GameOptions> gameOptions,
            IOptions<HubOptions> hubOptions
        )
        {
            _logger = logger;
            _targetNumber = targetNumber;
            _gameOptions = gameOptions;
            _hubOptions = hubOptions;
            GameId = Guid.NewGuid().ToString();
        }
        
        public string GameId { get; }

        public async void RunGame()
        {
            RunGameAsync().Wait();
        }

        private async Task RunGameAsync()
        {
            _logger.LogInformation("Starting game {gameId}", GameId);
            _currentGame = new GameInfo
            {
                Min = _gameOptions.CurrentValue.MinNumber,
                Max = _gameOptions.CurrentValue.MaxNumber,
                GameId = GameId
            };
            _gameComplete = new TaskCompletionSource<string>();
            
            _logger.LogDebug("Creating new SignalR client to url {url}", _hubOptions.Value.Url);
            var client = new HubConnectionBuilder()
                .WithUrl(_hubOptions.Value.Url)
                .Build();
            
            client.On<string>("GameJoined", name =>
            {
                _logger.LogInformation("{name} has joined the game", name);  
            });

            client.On<string, string>("GuessMade", async (name, guess) =>
            {
                _logger.LogInformation("{name} has made guess {guess}", name, guess);
                if (!int.TryParse(guess, out var g))
                {
                    await client.InvokeAsync("GuessMadeResponse", name, "Guess must be a number");
                    return;
                }
                
                var comparison = g.CompareTo(_targetNumber.Value);

                if (comparison == 0)
                {
                    _logger.LogInformation("We have a winner! {name}", name);
                    _currentGame.Winner = name;
                    _currentGame.Finished = true;
                    _logger.LogDebug("Sending EndGame({gameId})", GameId);
                    await client.InvokeAsync("EndGame", _currentGame, _hubOptions.Value.Password);
                    _gameComplete.TrySetResult(name);
                }
                else
                {
                    var message = $"Your guess is too {(comparison > 0 ? "high" : "low")}";
                    await client.InvokeAsync("GuessMadeResponse", name, message);
                }
            });

            try
            {
                _logger.LogDebug("Starting Client");
                await client.StartAsync();

                _logger.LogDebug("Sending CreateGame({gameId})", GameId);
                await client.InvokeAsync("CreateGame", _currentGame, _hubOptions.Value.Password);
                
                _logger.LogDebug("Waiting for signal to start game");
                Console.WriteLine("Press any key to start the game...");
                Console.ReadKey();

                _logger.LogDebug("Sending StartGame({gameId})", GameId);
                await client.InvokeAsync("StartGame", GameId, _hubOptions.Value.Password);
                
                await _gameComplete.Task;
            }
            catch (Exception ex)
            {               
                _logger.LogError(ex, "Caught an exception while trying to run the game.  Oops.");
                await client.StopAsync();
                _gameComplete.TrySetCanceled();
            }
        }
        
        public class GameInfo
        {
            public string GameId { get; set; }
            public bool Started { get; set; }
            public bool Finished { get; set; }
            public int Min { get; set; }
            public int Max { get; set; }
            public string Winner { get; set; }
        }
    }
}