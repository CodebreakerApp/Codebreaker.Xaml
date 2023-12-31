using Codebreaker.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Media.Animation;

namespace CodeBreaker.WinUI.Views.Components;

internal sealed partial class PegSelectionComponent : UserControl, IRecipient<GameMoveMessage>
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

    public void Receive(GameMoveMessage message)
    {
        if (message.GameMoveValue is not GameMoveValue.Started)
            return;

        var animationService = ConnectedAnimationService.GetForCurrentView();
        this.FindItemsOfType<ComboBox>(this)
            .Foreach((comboBox, i) => animationService.PrepareToAnimate($"guess{i}", comboBox));
    }
}
