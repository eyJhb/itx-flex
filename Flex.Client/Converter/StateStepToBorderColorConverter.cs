// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Converter.StateStepToBorderColorConverter
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Itx.Flex.Client.Converter
{
  [ValueConversion(typeof (StateStep), typeof (SolidColorBrush))]
  public class StateStepToBorderColorConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      switch ((StateStep) value)
      {
        case StateStep.Finished:
          return (object) (SolidColorBrush) Application.Current.FindResource((object) "StateStepFinishedBorderColor");
        case StateStep.Ongoing:
          return (object) (SolidColorBrush) Application.Current.FindResource((object) "StateStepOngoingBorderColor");
        case StateStep.Awaiting:
          return (object) (SolidColorBrush) Application.Current.FindResource((object) "StateStepAwaitingBorderColor");
        default:
          throw new ArgumentOutOfRangeException(nameof (value), value, (string) null);
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
