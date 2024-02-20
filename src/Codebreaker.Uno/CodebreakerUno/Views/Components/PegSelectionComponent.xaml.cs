using Codebreaker.ViewModels;
using CodeBreaker.Uno.Helpers;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Media.Animation;

namespace CodeBreaker.Uno.Views.Components;

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
        set => SetValue(ViewModelProperty, value);
    }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register("ViewModel", typeof(GamePageViewModel), typeof(PegSelectionComponent), new PropertyMetadata(null, propertyChangedCallback: OnViewModelChanged));

    private static void OnViewModelChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
    {
        var @this = (PegSelectionComponent)dependencyObject;
        @this.DataContext = (GamePageViewModel)args.NewValue;
    }

    public void Receive(GameMoveMessage message)
    {
        if (message.GameMoveValue is not GameMoveValue.Started)
            return;

        var animationService = ConnectedAnimationService.GetForCurrentView();
        this.FindItemsOfType<ComboBox>(this)
            .ForEach((i, comboBox) => animationService.PrepareToAnimate($"guess{i}", comboBox));
    }
}
