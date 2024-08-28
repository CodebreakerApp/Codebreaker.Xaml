namespace CodeBreaker.WinUI.Views.Components.GamePage.Shapes;

public partial class ShapeBase : UserControl
{
    public ShapeBase()
    {
        InitializeComponent();
    }

    // Dependency property for color
    public string ColorName
    {
        get => (string)GetValue(ColorNameProperty);
        set => SetValue(ColorNameProperty, value);
    }

    public static readonly DependencyProperty ColorNameProperty =
        DependencyProperty.Register(nameof(ColorName), typeof(string), typeof(ShapeBase), new PropertyMetadata(string.Empty));

    // Dependency property for size
    public int Size
    {
        get => (int)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public static readonly DependencyProperty SizeProperty =
        DependencyProperty.Register(nameof(Size), typeof(int), typeof(ShapeBase), new PropertyMetadata(60));
}
