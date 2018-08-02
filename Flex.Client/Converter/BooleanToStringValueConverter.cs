// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Converter.BooleanToStringValueConverter
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace Itx.Flex.Client.Converter
{
  public class BooleanToStringValueConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (Convert.ToString(value).Equals(Convert.ToString(parameter)))
        return (object) true;
      return (object) false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (Convert.ToBoolean(value))
        return parameter;
      return (object) null;
    }
  }
}
