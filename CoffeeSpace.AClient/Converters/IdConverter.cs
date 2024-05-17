using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace CoffeeSpace.AClient.Converters;

internal sealed class IdConverter : IValueConverter
{

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value!.ToString()![..8] + "...";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}