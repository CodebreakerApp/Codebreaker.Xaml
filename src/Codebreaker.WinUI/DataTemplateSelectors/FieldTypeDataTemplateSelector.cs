using Codebreaker.ViewModels.Models;

namespace CodeBreaker.WinUI.DataTemplateSelectors;

public class FieldDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate? CircleTemplate { get; set; }

    public DataTemplate? RectangleTemplate { get; set; }

    public DataTemplate? TriangleTemplate { get; set; }

    public DataTemplate? SquareTemplate { get; set; }

    public DataTemplate? StarTemplate { get; set; }

    public DataTemplate? DefaultTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        if (item is not Field field)
            throw new ArgumentException($"The provided value for the DataTemplateSelector must be a {nameof(Field)}");

        return field switch
        {
            //{ Color: null or { Length: 0 } } => throw new ArgumentException("The color of the field is not set."),
            // ColorField:
            { Color: { Length: > 0 }, Shape: null or { Length: 0 } } => CircleTemplate ?? throw new InvalidOperationException($"{nameof(CircleTemplate)} is not set and therefore null"),
            // ColorShapeField:
            { Shape: "Circle" } => CircleTemplate ?? throw new InvalidOperationException($"{nameof(CircleTemplate)} is not set and therefore null"),
            { Shape: "Rectangle" } => RectangleTemplate ?? throw new InvalidOperationException($"{nameof(RectangleTemplate)} is not set and therefore null"),
            { Shape: "Triangle" } => TriangleTemplate ?? throw new InvalidOperationException($"{nameof(TriangleTemplate)} is not set and therefore null"),
            { Shape: "Square" } => SquareTemplate ?? throw new InvalidOperationException($"{nameof(SquareTemplate)} is not set and therefore null"),
            { Shape: "Star" } => StarTemplate ?? throw new InvalidOperationException($"{nameof(StarTemplate)} is not set and therefore null"),
            _ => DefaultTemplate ?? throw new InvalidOperationException($"Invalid field and {nameof(DefaultTemplate)} is not set and therefore null")
        };
    }

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container) =>
        SelectTemplateCore(item);
}