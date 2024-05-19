namespace Microsoft.UI.Xaml;

internal static class VisualStateHelpers
{
    public static bool GoToState(this Control control, string stateName, bool useAnimation = true) =>
        VisualStateManager.GoToState(control, stateName, useAnimation);
}
