using Codebreaker.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;

namespace CodeBreaker.WinUI.Views.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class GamePage : Page, IRecipient<GameMoveMessage>
{
    public GamePageViewModel ViewModel { get; }

    public GamePage()
    {
        ViewModel = App.GetService<GamePageViewModel>();
        InitializeComponent();
        WeakReferenceMessenger.Default.Register(this);
        WeakReferenceMessenger.Default.UnregisterAllOnUnloaded(this);
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        VisualStateManager.GoToState(this, "Start", false);
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(GamePageViewModel.GameStatus))
            return;

        var stateName = ViewModel.GameStatus switch
        {
            GameMode.Started or GameMode.MoveSet => "Playing",
            GameMode.Won or GameMode.Lost => "Finished",
            _ => "Start",
        };
        VisualStateManager.GoToState(this, stateName, true);
    }

    public void Receive(GameMoveMessage message)
    {
        if (message.GameMoveValue is not GameMoveValue.Completed)
            return;

        var selectionAndKeyPegs = message.SelectionAndKeyPegs ?? throw new InvalidOperationException();
        var animationService = ConnectedAnimationService.GetForCurrentView();
        animationService.DefaultDuration = TimeSpan.FromMilliseconds(500);
        var container = listGameMoves.ItemContainerGenerator.ContainerFromItem(selectionAndKeyPegs);
        this.FindItemsOfType<Ellipse>(container)
            .Foreach((ellipse, i) =>
            {
                ConnectedAnimation? animation = animationService.GetAnimation($"guess{i}");

                // No animation found for this ellipxe -> the ellipse is most likely a key-peg
                if (animation is null)
                    return;

                animation.Configuration = new BasicConnectedAnimationConfiguration();
                animation.TryStart(ellipse);
            });

        // Scroll to bottom
        PegScrollViewer.UpdateLayout();
        PegScrollViewer.ScrollToVerticalOffset(PegScrollViewer.ScrollableHeight);
    }
}
