// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.GlobalLogService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using Arcanic.ITX.Flex.WebserviceClient;
using System;

namespace Itx.Flex.Client.Service
{
  public class GlobalLogService : IGlobalLogService
  {
    private readonly IFlexClient _flexClient;

    public GlobalLogService(IFlexClient flexClient)
    {
      this._flexClient = flexClient;
    }

    public void SendDump(string message, string boardingPass = null)
    {
      this._flexClient.SendCrashReport(new LogDumpRequest()
      {
        BoardingPass = boardingPass,
        ClientTimestamp = DateTime.Now,
        LogTextDump = message
      });
    }
  }
}
