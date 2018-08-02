// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.View.MainWindow
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
using System.Windows.Markup;

namespace Itx.Flex.Client.View
{
  public partial class MainWindow : Window, IComponentConnector
  {
    private readonly IMessenger _messenger;
    private bool _contentLoaded;

    protected override void OnClosing(CancelEventArgs e)
    {
      e.Cancel = true;
      this._messenger.Send<OnClosingProgramRequested>(new OnClosingProgramRequested());
    }

    public MainWindow(IMainWindowViewModel mainWindowViewModel, IMessenger messenger)
    {
      this._messenger = messenger;
      this.DataContext = (object) mainWindowViewModel;
      this._messenger.Register<OnLoginErrorPopupOpened>((object) this, new Action<OnLoginErrorPopupOpened>(this.OnLoginErrorPopupOpened));
      this._messenger.Register<OnOkPopupOpened>((object) this, new Action<OnOkPopupOpened>(this.OnOkPopupOpened));
      this._messenger.Register<OnOkCancelPopupOpened>((object) this, new Action<OnOkCancelPopupOpened>(this.OnOkCancelPopupOpened));
      this.InitializeComponent();
    }

    private void OnOkPopupOpened(OnOkPopupOpened onOkPopupOpened)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        new OkPopupView()
        {
          Owner = ((Window) this),
          DataContext = ((object) onOkPopupOpened.OkPopupViewModel)
        }.ShowDialog();
      }));
    }

    private void OnOkCancelPopupOpened(OnOkCancelPopupOpened onOkCancelPopupOpened)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        OkCancelPopupView okCancelPopupView = new OkCancelPopupView();
        okCancelPopupView.Owner = (Window) this;
        okCancelPopupView.DataContext = (object) onOkCancelPopupOpened.OkCancelPopupViewModel;
        okCancelPopupView.ShowDialog();
        onOkCancelPopupOpened.ExecuteAfterPopup(okCancelPopupView.OkSelected);
      }));
    }

    private void OnLoginErrorPopupOpened(OnLoginErrorPopupOpened obj)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        new OkPopupView()
        {
          Owner = ((Window) this),
          DataContext = ((object) obj.OkPopupViewModel)
        }.ShowDialog();
        this._messenger.Send<OnLoginErrorPopupClosed>(new OnLoginErrorPopupClosed());
      }));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Flex.Client;component/view/mainwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      this._contentLoaded = true;
    }
  }
}
