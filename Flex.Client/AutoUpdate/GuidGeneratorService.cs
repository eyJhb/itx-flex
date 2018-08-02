// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.AutoUpdate.GuidGeneratorService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Itx.Flex.Client.AutoUpdate
{
  public class GuidGeneratorService : IGuidGeneratorService
  {
    public Guid NewGuid()
    {
      return Guid.NewGuid();
    }
  }
}
