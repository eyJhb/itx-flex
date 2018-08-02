// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.PinCodeStorageService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

namespace Itx.Flex.Client.Service
{
  public class PinCodeStorageService : IPinCodeStorageService
  {
    private readonly IRegistryService registryService;
    private const string PinCodeKey = "PinCode";

    public PinCodeStorageService(IRegistryService registryService)
    {
      this.registryService = registryService;
    }

    public bool HasExisting()
    {
      int result;
      return int.TryParse(this.registryService.GetValue("PinCode"), out result);
    }

    public string GetExisting()
    {
      return this.registryService.GetValue("PinCode");
    }

    public void Store(string pinCode)
    {
      this.registryService.SetValue("PinCode", pinCode);
    }

    public void Clear()
    {
      this.registryService.ClearValue("PinCode");
    }
  }
}
