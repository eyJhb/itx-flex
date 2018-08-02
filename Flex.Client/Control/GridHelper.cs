// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Control.GridHelper
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Itx.Flex.Client.Control
{
  public class GridHelper
  {
    public static readonly DependencyProperty RowCountProperty = DependencyProperty.RegisterAttached("RowCount", typeof (int), typeof (GridHelper), new PropertyMetadata((object) -1, new PropertyChangedCallback(GridHelper.RowCountChanged)));
    public static readonly DependencyProperty ColumnCountProperty = DependencyProperty.RegisterAttached("ColumnCount", typeof (int), typeof (GridHelper), new PropertyMetadata((object) -1, new PropertyChangedCallback(GridHelper.ColumnCountChanged)));
    public static readonly DependencyProperty StarRowsProperty = DependencyProperty.RegisterAttached("StarRows", typeof (string), typeof (GridHelper), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(GridHelper.StarRowsChanged)));
    public static readonly DependencyProperty StarColumnsProperty = DependencyProperty.RegisterAttached("StarColumns", typeof (string), typeof (GridHelper), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(GridHelper.StarColumnsChanged)));

    public static int GetRowCount(DependencyObject obj)
    {
      return (int) obj.GetValue(GridHelper.RowCountProperty);
    }

    public static void SetRowCount(DependencyObject obj, int value)
    {
      obj.SetValue(GridHelper.RowCountProperty, (object) value);
    }

    public static void RowCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      if (!(obj is Grid) || (int) e.NewValue < 0)
        return;
      Grid grid = (Grid) obj;
      grid.RowDefinitions.Clear();
      for (int index = 0; index < (int) e.NewValue; ++index)
        grid.RowDefinitions.Add(new RowDefinition()
        {
          Height = GridLength.Auto
        });
      GridHelper.SetStarRows(grid);
    }

    public static int GetColumnCount(DependencyObject obj)
    {
      return (int) obj.GetValue(GridHelper.ColumnCountProperty);
    }

    public static void SetColumnCount(DependencyObject obj, int value)
    {
      obj.SetValue(GridHelper.ColumnCountProperty, (object) value);
    }

    public static void ColumnCountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      if (!(obj is Grid) || (int) e.NewValue < 0)
        return;
      Grid grid = (Grid) obj;
      grid.ColumnDefinitions.Clear();
      for (int index = 0; index < (int) e.NewValue; ++index)
        grid.ColumnDefinitions.Add(new ColumnDefinition()
        {
          Width = GridLength.Auto
        });
      GridHelper.SetStarColumns(grid);
    }

    public static string GetStarRows(DependencyObject obj)
    {
      return (string) obj.GetValue(GridHelper.StarRowsProperty);
    }

    public static void SetStarRows(DependencyObject obj, string value)
    {
      obj.SetValue(GridHelper.StarRowsProperty, (object) value);
    }

    public static void StarRowsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      if (!(obj is Grid) || string.IsNullOrEmpty(e.NewValue.ToString()))
        return;
      GridHelper.SetStarRows((Grid) obj);
    }

    public static string GetStarColumns(DependencyObject obj)
    {
      return (string) obj.GetValue(GridHelper.StarColumnsProperty);
    }

    public static void SetStarColumns(DependencyObject obj, string value)
    {
      obj.SetValue(GridHelper.StarColumnsProperty, (object) value);
    }

    public static void StarColumnsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
    {
      if (!(obj is Grid) || string.IsNullOrEmpty(e.NewValue.ToString()))
        return;
      GridHelper.SetStarColumns((Grid) obj);
    }

    private static void SetStarColumns(Grid grid)
    {
      string[] strArray = GridHelper.GetStarColumns((DependencyObject) grid).Split(',');
      for (int index = 0; index < grid.ColumnDefinitions.Count; ++index)
      {
        if (((IEnumerable<string>) strArray).Contains<string>(index.ToString()))
          grid.ColumnDefinitions[index].Width = new GridLength(1.0, GridUnitType.Star);
      }
    }

    private static void SetStarRows(Grid grid)
    {
      string[] strArray = GridHelper.GetStarRows((DependencyObject) grid).Split(',');
      for (int index = 0; index < grid.RowDefinitions.Count; ++index)
      {
        if (((IEnumerable<string>) strArray).Contains<string>(index.ToString()))
          grid.RowDefinitions[index].Height = new GridLength(1.0, GridUnitType.Star);
      }
    }
  }
}
