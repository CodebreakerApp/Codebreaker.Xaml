namespace CodeBreaker.WinUI.Views.Components.GamePage.Shapes;

public sealed partial class Rectangle : ShapeBase
{
    public Rectangle()
    {
        InitializeComponent();
    }

    private int ShapeHeight => Size;

    private int ShapeWidth => (int)(Size * 1.5);
}
