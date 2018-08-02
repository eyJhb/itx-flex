// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Converter.EmptyListToVisibilityConverter
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Itx.Flex.Client.Converter
{
  public class EmptyListToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) (Visibility) (int.Parse(value.ToString()) == 0 ? 2 : 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
