// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.LastFileSaveService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace Itx.Flex.Client.Service
{
  public class LastFileSaveService : ILastFileSaveService
  {
    private readonly IDirectoryService _directoryService;
    private readonly IFileService _fileService;
    private readonly IDateTimeService _dateTimeService;

    public LastFileSaveService(IDirectoryService directoryService, IFileService fileService, IDateTimeService dateTimeService)
    {
      this._directoryService = directoryService;
      this._fileService = fileService;
      this._dateTimeService = dateTimeService;
    }

    public bool AnyFilesInPath(string handInPath)
    {
      try
      {
        return this._directoryService.GetFilenamesInDirectory(handInPath).Any<string>();
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public int GetTimeSinceLastSaveInMinutes(string handInPath)
    {
      IEnumerable<string> filenamesInDirectory = this._directoryService.GetFilenamesInDirectory(handInPath);
      DateTime dateTime = DateTime.MinValue;
      foreach (string path in filenamesInDirectory)
      {
        DateTime lastModifiedUtc = this._fileService.GetLastModifiedUtc(path);
        if (lastModifiedUtc > dateTime)
          dateTime = lastModifiedUtc;
      }
      return (int) (this._dateTimeService.LocalTime.ToUniversalTime() - dateTime).TotalMinutes;
    }
  }
}
