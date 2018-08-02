// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.CheckConnectionHealthCheckService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using Arcanic.ITX.Flex.WebserviceClient;
using Itx.Flex.Client.Model;
using System;

namespace Itx.Flex.Client.Service
{
  public class CheckConnectionHealthCheckService : IHealthCheckService
  {
    private readonly IConfigurationService _configurationService;
    private readonly IFlexClient _flexClient;

    public CheckConnectionHealthCheckService(IConfigurationService configurationService, IFlexClient flexClient)
    {
      this._configurationService = configurationService;
      this._flexClient = flexClient;
    }

    public HealthCheckStatus Check()
    {
      HealthCheckStatus healthCheckStatus1 = new HealthCheckStatus() { DescriptionKey = "HealthCheckConnectionHealthOkText", ImageSource = "..\\Resources\\okCheckMark.png", CanContinue = true };
      HealthCheckStatus healthCheckStatus2 = new HealthCheckStatus() { DescriptionKey = "HealthCheckConnectionHealthErrorText", ImageSource = "..\\Resources\\errorCheckMark.png", CanContinue = false, ReadMoreKey = "HealthCheckConnectionHealthFullDescriptionText" };
      try
      {
        if (this._configurationService.GlobalResponse != null)
          return healthCheckStatus1;
        GlobalResponse globalSettings = this._flexClient.GetGlobalSettings();
        this._configurationService.GlobalResponse = globalSettings;
        return globalSettings != null ? healthCheckStatus1 : healthCheckStatus2;
      }
      catch (Exception ex)
      {
        return healthCheckStatus2;
      }
    }

    public int RunOrder
    {
      get
      {
        return 10;
      }
    }
  }
}
