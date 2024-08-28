using Codebreaker.ViewModels.Models;

namespace CodeBreaker.WinUI.Views.Components.GamePage;

public sealed partial class Peg : UserControl
{
    public Peg()
    {
        InitializeComponent();
        contentControl.Content = new Field(ColorName, ShapeName);
    }

    // Dependency property for the color name
    public string? ColorName
    {
        get => (string?)GetValue(ColorNameProperty);
        set => SetValue(ColorNameProperty, value);
    }

    public static readonly DependencyProperty ColorNameProperty =
        DependencyProperty.Register(nameof(ColorName), typeof(string), typeof(Peg), new PropertyMetadata(null, OnDependencyPropertChanged));

    // Dependency property for the shape name
    public string? ShapeName
    {
        get => (string?)GetValue(ShapeNameProperty);
        set => SetValue(ShapeNameProperty, value);
    }

    public static readonly DependencyProperty ShapeNameProperty =
        DependencyProperty.Register(nameof(ShapeName), typeof(string), typeof(Peg), new PropertyMetadata(null, OnDependencyPropertChanged));

    private static void OnDependencyPropertChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (Peg)d;
        control.contentControl.Content = new Field(control.ColorName, control.ShapeName);
    }

    // Dependency property of configuring if the peg is draggable
    public bool IsDraggable
    {
        get => (bool)GetValue(IsDraggableProperty);
        set => SetValue(IsDraggableProperty, value);
    }

    public static readonly DependencyProperty IsDraggableProperty =
        DependencyProperty.Register(nameof(IsDraggable), typeof(bool), typeof(Peg), new PropertyMetadata(false));

    private void OnDragStarting(UIElement sender, DragStartingEventArgs args)
    {
        if (ColorName is not null)
            args.Data.Properties.Add("PegColor", ColorName);
        
        if (ShapeName is not null)
            args.Data.Properties.Add("PegShape", ShapeName);
    }
}