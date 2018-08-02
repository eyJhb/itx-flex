// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.HandInFieldIdStorageService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Itx.Flex.Client.Service
{
  public class HandInFieldIdStorageService : IHandInFieldIdStorageService
  {
    private readonly IRegistryService _registryService;
    private const string HandInFieldIdKey = "HandInFieldId";

    public HandInFieldIdStorageService(IRegistryService registryService)
    {
      this._registryService = registryService;
    }

    public void Store(Guid id)
    {
      this._registryService.SetValue("HandInFieldId", id.ToString());
    }

    public Guid? Get()
    {
      string g = this._registryService.GetValue("HandInFieldId");
      if (g == null)
        return new Guid?();
      return new Guid?(new Guid(g));
    }

    public void Clear()
    {
      this._registryService.ClearValue("HandInFieldId");
    }
  }
}
