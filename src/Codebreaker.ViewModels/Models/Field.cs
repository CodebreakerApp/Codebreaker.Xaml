namespace Codebreaker.ViewModels.Models;

/// <summary>
/// Represents a field in a move.
/// </summary>
public partial class Field : ObservableObject
{
    /// <summary>
    /// The selected color for this field.
    /// </summary>
    [ObservableProperty]
    private string? _color;

    /// <summary>
    /// The selected shape for this field.
    /// </summary>
    [ObservableProperty]
    private string? _shape;

    /// <summary>
    /// The possible colors for this field.
    /// </summary>
    public string[]? PossibleColors { get; set; }

    /// <summary>
    /// The possible shapes for this field.
    /// </summary>
    public string[]? PossibleShapes { get; set; }
}