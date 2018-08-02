// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.View.CaptureWindow
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;

namespace Itx.Flex.Client.View
{
  public partial class CaptureWindow : Window, ICaptureWindow, IComponentConnector
  {
    private const int WS_EX_TRANSPARENT = 32;
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_TOOLWINDOW = 128;
    private bool _contentLoaded;

    public CaptureWindow(int left, int top, int width, int height)
    {
      this.InitializeComponent();
      this.Left = (double) left;
      this.Top = (double) top;
      this.Width = (double) width;
      this.Height = (double) height;
    }

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

    protected override void OnSourceInitialized(EventArgs e)
    {
      base.OnSourceInitialized(e);
      IntPtr handle = new WindowInteropHelper((Window) this).Handle;
      CaptureWindow.SetWindowLong(handle, -20, CaptureWindow.GetWindowLong(handle, -20) | 32 | 128);
    }

    public void HideOverlay()
    {
      this.Hide();
    }

    public void ShowOverlay()
    {
      this.Show();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Flex.Client;component/view/capturewindow.xaml", UriKind.Relative));
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
