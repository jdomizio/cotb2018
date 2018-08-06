using System;
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
            
            _logger.LogDebug("Creating new SignalR client to url {url}", _hubOptions.Value.Url);
            var client = new HubConnectionBuilder()
                .WithUrl(_hubOptions.Value.Url)
                .Build();
            
            client.On<string>("GameJoined", name =>
            {
                _logger.LogInformation("{name} has joined the game", name);  
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

                _logger.LogDebug("Sending EndGame({gameId})", GameId);
                await client.InvokeAsync("EndGame", GameId, _hubOptions.Value.Password);

                _logger.LogDebug("Stopping Client");
                await client.StopAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Caught an exception while trying to run the game.  Oops.");
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