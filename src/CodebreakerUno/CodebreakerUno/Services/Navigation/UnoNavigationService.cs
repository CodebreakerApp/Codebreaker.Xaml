using CodeBreaker.Uno.Contracts.Services.Navigation;
using CodeBreaker.Uno.Contracts.ViewModels;
using CodeBreaker.Uno.Helpers;
using Microsoft.UI.Xaml.Media.Animation;

namespace CodeBreaker.Uno.Services.Navigation;

internal class UnoNavigationService(IPageService pageService) : IUnoNavigationService
{
    private readonly IPageService _pageService = pageService;
    private object? _lastParameterUsed;
    private Frame? _frame;

    public event NavigatedEventHandler? Navigated;
    public event NavigationFailedEventHandler? NavigationFailed;

    public NavigationTransitionInfo? EntranceNavigationTransition { get; set; }

    public NavigationTransitionInfo? BackNavigationTransition { get; set; }

    public Frame Frame
    {
        get
        {
            if (_frame is null)
            {
                _frame = App.Current.MainWindow?.Content as Frame ?? throw new InvalidOperationException("MainWindow content is not a frame");
                RegisterFrameEvents();
            }

            return _frame;
        }

        set
        {
            UnregisterFrameEvents();
            _frame = value;
            RegisterFrameEvents();
        }
    }

    public bool CanGoBack => Frame.CanGoBack;

    private void RegisterFrameEvents()
    {
        if (_frame is not null)
            _frame.Navigated += OnNavigated;
    }

    private void UnregisterFrameEvents()
    {
        if (_frame is not null)
            _frame.Navigated -= OnNavigated;
    }

    public ValueTask<bool> GoBackAsync() =>
        ValueTask.FromResult(GoBack());

    public ValueTask<bool> NavigateToAsync(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        var pageType = _pageService.GetPageType(pageKey);
        return ValueTask.FromResult(NavigateTo(pageType, parameter, clearNavigation));
    }

    private bool GoBack()
    {
        if (!CanGoBack || _frame is null)
            return false;

        object? vmBeforeNavigation = _frame.GetPageViewModel();
        _frame.GoBack(BackNavigationTransition);

        if (vmBeforeNavigation is INavigationAware navigationAware)
            navigationAware.OnNavigatedFrom();

        return true;
    }

    private bool NavigateTo(Type pageType, object? parameter, bool clearNavigation = false)
    {
        if (_frame != null && (_frame.Content?.GetType() != pageType || parameter != null && !parameter.Equals(_lastParameterUsed)))
        {
            _frame!.Tag = clearNavigation;
            var vmBeforeNavigation = _frame.GetPageViewModel();
            var navigated = _frame.Navigate(pageType, parameter, EntranceNavigationTransition);

            if (navigated)
            {
                _lastParameterUsed = parameter;

                if (vmBeforeNavigation is INavigationAware navigationAware)
                    navigationAware.OnNavigatedFrom();
            }

            return navigated;
        }

        return false;
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is not Frame frame)
            return;

        bool clearNavigation = (bool)frame.Tag;

        if (clearNavigation)
            frame.BackStack.Clear();

        if (frame.GetPageViewModel() is INavigationAware navigationAware)
            navigationAware.OnNavigatedTo(e.Parameter);

        Navigated?.Invoke(sender, e);
    }
}
