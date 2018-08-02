﻿// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.View.OkCancelPopupView
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using Itx.Flex.Client.Message;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

namespace Itx.Flex.Client.View
{
  public partial class OkCancelPopupView : Window, IComponentConnector
  {
    private bool _contentLoaded;

    public OkCancelPopupView()
    {
      this.InitializeComponent();
      Messenger.Default.Register<OnOkCancelPopupClosing>((object) this, new Action<OnOkCancelPopupClosing>(this.ClosePopup));
    }

    private void ClosePopup(OnOkCancelPopupClosing onOkCancelPopupClosing)
    {
      this.OkSelected = onOkCancelPopupClosing.OkSelected;
      this.Close();
    }

    public bool OkSelected { get; private set; }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Flex.Client;component/view/okcancelpopupview.xaml", UriKind.Relative));
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
