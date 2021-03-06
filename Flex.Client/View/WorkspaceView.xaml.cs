﻿// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.View.WorkspaceView
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Message;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Itx.Flex.Client.View
{
  public partial class WorkspaceView : UserControl, IComponentConnector
  {
    internal WorkspaceView WorkspaceViewName;
    private bool _contentLoaded;

    public WorkspaceView()
    {
      this.InitializeComponent();
      Messenger.Default.Register<OnPinCodePopupOpened>((object) this, new Action<OnPinCodePopupOpened>(this.OnPinCodePopupOpened));
      Messenger.Default.Register<OnWorkspaceDirectoryAccessDeniedPopupOpened>((object) this, new Action<OnWorkspaceDirectoryAccessDeniedPopupOpened>(this.OnWorkspaceDirectoryAccessDeniedPopupOpened));
    }

    private void OnPinCodePopupOpened(OnPinCodePopupOpened obj)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        new PinCodePopupView()
        {
          Owner = Window.GetWindow((DependencyObject) this),
          DataContext = ((object) obj.PinCodePopupViewModel)
        }.ShowDialog();
      }));
    }

    private void OnWorkspaceDirectoryAccessDeniedPopupOpened(OnWorkspaceDirectoryAccessDeniedPopupOpened onWorkspaceDirectoryAccessDeniedPopupOpened)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        new OkPopupView()
        {
          Owner = Window.GetWindow((DependencyObject) this),
          DataContext = ((object) onWorkspaceDirectoryAccessDeniedPopupOpened.OkPopupViewModel)
        }.ShowDialog();
      }));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Flex.Client;component/view/workspaceview.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.WorkspaceViewName = (WorkspaceView) target;
      else
        this._contentLoaded = true;
    }
  }
}
