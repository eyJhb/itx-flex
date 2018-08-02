// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Converter.StateStepPointMultiValueConverter
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Itx.Flex.Client.Converter
{
  public class StateStepPointMultiValueConverter : IMultiValueConverter
  {
    private readonly List<double> _addToXsEnd = new List<double>() { 0.0, 0.0, 0.0 };
    private readonly List<double> _addToXsArrow = new List<double>() { 0.0, 20.0, 0.0 };
    private readonly List<double> _yCoordinates = new List<double>() { 50.0, 25.0, 0.0 };

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      double num1 = (double) values[0];
      int num2 = (int) Enum.Parse(typeof (ViewState), values[1].ToString()) + 1;
      int index = int.Parse(values[2].ToString());
      int num3 = int.Parse(values[3].ToString());
      List<double> doubleList = num3 == num2 ? this._addToXsEnd : this._addToXsArrow;
      double num4 = (double) num3;
      return (object) new Point(num1 / num4 * (double) num2 + doubleList[index], this._yCoordinates[index]);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
