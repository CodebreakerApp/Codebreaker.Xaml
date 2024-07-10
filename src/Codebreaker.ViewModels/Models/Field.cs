namespace Codebreaker.ViewModels.Models;

/// <summary>
/// Represents a field in a move.
/// </summary>
public partial class Field : ObservableObject
{
    public Field()
    {
    }

    public Field(string? color, string? shape)
    {
        Color = color;
        Shape = shape;
    }

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
}