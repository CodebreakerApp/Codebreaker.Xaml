namespace Codebreaker.ViewModels.Models;

public partial class Move(ICollection<string> guessPegs, ICollection<string>? keyPegs = null) : ObservableObject
{
    /// <summary>
    /// The guess pegs from the user for this move.
    /// </summary>
    public ICollection<string> GuessPegs { get; } = guessPegs;

    /// <summary>
    /// The result from the analyer for this move based on the associated game that contains the move.
    /// </summary>
    /// <remarks>
    /// Null if the move was not analyzed yet.
    /// </remarks>
    public ICollection<string>? KeyPegs { get; } = keyPegs;
}
