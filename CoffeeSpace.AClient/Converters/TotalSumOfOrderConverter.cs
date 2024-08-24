using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using CoffeeSpace.AClient.Models;

namespace CoffeeSpace.AClient.Converters;

internal sealed class TotalSumOfOrderConverter : IValueConverter
{

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not Order order
            ? string.Empty
            : order.OrderItems.Sum(item => item.Total).ToString("F1") + " $";

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}