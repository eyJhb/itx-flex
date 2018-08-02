// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.View.PinCodePopupView
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.ViewModel;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace Itx.Flex.Client.View
{
  public partial class PinCodePopupView : Window, IComponentConnector
  {
    private bool _allowClosing = true;
    internal TextBox PinCodeTextBox;
    private bool _contentLoaded;

    protected override void OnClosing(CancelEventArgs e)
    {
      e.Cancel = !this._allowClosing;
      base.OnClosing(e);
    }

    public PinCodePopupView()
    {
      this.InitializeComponent();
      Messenger.Default.Register<OnAllowClosingPinCodePopup>((object) this, (Action<OnAllowClosingPinCodePopup>) (c => this._allowClosing = c.AllowClosing));
      Messenger.Default.Register<OnPinCodeLoginSuccessful>((object) this, (Action<OnPinCodeLoginSuccessful>) (c => DispatcherHelper.CheckBeginInvokeOnUI(new Action(((Window) this).Close))));
      Messenger.Default.Register<OnPinCodeLoginCancelled>((object) this, (Action<OnPinCodeLoginCancelled>) (c => DispatcherHelper.CheckBeginInvokeOnUI(new Action(((Window) this).Close))));
      Messenger.Default.Register<OnPinCodeLoginForceContinue>((object) this, (Action<OnPinCodeLoginForceContinue>) (c => DispatcherHelper.CheckBeginInvokeOnUI(new Action(((Window) this).Close))));
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
      (this.DataContext as PinCodePopupViewModel)?.HandleKeyDown(e.Key);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Flex.Client;component/view/pincodepopupview.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.PinCodeTextBox = (TextBox) target;
      else
        this._contentLoaded = true;
    }
  }
}
