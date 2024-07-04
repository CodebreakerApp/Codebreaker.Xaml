using Codebreaker.ViewModels;
using Codebreaker.ViewModels.Messages;
using CommunityToolkit.Mvvm.Messaging;

namespace CodeBreaker.WinUI.Views.Components.GamePage;

public sealed partial class GameResultDisplay : UserControl,
    IRecipient<GameEndedMessage>,
    IRecipient<GameStartedMessage>
{
    public GameResultDisplay()
    {
        WeakReferenceMessenger.Default.RegisterAll(this);
        WeakReferenceMessenger.Default.UnregisterAllOnUnloaded(this);
        InitializeComponent();
        this.GoToState("Default", false);
    }

    public GamePageViewModel ViewModel
    {
        get => (GamePageViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(nameof(ViewModel), typeof(GamePageViewModel), typeof(GameResultDisplay), new PropertyMetadata(null));

    public void Receive(GameEndedMessage message) =>
        this.GoToState(message.IsVictory ? "Won" : "Lost");

    public void Receive(GameStartedMessage message) =>
        this.GoToState("Default");
}