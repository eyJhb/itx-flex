// Decompiled with JetBrains decompiler
// Type: Flex.Updater.App
// Assembly: Flex.Updater, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: A51821AE-A539-4ADD-B006-972CC1F7D242
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Updater.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;

namespace Flex.Updater
{
  public class App : Application
  {
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      this.StartupUri = new Uri("UpdateMainWindow.xaml", UriKind.Relative);
    }

    [STAThread]
    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public static void Main()
    {
      App app = new App();
      app.InitializeComponent();
      app.Run();
    }
  }
}
