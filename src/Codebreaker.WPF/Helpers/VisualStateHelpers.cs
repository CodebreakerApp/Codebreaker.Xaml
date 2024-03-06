namespace Codebreaker.WPF.Helpers;

internal static class VisualStateHelpers
{
    public static bool GoToState(this FrameworkElement element, string visualStateName, bool useTransitions = true) =>
        VisualStateManager.GoToElementState(element, visualStateName, useTransitions);
}
