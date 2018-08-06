namespace GuessingGame
{
    public interface IGuessingGame
    {
        void RunGame();
        
        string GameId { get; }
    }
}