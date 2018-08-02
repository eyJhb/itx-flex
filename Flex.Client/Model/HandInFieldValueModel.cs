// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Model.HandInFieldValueModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Itx.Flex.Client.Model
{
  public class HandInFieldValueModel
  {
    public HandInFieldValueModel(Guid id, string value)
    {
      this.Id = id;
      this.Value = value;
    }

    public Guid Id { get; }

    public string Value { get; }
  }
}
