// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.OkCancelPopupViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using Itx.Flex.Client.Message;
using System;
using System.Windows.Input;

namespace Itx.Flex.Client.ViewModel
{
  public class OkCancelPopupViewModel : BaseViewModel
  {
    private readonly IMessenger _messenger;
    private string _messageText;
    private string _okButtonText;
    private string _cancelButtonText;

    public OkCancelPopupViewModel(string messageText, string okButtonText, string cancelButtonText, IMessenger messenger)
    {
      this._messenger = messenger;
      this.MessageText = messageText;
      this.OkButtonText = okButtonText;
      this.CancelButtonText = cancelButtonText;
      this.OkPopupCommand = (ICommand) new RelayCommand((Action<object>) (c => this.ClosePopup(true)), (Predicate<object>) null);
      this.CancelPopupCommand = (ICommand) new RelayCommand((Action<object>) (c => this.ClosePopup(false)), (Predicate<object>) null);
    }

    private void ClosePopup(bool okSelected)
    {
      this._messenger.Send<OnOkCancelPopupClosing>(new OnOkCancelPopupClosing(okSelected));
    }

    public ICommand OkPopupCommand { get; }

    public ICommand CancelPopupCommand { get; }

    public string MessageText
    {
      get
      {
        return this._messageText;
      }
      set
      {
        this._messageText = value;
        this.OnPropertyChanged(nameof (MessageText));
      }
    }

    public string OkButtonText
    {
      get
      {
        return this._okButtonText;
      }
      set
      {
        this._okButtonText = value;
        this.OnPropertyChanged(nameof (OkButtonText));
      }
    }

    public string CancelButtonText
    {
      get
      {
        return this._cancelButtonText;
      }
      set
      {
        this._cancelButtonText = value;
        this.OnPropertyChanged(nameof (CancelButtonText));
      }
    }
  }
}
