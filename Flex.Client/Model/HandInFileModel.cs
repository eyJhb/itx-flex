// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Model.HandInFileModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

namespace Itx.Flex.Client.Model
{
  public class HandInFileModel
  {
    public HandInFileModel(string name, string path)
    {
      this.Name = name;
      this.Path = path;
    }

    public string Name { get; }

    public string Path { get; }
  }
}
