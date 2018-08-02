// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Message.OnExaminationDataLoaded
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Itx.Flex.Client.Message
{
  public class OnExaminationDataLoaded
  {
    public string TitleDaDk { get; }

    public string TitleEnGb { get; }

    public DateTime Start { get; }

    public DateTime End { get; }

    public string Username { get; }

    public int EarliestLoginTimeBeforeStartInMinutes { get; }

    public OnExaminationDataLoaded(string titleDaDk, string titleEnGb, DateTime start, DateTime end, string username, int earliestLoginTimeBeforeStartInMinutes)
    {
      this.TitleDaDk = titleDaDk;
      this.TitleEnGb = titleEnGb;
      this.Start = start;
      this.End = end;
      this.Username = username;
      this.EarliestLoginTimeBeforeStartInMinutes = earliestLoginTimeBeforeStartInMinutes;
    }
  }
}
