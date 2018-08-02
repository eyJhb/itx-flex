// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.PathService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System.IO;

namespace Itx.Flex.Client.Service
{
  public class PathService : IPathService
  {
    public string Combine(string path1, string path2)
    {
      return Path.Combine(path1, path2);
    }

    public string GetTempPath()
    {
      return Path.GetTempPath();
    }

    public string GetDirectoryName(string path)
    {
      return Path.GetDirectoryName(path);
    }

    public string GetFileName(string filePath)
    {
      return Path.GetFileName(filePath);
    }

    public string GetFileNameWithoutExtension(string filePath)
    {
      return Path.GetFileNameWithoutExtension(filePath);
    }

    public string GetExtension(string filePath)
    {
      string extension = Path.GetExtension(filePath);
      if (string.IsNullOrEmpty(extension))
        return extension;
      return extension.Substring(1);
    }
  }
}
