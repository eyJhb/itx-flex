// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Converter.StateStepMaxWidthMultiValueConverter
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace Itx.Flex.Client.Converter
{
  public class StateStepMaxWidthMultiValueConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      return (object) (double.Parse(values[0].ToString()) / double.Parse(values[1].ToString()) - 40.0);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
