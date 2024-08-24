using System;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using CoffeeSpace.AClient.Models;

namespace CoffeeSpace.AClient.Converters;

internal sealed class ImageInformationToBitMap : IValueConverter
{

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var imageInformation = (ImageInformation)value!;
        int baseIndex = imageInformation.Data.IndexOf(',') + 1;
        byte[] bytes = System.Convert.FromBase64String(imageInformation.Data[baseIndex..]);
        var bitmap = new Bitmap(new MemoryStream(bytes));   
        return bitmap;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}