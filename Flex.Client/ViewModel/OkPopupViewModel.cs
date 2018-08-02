// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.OkPopupViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using Itx.Flex.Client.Message;
using System;
using System.Windows.Input;

namespace Itx.Flex.Client.ViewModel
{
  public class OkPopupViewModel : BaseViewModel
  {
    private readonly IMessenger _messenger;
    private string _messageText;
    private string _buttonText;

    public OkPopupViewModel(string messageText, string buttonText, IMessenger messenger)
    {
      this._messenger = messenger;
      this.MessageText = messageText;
      this.ButtonText = buttonText;
      this.ClosePopupCommand = (ICommand) new RelayCommand((Action<object>) (c => this.ClosePopup()), (Predicate<object>) null);
    }

    private void ClosePopup()
    {
      this._messenger.Send<OnOkPopupClosing>(new OnOkPopupClosing());
    }

    public ICommand ClosePopupCommand { get; }

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

    public string ButtonText
    {
      get
      {
        return this._buttonText;
      }
      set
      {
        this._buttonText = value;
        this.OnPropertyChanged(nameof (ButtonText));
      }
    }
  }
}
