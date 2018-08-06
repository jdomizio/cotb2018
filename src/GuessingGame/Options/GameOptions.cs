namespace GuessingGame.Options
{
    public class GameOptions
    {
        public int MinNumber { get; set; } = 0;

        public int MaxNumber { get; set; } = 100;

        public string GuessFactory { get; set; } = @"console";

        public string GameType { get; set; } = @"local";
    }
}