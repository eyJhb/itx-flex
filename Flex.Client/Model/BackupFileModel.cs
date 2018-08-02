// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Model.BackupFileModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Itx.Flex.Client.Model
{
  public class BackupFileModel
  {
    public BackupFileModel(byte[] data, string hash, DateTime lastChanged, string path)
    {
      this.Data = data;
      this.Hash = hash;
      this.LastChanged = lastChanged;
      this.Path = path;
    }

    public byte[] Data { get; }

    public string Hash { get; }

    public DateTime LastChanged { get; }

    public string Path { get; }
  }
}
