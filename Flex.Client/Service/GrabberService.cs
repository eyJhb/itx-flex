// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.GrabberService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Grabber.Core;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Itx.Flex.Client.Service
{
  public class GrabberService : IGrabberService
  {
    private List<ICaptureWindow> captureWindows = new List<ICaptureWindow>();
    private readonly IGrabberSession grabberSession;
    private readonly ITimerService noGrabAfterTimer;
    private readonly ITimerService startGrabTimer;
    private readonly IConfigurationService _configurationService;

    public GrabberService(IGrabberSession grabberSession, IMessenger messenger, ITimerService noGrabAfterTimer, ITimerService startGrabTimer, IConfigurationService configurationService)
    {
      this.grabberSession = grabberSession;
      this.noGrabAfterTimer = noGrabAfterTimer;
      this.startGrabTimer = startGrabTimer;
      this._configurationService = configurationService;
      this.noGrabAfterTimer.AutoReset = false;
      this.noGrabAfterTimer.Elapsed += (ElapsedEventHandler) ((sender, args) => this.grabberSession.Stop());
      this.startGrabTimer.AutoReset = false;
      this.startGrabTimer.Elapsed += (ElapsedEventHandler) ((sender, args) => this.grabberSession.Start());
      grabberSession.StateChanged += new StateChangedEventHandler(this.GrabberSession_StateChanged);
      messenger.Register<OnNewGrabSettings>((object) this, new Action<OnNewGrabSettings>(this.OnNewGrabSettings));
      foreach (Screen allScreen in Screen.AllScreens)
      {
        Rectangle bounds = allScreen.Bounds;
        this.captureWindows.Add((ICaptureWindow) new CaptureWindow(bounds.Left, bounds.Top, bounds.Width, bounds.Height));
      }
    }

    private void GrabberSession_StateChanged(GrabberState newState)
    {
      switch (newState)
      {
        case GrabberState.Running:
        case GrabberState.Error:
          DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
          {
            foreach (ICaptureWindow captureWindow in this.captureWindows)
              captureWindow.ShowOverlay();
          }));
          break;
        default:
          DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
          {
            foreach (ICaptureWindow captureWindow in this.captureWindows)
              captureWindow.HideOverlay();
          }));
          break;
      }
    }

    private void OnNewGrabSettings(OnNewGrabSettings onNewGrabSettings)
    {
      Task.Factory.StartNew((Action) (() =>
      {
        if (onNewGrabSettings.EnableGrabs)
        {
          if (onNewGrabSettings.TimeUntilStartGrabInSeconds > 0)
          {
            this.startGrabTimer.Stop();
            this.startGrabTimer.Interval = (double) (onNewGrabSettings.TimeUntilStartGrabInSeconds * 1000);
            this.startGrabTimer.Start();
          }
          else if (onNewGrabSettings.NoGrabsAfterSeconds > 0)
          {
            this.grabberSession.Start();
            this.noGrabAfterTimer.Stop();
            this.noGrabAfterTimer.Interval = (double) (onNewGrabSettings.NoGrabsAfterSeconds * 1000);
            this.noGrabAfterTimer.Start();
          }
          else
            this.grabberSession.Stop();
        }
        else
          this.grabberSession.Stop();
      }));
    }

    public void Stop()
    {
      this.grabberSession.Stop();
    }

    public void UploadPreviousSessions()
    {
      this.grabberSession.FinishPreviousSessions(ConfigurationService.GlobalEndpoint, (IEnumerable<PrefixEndpointSelector>) this._configurationService.GlobalResponse.EndpointSelectors);
    }
  }
}
