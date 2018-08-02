// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.View.LoginView
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Itx.Flex.Client.View
{
  public partial class LoginView : UserControl, IComponentConnector
  {
    internal LoginView LoginViewName;
    internal TextBox InputLoginTextBox;
    private bool _contentLoaded;

    public LoginView()
    {
      this.InitializeComponent();
      this.Loaded += (RoutedEventHandler) ((sender, args) => this.InputLoginTextBox.Focus());
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Flex.Client;component/view/loginview.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.InputLoginTextBox = (TextBox) target;
        else
          this._contentLoaded = true;
      }
      else
        this.LoginViewName = (LoginView) target;
    }
  }
}
