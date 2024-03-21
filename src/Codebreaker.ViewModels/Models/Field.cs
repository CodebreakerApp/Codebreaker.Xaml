namespace Codebreaker.ViewModels.Models;

/// <summary>
/// Represents a field in a move.
/// </summary>
/// <param name="possibleColors">The possible colors for this field.</param>
public partial class Field(string[] possibleColors) : ObservableObject
{
    /// <summary>
    /// The selected color for this field.
    /// </summary>
    [ObservableProperty]
    private string? _color;

    /// <summary>
    /// The possible colors for this field.
    /// </summary>
    public string[] PossibleColors { get; init; } = possibleColors;
}