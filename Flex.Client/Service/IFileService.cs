// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.IFileService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.Collections.Generic;
using System.IO;

namespace Itx.Flex.Client.Service
{
  public interface IFileService
  {
    void WriteToFile(string filepath, byte[] fileData);

    void CopyFile(string sourcePath, string destinationPath, bool overwrite);

    IEnumerable<string> ReadLinesFromFile(string path);

    byte[] ReadAllBytesFromFile(string path);

    FileStream OpenStream(string path);

    bool Exists(string path);

    void Delete(string path);

    DateTime GetLastModifiedUtc(string path);

    long GetSizeInBytes(string path);

    void Append(string path, string message);
  }
}
