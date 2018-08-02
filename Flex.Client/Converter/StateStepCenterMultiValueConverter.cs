// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Converter.StateStepCenterMultiValueConverter
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.ViewModel;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Itx.Flex.Client.Converter
{
  public class StateStepCenterMultiValueConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      double num1 = double.Parse(values[0].ToString());
      int num2 = (int) Enum.Parse(typeof (ViewState), values[1].ToString());
      double num3 = (double) int.Parse(values[2].ToString());
      return (object) new Thickness(num1 / num3 * (double) num2, 0.0, 0.0, 0.0);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
