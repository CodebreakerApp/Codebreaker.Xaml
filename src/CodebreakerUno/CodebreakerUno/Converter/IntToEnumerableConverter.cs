using Microsoft.UI.Xaml.Data;

namespace CodeBreaker.Uno.Converters;

internal class IntToEnumerableConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not int count)
            throw new ArgumentException("The value needs to be an integer");

        return Enumerable.Range(0, count);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
