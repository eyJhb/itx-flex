// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.DirectoryResult
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

namespace Itx.Flex.Client.Service
{
  public class DirectoryResult
  {
    public static DirectoryResult CreateSelectedDirectoryResult(string path)
    {
      return new DirectoryResult(true, path);
    }

    public static DirectoryResult CreateCancelledResult()
    {
      return new DirectoryResult(false, (string) null);
    }

    private DirectoryResult(bool hasSelected, string path = null)
    {
      this.HasSelected = hasSelected;
      this.Path = path;
    }

    public bool HasSelected { get; }

    public string Path { get; }
  }
}
