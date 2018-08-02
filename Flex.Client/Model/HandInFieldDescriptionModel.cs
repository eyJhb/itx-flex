// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Model.HandInFieldDescriptionModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Itx.Flex.Client.Model
{
  public class HandInFieldDescriptionModel
  {
    public Guid Id { get; set; }

    public string TextDaDk { get; set; }

    public string TextEnGb { get; set; }

    public string DescriptionDaDk { get; set; }

    public string DescriptionEnGb { get; set; }

    public HandInFieldValueType ValueType { get; set; }

    public bool Required { get; set; }

    public string RegexValidation { get; set; }
  }
}
