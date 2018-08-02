// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.DirectoryService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System.Collections.Generic;
using System.IO;

namespace Itx.Flex.Client.Service
{
  public class DirectoryService : IDirectoryService
  {
    public void CreateDirectory(string directoryPath)
    {
      Directory.CreateDirectory(directoryPath);
    }

    public IEnumerable<string> GetFilenamesInDirectory(string directoryPath)
    {
      return (IEnumerable<string>) Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);
    }

    public bool Exists(string directoryPath)
    {
      return Directory.Exists(directoryPath);
    }

    public void Delete(string directoryPath)
    {
      Directory.Delete(directoryPath, true);
    }
  }
}
