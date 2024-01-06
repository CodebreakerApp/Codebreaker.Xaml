using Codebreaker.WPF.Contracts.Services.Navigation;

namespace Codebreaker.WPF.Services.Navigation;

internal class WPFNavigationService(IPageService pageService) : IWPFNavigationService
{
    private Frame? _frame;

    private object? _lastParameterUsed;

    public Frame Frame
    {
        get
        {
            if (_frame is null)
                _frame = App.Current.MainWindow.Content as Frame ?? throw new InvalidOperationException("MainWindow content is not a frame");

            return _frame;
        }
        set
        {
            bool firstSet = _frame is null;
            _frame = value;

            if (firstSet)
                NavigateTo(pageService.GetInitialPage());
        }
    }

    public bool CanGoBack { get; }

    public bool GoBack()
    {
        if (Frame.CanGoBack || _frame is null)
            return false;

        Frame.GoBack();
        return true;
    }

    public ValueTask<bool> GoBackAsync() =>
        ValueTask.FromResult(GoBack());

    public ValueTask<bool> NavigateToAsync(string key, object? parameter = null, bool clearNavigation = false)
    {
        var page = pageService.GetPage(key);
        return ValueTask.FromResult(NavigateTo(page, parameter, clearNavigation));
    }

    private bool NavigateTo(Page page, object? parameter = null, bool clearNavigation = false)
    {
        if (_frame != null && (_frame.Content?.GetType() != page.GetType() || parameter is not null && !parameter.Equals(_lastParameterUsed)))
        {
            _frame!.Tag = clearNavigation;
            var navigated = _frame.Navigate(page);

            if (navigated)
            {
                _lastParameterUsed = parameter;
            }

            return navigated;
        }

        return false;
    }
}
