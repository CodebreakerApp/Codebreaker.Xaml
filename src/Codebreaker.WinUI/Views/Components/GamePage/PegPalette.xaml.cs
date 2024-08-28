namespace CodeBreaker.WinUI.Views.Components.GamePage;

public partial class PegPalette : UserControl
{
    public PegPalette()
    {
        InitializeComponent();
    }

    public IEnumerable<string> ColorNames
    {
        get => (IEnumerable<string>)GetValue(ColorNamesProperty);
        set
        {
            value ??= Array.Empty<string>();
            SetValue(ColorNamesProperty, value);
        }
    }

    public static readonly DependencyProperty ColorNamesProperty =
        DependencyProperty.Register(nameof(ColorNames), typeof(IEnumerable<string>), typeof(PegPalette), new PropertyMetadata(Array.Empty<string>()));

    public IEnumerable<string> ShapeNames
    {
        get => (IEnumerable<string>)GetValue(ShapeNamesProperty);
        set
        {
            value ??= Array.Empty<string>();
            SetValue(ShapeNamesProperty, value);
        }
    }

    public static readonly DependencyProperty ShapeNamesProperty =
        DependencyProperty.Register(nameof(ShapeNames), typeof(IEnumerable<string>), typeof(PegPalette), new PropertyMetadata(Array.Empty<string>()));

    private void PegDragStarting(UIElement sender, DragStartingEventArgs args)
    {
        var peg = (Peg)sender;

        if (peg.ColorName is not null)
            args.Data.Properties.Add("PegColor", peg.ColorName);
       
        if (peg.ShapeName is not null)
            args.Data.Properties.Add("PegShape", peg.ShapeName);
    }
}
