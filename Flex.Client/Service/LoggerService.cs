// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.LoggerService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Itx.Flex.Client.Service
{
  public class LoggerService : ILoggerService
  {
    private readonly IFileService _fileService;
    private readonly IConfigurationService _configurationService;
    private readonly IDateTimeService _dateTimeService;

    public LoggerService(IFileService fileService, IConfigurationService configurationService, IDateTimeService dateTimeService)
    {
      this._fileService = fileService;
      this._configurationService = configurationService;
      this._dateTimeService = dateTimeService;
    }

    public void Log(LogType logType, string errorMessage, string stackTrace = null)
    {
      if (logType <= this._configurationService.LoggingLevel)
        return;
      this._fileService.Append(this._configurationService.LogPath, logType.ToString().ToUpper() + ": " + (object) this._dateTimeService.LocalTime + " - " + errorMessage + Environment.NewLine + stackTrace + Environment.NewLine);
    }

    public void Log(string message, Exception exception)
    {
      string stackTrace = "";
      string str = "";
      for (Exception exception1 = exception; exception1 != null; exception1 = exception1.InnerException)
      {
        stackTrace = stackTrace + exception1.StackTrace + Environment.NewLine;
        str = str + exception1.Message + Environment.NewLine;
      }
      this.Log(LogType.Error, message + Environment.NewLine + str, stackTrace);
    }
  }
}
