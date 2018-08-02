// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.ServerLogService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Flex.WebserviceClient;
using System;

namespace Itx.Flex.Client.Service
{
  public class ServerLogService : IServerLogService
  {
    private readonly IFlexClient _flexClient;
    private readonly IDateTimeService _dateTimeService;

    public ServerLogService(IFlexClient flexClient, IDateTimeService dateTimeService)
    {
      this._flexClient = flexClient;
      this._dateTimeService = dateTimeService;
    }

    public void ClosedWithoutConfirmation()
    {
      try
      {
        this._flexClient.LogClientClosedByUserWithoutConfirmation(this._dateTimeService.LocalTime);
      }
      catch (Exception ex)
      {
      }
    }

    public void ClosedAfterConfirmation()
    {
      try
      {
        this._flexClient.LogClientClosedByUserAfterConfirmation(this._dateTimeService.LocalTime);
      }
      catch (Exception ex)
      {
      }
    }
  }
}
