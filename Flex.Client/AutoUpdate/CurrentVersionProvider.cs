// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.AutoUpdate.CurrentVersionProvider
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.Diagnostics;
using System.Reflection;

namespace Itx.Flex.Client.AutoUpdate
{
  public class CurrentVersionProvider : ICurrentVersionProvider
  {
    public Version Version
    {
      get
      {
        return this.GetVersion();
      }
    }

    private Version GetVersion()
    {
      return new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
    }
  }
}
