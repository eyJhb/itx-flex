// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.DateTimeService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Grabber.Core.Providers;
using System;
using System.Diagnostics;

namespace Itx.Flex.Client.Service
{
  public class DateTimeService : IDateTimeService, IGrabTimeService
  {
    private readonly object _serverTimeLock = new object();
    private readonly Stopwatch _stopwatch;
    private DateTime _serverTime;

    public DateTime LocalTime
    {
      get
      {
        return DateTime.Now;
      }
    }

    public DateTimeService(Stopwatch stopwatch)
    {
      this._stopwatch = stopwatch;
    }

    public DateTime ServerTime
    {
      get
      {
        lock (this._serverTimeLock)
          return this._serverTime + this._stopwatch.Elapsed;
      }
    }

    public void UpdateServerTime(DateTime serverTime)
    {
      lock (this._serverTimeLock)
      {
        this._stopwatch.Reset();
        this._serverTime = serverTime;
        this._stopwatch.Start();
      }
    }

    public DateTime GrabTime
    {
      get
      {
        return this.ServerTime;
      }
    }
  }
}
