// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.DirectoryCreationValidator
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.IO;

namespace Itx.Flex.Client.Service
{
  public class DirectoryCreationValidator : IDirectoryAccessValidator
  {
    private readonly IFileService _fileService;
    private const int MaxPathLength = 120;

    public DirectoryCreationValidator(IFileService fileService)
    {
      this._fileService = fileService;
    }

    public bool HasWriteAccess(string directoryPath)
    {
      Guid guid = Guid.NewGuid();
      string str = Path.Combine(directoryPath, guid.ToString() + ".test");
      try
      {
        this._fileService.WriteToFile(str, new byte[0]);
        this._fileService.Delete(str);
        return true;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public int GetMaxPathLength()
    {
      return 120;
    }

    public bool IsPathLengthAcceptable(string directoryPath)
    {
      return directoryPath.Length < 120;
    }
  }
}
