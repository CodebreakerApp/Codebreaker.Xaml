namespace Microsoft.Maui.Controls;

internal static class VisualStateHelpers
{
    public static bool GoToState(this VisualElement element, string stateName) =>
        VisualStateManager.GoToState(element, stateName);
}
