namespace Codebreaker.ViewModels.Messages;

public record class MakeMoveMessage(Move Move)
{
    /// <summary>
    /// Specifies if the move was already made.
    /// </summary>
    public bool IsSet => Move.KeyPegs is not null;
}