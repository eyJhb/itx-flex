// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.ISelectFilesService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System.Collections.Generic;

namespace Itx.Flex.Client.Service
{
  public interface ISelectFilesService
  {
    FilesResult GetSelectedFile(string initialDirectory, string fileExtensionValidText, IEnumerable<string> fileExtensions);

    FilesResult GetSelectedFilesNoFilter(string initialDirectory);
  }
}
