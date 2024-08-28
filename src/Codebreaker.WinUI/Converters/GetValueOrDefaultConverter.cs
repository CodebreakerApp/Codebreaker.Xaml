using Microsoft.UI.Xaml.Data;
using System.Collections;

namespace CodeBreaker.WinUI.Converters;

public class GetValueOrDefaultConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not IDictionary dictionary)
            throw new ArgumentException($"The provided value for the {nameof(GetValueOrDefaultConverter)} must be a {nameof(IDictionary)}");

        return dictionary.Contains(parameter)
            ? dictionary[parameter]
            : null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
