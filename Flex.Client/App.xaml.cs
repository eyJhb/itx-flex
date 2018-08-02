// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.App
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Service;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace Itx.Flex.Client
{
  public partial class App : Application
  {
    private static Mutex mutex;
    private bool _contentLoaded;

    protected override void OnStartup(StartupEventArgs e)
    {
      bool createdNew;
      App.mutex = new Mutex(true, "ItxFlex", out createdNew);
      if (!createdNew)
        Application.Current.Shutdown();
      base.OnStartup(e);
      try
      {
        Startup.Start();
      }
      catch (Exception ex)
      {
        new LoggerService((IFileService) new FileService(), (IConfigurationService) new ConfigurationService(), (IDateTimeService) new DateTimeService(new Stopwatch())).Log(LogType.Fatal, "Uncaught error: " + ex.Message, ex.StackTrace);
        int num = (int) MessageBox.Show(ex.ToString());
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Flex.Client;component/app.xaml", UriKind.Relative));
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
