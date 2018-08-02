// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.AutoUpdate.UpdaterProvider
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Service;
using System;
using System.Diagnostics;
using System.Threading;

namespace Itx.Flex.Client.AutoUpdate
{
  public class UpdaterProvider : IUpdater
  {
    private readonly IApplicationExitProvider _applicationExitProvider;
    private readonly IFileService _fileService;
    private readonly IDirectoryService _directoryService;
    private readonly IWebClientService _webClientService;
    private readonly IPathService _pathService;
    private readonly IAssemblyService _assemblyService;
    private readonly IProcessStarterService _processStarterService;
    private readonly IGuidGeneratorService _guidGeneratorService;
    private readonly IIsCurrentVersionProvider _isCurrentVersionProvider;
    private readonly IGlobalLogService _globalLogService;

    public UpdaterProvider(IApplicationExitProvider applicationExitProvider, IFileService fileService, IDirectoryService directoryService, IWebClientService webClientService, IPathService pathService, IAssemblyService assemblyService, IProcessStarterService processStarterService, IGuidGeneratorService guidGeneratorService, IIsCurrentVersionProvider isCurrentVersionProvider, IGlobalLogService globalLogService)
    {
      this._applicationExitProvider = applicationExitProvider;
      this._fileService = fileService;
      this._directoryService = directoryService;
      this._webClientService = webClientService;
      this._pathService = pathService;
      this._assemblyService = assemblyService;
      this._processStarterService = processStarterService;
      this._guidGeneratorService = guidGeneratorService;
      this._isCurrentVersionProvider = isCurrentVersionProvider;
      this._globalLogService = globalLogService;
    }

    private string TemporaryDirectory
    {
      get
      {
        return this._pathService.Combine(this._pathService.GetTempPath(), "FlexUpdate");
      }
    }

    private void UpdateIfNeeded(string urlToNewClient)
    {
      if (this._isCurrentVersionProvider.IsCurrentVersion())
        return;
      Thread.Sleep(1000);
      this._directoryService.CreateDirectory(this.TemporaryDirectory);
      string tempFolder1 = this.DownloadMsiToTempFolder(urlToNewClient);
      string tempFolder2 = this.CopyUpdateApplicationToTempFolder();
      string str = string.Format("update \"{0}\"", (object) tempFolder1);
      ProcessStartInfo processStartInfo = new ProcessStartInfo() { FileName = tempFolder2, Arguments = str };
      try
      {
        this._globalLogService.SendDump(string.Format("Update downloaded. Starting process: Filename {0}, Arguments {1}, TempMsiPath {2}.", (object) tempFolder2, (object) str, (object) tempFolder1), (string) null);
      }
      catch (Exception ex)
      {
      }
      this._processStarterService.Start(processStartInfo);
      this._applicationExitProvider.Exit();
    }

    private string DownloadMsiToTempFolder(string url)
    {
      string str = this.TemporaryDirectory + "\\installer_" + (object) this._guidGeneratorService.NewGuid() + (url.ToLower().Contains(".msi") ? ".msi" : ".exe");
      Uri uri = new Uri(url);
      if (uri.Scheme == "file")
        this._fileService.CopyFile(url, str, true);
      else
        this._webClientService.DownloadFile(uri.ToString(), str);
      return str;
    }

    private string CopyUpdateApplicationToTempFolder()
    {
      string directoryName = this._pathService.GetDirectoryName(this._assemblyService.GetExecutingAssembly().Location);
      string str1 = this.TemporaryDirectory + "\\update_" + (object) this._guidGeneratorService.NewGuid();
      this._directoryService.CreateDirectory(str1);
      this._directoryService.CreateDirectory(this._pathService.Combine(str1, "Language"));
      foreach (string str2 in this._directoryService.GetFilenamesInDirectory(directoryName))
        this._fileService.CopyFile(str2, this._pathService.Combine(str1, this._pathService.GetFileName(str2)), true);
      foreach (string str2 in this._directoryService.GetFilenamesInDirectory(this._pathService.Combine(directoryName, "Language")))
        this._fileService.CopyFile(str2, this._pathService.Combine(this._pathService.Combine(str1, "Language"), this._pathService.GetFileName(str2)), true);
      return this._pathService.Combine(str1, "Flex.Updater.exe");
    }

    private void Clean()
    {
      try
      {
        if (!this._directoryService.Exists(this.TemporaryDirectory))
          return;
        foreach (string path in this._directoryService.GetFilenamesInDirectory(this.TemporaryDirectory))
        {
          try
          {
            this._fileService.Delete(path);
          }
          catch
          {
          }
        }
      }
      catch
      {
      }
    }

    public void Update(string urlToNewClient)
    {
      this.Clean();
      this.UpdateIfNeeded(urlToNewClient);
    }
  }
}
