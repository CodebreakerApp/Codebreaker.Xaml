namespace Codebreaker.ViewModels.Messages;

public record class GameEndedMessage(Game Game)
{
    public bool IsVictory => Game.IsVictory;
}