using Microsoft.UI.Xaml.Data;

namespace CodeBreaker.Uno.Converters;

internal class ElementThemeToIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not ElementTheme elementTheme)
            throw new ArgumentException(nameof(value), "Invalid type");

        return (int)elementTheme;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is not int integer)
            throw new ArgumentException(nameof(value), "Invalid type");

        if (!Enum.IsDefined((ElementTheme)integer))
            return ElementTheme.Default;

        return (ElementTheme)integer;
    }
}
