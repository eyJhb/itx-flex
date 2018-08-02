// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.HandInFileViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;

namespace Itx.Flex.Client.ViewModel
{
  public class HandInFileViewModel : BaseViewModel
  {
    private ClickablePathViewModel _clickablePathViewModel;

    public HandInFileModel HandInFileModel { get; }

    public HandInFileViewModel(HandInFileModel handInFileModel)
    {
      this.HandInFileModel = handInFileModel;
      this.ClickablePathViewModel = new ClickablePathViewModel(handInFileModel.Path, handInFileModel.Name);
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
