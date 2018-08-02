// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.FileService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.Collections.Generic;
using System.IO;

namespace Itx.Flex.Client.Service
{
  public class FileService : IFileService
  {
    public void WriteToFile(string filepath, byte[] fileData)
    {
      File.WriteAllBytes(filepath, fileData);
    }

    public void CopyFile(string sourcePath, string destinationPath, bool overwrite)
    {
      File.Copy(sourcePath, destinationPath, overwrite);
    }

    public IEnumerable<string> ReadLinesFromFile(string path)
    {
      return (IEnumerable<string>) File.ReadAllLines(path);
    }

    public byte[] ReadAllBytesFromFile(string path)
    {
      byte[] buffer = (byte[]) null;
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        buffer = new byte[fileStream.Length];
        fileStream.Read(buffer, 0, (int) fileStream.Length);
      }
      return buffer;
    }

    public FileStream OpenStream(string path)
    {
      return File.OpenRead(path);
    }

    public bool Exists(string path)
    {
      return File.Exists(path);
    }

    public void Delete(string path)
    {
      File.Delete(path);
    }

    public DateTime GetLastModifiedUtc(string path)
    {
      return File.GetLastWriteTimeUtc(path);
    }

    public long GetSizeInBytes(string path)
    {
      return new FileInfo(path).Length;
    }

    public void Append(string path, string message)
    {
      File.AppendAllText(path, message);
    }
  }
}
