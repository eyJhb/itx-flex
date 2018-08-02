// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.TimerService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System.Timers;

namespace Itx.Flex.Client.Service
{
  public class TimerService : ITimerService
  {
    private readonly Timer timer;

    public TimerService(Timer timer)
    {
      this.timer = timer;
    }

    public double Interval
    {
      get
      {
        return this.timer.Interval;
      }
      set
      {
        this.timer.Interval = value;
      }
    }

    public bool AutoReset
    {
      get
      {
        return this.timer.AutoReset;
      }
      set
      {
        this.timer.AutoReset = value;
      }
    }

    public event ElapsedEventHandler Elapsed
    {
      add
      {
        this.timer.Elapsed += value;
      }
      remove
      {
        this.timer.Elapsed -= value;
      }
    }

    public void Start()
    {
      this.timer.Start();
    }

    public void Stop()
    {
      this.timer.Stop();
    }
  }
}
