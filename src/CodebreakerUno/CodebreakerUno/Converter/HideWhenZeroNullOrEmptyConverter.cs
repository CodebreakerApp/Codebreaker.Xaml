using System.Collections;
using Microsoft.UI.Xaml.Data;

namespace CodeBreaker.Uno.Converters;

internal class HideWhenZeroNullOrEmptyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language) =>
        value is
            null or
            0 or
            string and { Length: 0 } or
            ICollection and { Count: 0 }
                ? Visibility.Collapsed
                : Visibility.Visible;

    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotImplementedException();
}
