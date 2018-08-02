// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.CompositeHealthCheckService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Itx.Flex.Client.Service
{
  public class CompositeHealthCheckService : ICompositeHealthCheckService
  {
    private readonly IEnumerable<IHealthCheckService> healthChecks;

    public CompositeHealthCheckService(IEnumerable<IHealthCheckService> healthChecks)
    {
      this.healthChecks = healthChecks;
    }

    public OverallHealthCheckStatus Check()
    {
      OverallHealthCheckStatus healthCheckStatus1 = new OverallHealthCheckStatus();
      List<HealthCheckStatus> source = new List<HealthCheckStatus>();
      bool flag1 = true;
      bool flag2 = false;
      foreach (IHealthCheckService healthCheckService in (IEnumerable<IHealthCheckService>) this.healthChecks.OrderBy<IHealthCheckService, int>((Func<IHealthCheckService, int>) (hc => hc.RunOrder)))
      {
        HealthCheckStatus healthCheckStatus2 = healthCheckService.Check();
        source.Add(healthCheckStatus2);
        flag1 = healthCheckStatus2.CanContinue & flag1;
        flag2 = healthCheckStatus2.MustUpdate | flag2;
      }
      List<HealthCheckStatus> list = source.ToList<HealthCheckStatus>();
      healthCheckStatus1.HealthCheckStatuses = (IEnumerable<HealthCheckStatus>) list;
      healthCheckStatus1.CanContinue = flag1;
      healthCheckStatus1.MustUpdate = flag2;
      string str1 = "HealthCheckOverallStatusErrorText";
      string str2 = "HealthCheckOverallStatusOkText";
      healthCheckStatus1.OverallStatusTextKey = flag1 ? str2 : str1;
      return healthCheckStatus1;
    }
  }
}
