// Decompiled with JetBrains decompiler
// Type: Flex.Updater.UpdateMainWindow
// Assembly: Flex.Updater, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: A51821AE-A539-4ADD-B006-972CC1F7D242
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Updater.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Flex.Updater
{
  public partial class UpdateMainWindow : Window, IComponentConnector
  {
    internal Label LabelStatus;
    private bool _contentLoaded;

    public UpdateMainWindow()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.OnLoaded);
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      StartupMsiInstaller updater = new StartupMsiInstaller();
      Action<string> statusCallback = (Action<string>) (status => this.Dispatcher.Invoke((Delegate) (() => this.LabelStatus.Content = (object) status)));
      Task.Factory.StartNew((Action) (() =>
      {
        try
        {
          updater.RunUpdate(statusCallback);
        }
        catch (Exception ex)
        {
          int num;
          this.Dispatcher.Invoke((Delegate) (() => num = (int) MessageBox.Show("Error: " + ex.Message)));
        }
        this.Dispatcher.Invoke((Delegate) new Action(((Window) this).Close));
      }));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Flex.Updater;component/updatemainwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.LabelStatus = (Label) target;
      else
        this._contentLoaded = true;
    }
  }
}
