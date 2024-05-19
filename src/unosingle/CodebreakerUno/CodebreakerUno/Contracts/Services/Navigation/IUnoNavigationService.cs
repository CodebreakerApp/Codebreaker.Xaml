using Codebreaker.ViewModels.Contracts.Services;
using Microsoft.UI.Xaml.Media.Animation;

namespace CodeBreaker.Uno.Contracts.Services.Navigation;

public interface IUnoNavigationService : INavigationService
{
    NavigationTransitionInfo? BackNavigationTransition { get; set; }
    NavigationTransitionInfo? EntranceNavigationTransition { get; set; }
    Frame Frame { get; set; }

    event NavigatedEventHandler? Navigated;
    event NavigationFailedEventHandler? NavigationFailed;
}
