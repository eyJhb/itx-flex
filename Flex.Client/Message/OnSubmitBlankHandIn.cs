﻿// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Message.OnSubmitBlankHandIn
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;

namespace Itx.Flex.Client.Message
{
  public class OnSubmitBlankHandIn
  {
    public HandInStatus HandInStatus { get; }

    public OnSubmitBlankHandIn(HandInStatus handInStatus)
    {
      this.HandInStatus = handInStatus;
    }
  }
}
