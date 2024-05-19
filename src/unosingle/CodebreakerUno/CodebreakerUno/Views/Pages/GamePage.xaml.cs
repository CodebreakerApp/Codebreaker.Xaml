using Codebreaker.ViewModels;
using CodeBreaker.Uno.Helpers;
using CommunityToolkit.Mvvm.Messaging;
using Windows.ApplicationModel.Resources;
#if WINDOWS10_0_17763_0_OR_GREATER
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
#endif

namespace CodeBreaker.Uno.Views.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class GamePage : Page, IRecipient<GameMoveMessage>
{
    public GamePage()
    {
        ViewModel = App.GetService<GamePageViewModel>();
        InitializeComponent();
        WeakReferenceMessenger.Default.Register(this);
        WeakReferenceMessenger.Default.UnregisterAllOnUnloaded(this);
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        this.GoToState("Start", false);
    }

    public GamePageViewModel ViewModel { get; }

    public void Receive(GameMoveMessage message)
    {
        if (message.GameMoveValue is not GameMoveValue.Completed)
            return;

#if WINDOWS10_0_17763_0_OR_GREATER
        var selectionAndKeyPegs = message.SelectionAndKeyPegs ?? throw new InvalidOperationException();
        var animationService = ConnectedAnimationService.GetForCurrentView();
        animationService.DefaultDuration = TimeSpan.FromMilliseconds(500);
        var container = listGameMoves.ItemContainerGenerator.ContainerFromItem(selectionAndKeyPegs);
        this.FindItemsOfType<Ellipse>(container)
            .ForEach((i, ellipse) =>
            {
                ConnectedAnimation? animation = animationService.GetAnimation($"guess{i}");

                // No animation found for this ellipxe -> the ellipse is most likely a key-peg
                if (animation is null)
                    return;

                animation.Configuration = new BasicConnectedAnimationConfiguration();
                animation.TryStart(ellipse);
            });
#endif

        // Scroll to bottom
        PegScrollViewer.UpdateLayout();
        PegScrollViewer.ScrollToVerticalOffset(PegScrollViewer.ScrollableHeight);
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
        this.GoToState(stateName);
    }
}
