// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Message.OnNewGrabSettings
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

namespace Itx.Flex.Client.Message
{
  public class OnNewGrabSettings
  {
    public bool EnableGrabs { get; }

    public int NoGrabsAfterSeconds { get; }

    public int TimeUntilStartGrabInSeconds { get; }

    public OnNewGrabSettings(bool enableGrabs, int noGrabsAfterSeconds, int timeUntilStartGrabInSeconds)
    {
      this.EnableGrabs = enableGrabs;
      this.NoGrabsAfterSeconds = noGrabsAfterSeconds;
      this.TimeUntilStartGrabInSeconds = timeUntilStartGrabInSeconds;
    }
  }
}
