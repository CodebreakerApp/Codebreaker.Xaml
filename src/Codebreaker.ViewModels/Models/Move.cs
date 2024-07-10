namespace Codebreaker.ViewModels.Models;

public class Move(ICollection<Field> guessPegs, ICollection<string>? keyPegs = null)
{
    /// <summary>
    /// The guess pegs from the user for this move.
    /// </summary>
    public ICollection<Field> GuessPegs { get; } = guessPegs;

    /// <summary>
    /// The result from the analyer for this move based on the associated game that contains the move.
    /// </summary>
    /// <remarks>
    /// Null if the move was not analyzed yet.
    /// </remarks>
    public ICollection<string>? KeyPegs { get; } = keyPegs;
}