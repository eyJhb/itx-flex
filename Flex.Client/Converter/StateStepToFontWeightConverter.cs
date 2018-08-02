// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Converter.StateStepToFontWeightConverter
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Itx.Flex.Client.Converter
{
  [ValueConversion(typeof (StateStep), typeof (FontWeight))]
  public class StateStepToFontWeightConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      switch ((StateStep) value)
      {
        case StateStep.Finished:
          return (object) (FontWeight) Application.Current.FindResource((object) "StateStepFinishedFontWeight");
        case StateStep.Ongoing:
          return (object) (FontWeight) Application.Current.FindResource((object) "StateStepOngoingFontWeight");
        case StateStep.Awaiting:
          return (object) (FontWeight) Application.Current.FindResource((object) "StateStepAwaitingFontWeight");
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
