namespace Codebreaker.WinForms.Helpers;

/// <summary>
/// Helper class to convert color names to Color objects for WinForms.
/// </summary>
public static class ColorHelper
{
    public static Color GetColorFromName(string colorName)
    {
        return colorName.ToLowerInvariant() switch
        {
            "red" => Color.Red,
            "green" => Color.Green,
            "blue" => Color.Blue,
            "yellow" => Color.Yellow,
            "orange" => Color.Orange,
            "purple" => Color.Purple,
            "white" => Color.White,
            "black" => Color.Black,
            _ => Color.Gray
        };
    }
}
