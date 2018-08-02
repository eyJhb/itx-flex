// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.PathLinkViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

namespace Itx.Flex.Client.ViewModel
{
  public class PathLinkViewModel : BaseViewModel
  {
    private string _pathText;
    private ClickablePathViewModel _clickablePathViewModel;

    public string PathText
    {
      get
      {
        return this._pathText;
      }
      set
      {
        this._pathText = value;
        this.OnPropertyChanged(nameof (PathText));
      }
    }

    public ClickablePathViewModel ClickablePathViewModel
    {
      get
      {
        return this._clickablePathViewModel;
      }
      set
      {
        if (this._clickablePathViewModel == value)
          return;
        this._clickablePathViewModel = value;
        this.OnPropertyChanged(nameof (ClickablePathViewModel));
      }
    }
  }
}
