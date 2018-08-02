// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Converter.HtmlToFlowDocumentConverter
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using HTMLConverter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Itx.Flex.Client.Converter
{
  public class HtmlToFlowDocumentConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
        return value;
      FlowDocument flowDocument = new FlowDocument();
      using (MemoryStream memoryStream = new MemoryStream(new UTF8Encoding().GetBytes(HtmlToXamlConverter.ConvertHtmlToXaml(value.ToString(), false))))
        new TextRange(flowDocument.ContentStart, flowDocument.ContentEnd).Load((Stream) memoryStream, DataFormats.Xaml);
      this.SubscribeToAllHyperlinks(flowDocument);
      flowDocument.FontStyle = FontStyles.Italic;
      flowDocument.FontSize = SystemFonts.MessageFontSize;
      flowDocument.FontFamily = SystemFonts.MessageFontFamily;
      return (object) flowDocument;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    private void SubscribeToAllHyperlinks(FlowDocument flowDocument)
    {
      foreach (Hyperlink hyperlink in HtmlToFlowDocumentConverter.GetVisuals((DependencyObject) flowDocument).OfType<Hyperlink>())
        hyperlink.RequestNavigate += new RequestNavigateEventHandler(this.link_RequestNavigate);
    }

    private static IEnumerable<DependencyObject> GetVisuals(DependencyObject root)
    {
      foreach (DependencyObject dependencyObject in LogicalTreeHelper.GetChildren(root).OfType<DependencyObject>())
      {
        DependencyObject child = dependencyObject;
        yield return child;
        foreach (DependencyObject visual in HtmlToFlowDocumentConverter.GetVisuals(child))
          yield return visual;
        child = (DependencyObject) null;
      }
    }

    private void link_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
      Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
      e.Handled = true;
    }
  }
}
