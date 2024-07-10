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
        this.FindChildrenRecursively<ComboBox>()
            .Foreach((comboBox, i) => animationService.PrepareToAnimate($"guess{i}", comboBox));
    }
}
