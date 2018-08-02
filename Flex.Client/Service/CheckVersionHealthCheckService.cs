// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.CheckVersionHealthCheckService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using Itx.Flex.Client.AutoUpdate;
using Itx.Flex.Client.Model;
using System;

namespace Itx.Flex.Client.Service
{
  public class CheckVersionHealthCheckService : IHealthCheckService
  {
    private readonly IIsCurrentVersionProvider _isCurrentVersionProvider;
    private readonly IMessenger messenger;

    public CheckVersionHealthCheckService(IIsCurrentVersionProvider isCurrentVersionProvider, IMessenger messenger)
    {
      this._isCurrentVersionProvider = isCurrentVersionProvider;
      this.messenger = messenger;
    }

    public HealthCheckStatus Check()
    {
      HealthCheckStatus healthCheckStatus1 = new HealthCheckStatus() { DescriptionKey = "HealthCheckVersionHealthOkText", ImageSource = "..\\Resources\\okCheckMark.png", CanContinue = true };
      HealthCheckStatus healthCheckStatus2 = new HealthCheckStatus() { DescriptionKey = "HealthCheckVersionHealthErrorText", ImageSource = "..\\Resources\\errorCheckMark.png", CanContinue = false, ReadMoreKey = "HealthCheckVersionHealthFullDescriptionText" };
      try
      {
        if (this._isCurrentVersionProvider.IsCurrentVersion())
          return healthCheckStatus1;
        healthCheckStatus2.MustUpdate = true;
        return healthCheckStatus2;
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
        return 20;
      }
    }
  }
}
