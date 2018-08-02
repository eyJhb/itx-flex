// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.AutoUpdate.IsCurrentVersionProvider
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Service;
using System;

namespace Itx.Flex.Client.AutoUpdate
{
  public class IsCurrentVersionProvider : IIsCurrentVersionProvider
  {
    private readonly ICurrentVersionProvider _currentVersionProvider;
    private readonly IConfigurationService _configurationService;

    public IsCurrentVersionProvider(ICurrentVersionProvider currentVersionProvider, IConfigurationService configurationService)
    {
      this._currentVersionProvider = currentVersionProvider;
      this._configurationService = configurationService;
    }

    public bool IsCurrentVersion()
    {
      Version version1 = new Version(this._configurationService.GlobalResponse.VersionInfo.WindowsClientVersion);
      Version version2 = this._currentVersionProvider.Version;
      if (version2.Major > version1.Major)
        return true;
      if (version2.Major == version1.Major)
        return version2.Minor >= version1.Minor;
      return false;
    }
  }
}
