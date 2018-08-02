// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.RegistryService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Microsoft.Win32;

namespace Itx.Flex.Client.Service
{
  public class RegistryService : IRegistryService
  {
    private const string SoftwareKey = "Software";
    private const string CompanyKey = "Arcanic";
    private const string ApplicationKey = "ItxFlex";

    public void SetValue(string keyName, string value)
    {
      this.GetOrCreateAppKey()?.SetValue(keyName, (object) value);
    }

    private RegistryKey GetOrCreateAppKey()
    {
      return Registry.CurrentUser.OpenSubKey("Software", true)?.CreateSubKey("Arcanic")?.CreateSubKey("ItxFlex");
    }

    public string GetValue(string keyName)
    {
      return this.GetOrCreateAppKey().GetValue(keyName)?.ToString();
    }

    public void ClearValue(string keyName)
    {
      this.GetOrCreateAppKey().DeleteValue(keyName, false);
    }
  }
}
