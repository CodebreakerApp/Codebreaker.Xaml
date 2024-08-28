using Codebreaker.ViewModels;
using Codebreaker.ViewModels.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Media.Animation;

namespace CodeBreaker.WinUI.Views.Components.GamePage;

internal sealed partial class PegSelectionComponent : UserControl, IRecipient<MakeMoveMessage>
{
    public PegSelectionComponent()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register(this);
        WeakReferenceMessenger.Default.UnregisterAllOnUnloaded(this);
    }

    public GamePageViewModel ViewModel
    {
        get => (GamePageViewModel)GetValue(ViewModelProperty);
        set
        {
            SetValue(ViewModelProperty, value);
            DataContext = ViewModel;
        }
    }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register("ViewModel", typeof(GamePageViewModel), typeof(PegSelectionComponent), new PropertyMetadata(null));

    public void Receive(MakeMoveMessage message)
    {
        // Move must be completed already
        if (message.IsSet)
            return;

        var animationService = ConnectedAnimationService.GetForCurrentView();
        this.FindChildrenRecursively<Peg>()
            .Foreach((peg, i) => animationService.PrepareToAnimate($"guess{i}", peg));
    }

    private void PegDragOver(object sender, DragEventArgs e)
    {
        e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Move;
    }

    private void PegDrop(object sender, DragEventArgs e)
    {
        var peg = (Peg)sender;
        var color = e.DataView.Properties.GetValueOrDefault("PegColor") as string;
        var shape = e.DataView.Properties.GetValueOrDefault("PegShape") as string;

        if (color is not null)
            peg.ColorName = color;
        else if (shape is not null)
            peg.ShapeName = shape;
    }
}

public static class TestExtensions
{
    public static TValue? GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) =>
        dictionary.TryGetValue(key, out var value) ? value : default;
}