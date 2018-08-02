// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.ClickablePathViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Itx.Flex.Client.ViewModel
{
  public class ClickablePathViewModel : BaseViewModel
  {
    private string _displayName;

    public ClickablePathViewModel(string path, string displayName = null)
    {
      this.OpenBrowserToPathCommand = (ICommand) new RelayCommand((Action<object>) (c => this.OpenBrowserToPathClick()), (Predicate<object>) null);
      this.Path = path;
      this.DisplayName = !string.IsNullOrEmpty(displayName) ? displayName : path;
    }

    private void OpenBrowserToPathClick()
    {
      try
      {
        Process.Start(this.Path);
      }
      catch (Exception ex)
      {
      }
    }

    private string Path { get; }

    public string DisplayName
    {
      get
      {
        return this._displayName;
      }
      private set
      {
        this._displayName = value;
        this.OnPropertyChanged(nameof (DisplayName));
      }
    }

    public ICommand OpenBrowserToPathCommand { get; set; }
  }
}
