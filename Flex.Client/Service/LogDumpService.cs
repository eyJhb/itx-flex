// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.LogDumpService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.Linq;

namespace Itx.Flex.Client.Service
{
  public class LogDumpService : ILogDumpService
  {
    private readonly IBoardingPassStorageService _boardingPassStorageService;
    private readonly IFileService _fileService;
    private readonly IConfigurationService _configurationService;
    private readonly IGlobalLogService _globalLogService;

    public LogDumpService(IBoardingPassStorageService boardingPassStorageService, IFileService fileService, IConfigurationService configurationService, IGlobalLogService globalLogService)
    {
      this._boardingPassStorageService = boardingPassStorageService;
      this._fileService = fileService;
      this._configurationService = configurationService;
      this._globalLogService = globalLogService;
    }

    public void DumpLog()
    {
      try
      {
        if (!this._fileService.Exists(this._configurationService.LogPath))
          return;
        this._globalLogService.SendDump(string.Join("\r\n", this._fileService.ReadLinesFromFile(this._configurationService.LogPath).ToArray<string>()), this._boardingPassStorageService.HasExisting() ? this._boardingPassStorageService.GetExisting() : (string) null);
        this._fileService.Delete(this._configurationService.LogPath);
      }
      catch (Exception ex)
      {
      }
    }
  }
}
