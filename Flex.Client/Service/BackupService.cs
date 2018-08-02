// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.BackupService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using Arcanic.ITX.Flex.WebserviceClient;
using GalaSoft.MvvmLight.Messaging;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

namespace Itx.Flex.Client.Service
{
  public class BackupService : IBackupService
  {
    private readonly IFileService _fileService;
    private readonly IFlexClient _flexClient;
    private readonly IDirectoryService _directoryService;
    private readonly IHashProvider _hashProvider;
    private readonly ITimerService _backupIntervalTimer;
    private readonly ITimerService _noBackupAfterTimer;
    private readonly ILoggerService _loggerService;

    public BackupService(IFileService fileService, IConfigurationService configurationService, IFlexClient flexClient, IDirectoryService directoryService, IHashProvider hashProvider, ITimerService backupIntervalTimer, IMessenger messenger, ITimerService noBackupAfterTimer, ILoggerService loggerService)
    {
      this._fileService = fileService;
      this._flexClient = flexClient;
      this._directoryService = directoryService;
      this._hashProvider = hashProvider;
      this._backupIntervalTimer = backupIntervalTimer;
      this._noBackupAfterTimer = noBackupAfterTimer;
      this._loggerService = loggerService;
      this._backupIntervalTimer.Interval = (double) (configurationService.BackupIntervalInSeconds * 1000);
      this._backupIntervalTimer.AutoReset = true;
      this._noBackupAfterTimer.AutoReset = false;
      this._noBackupAfterTimer.Elapsed += (ElapsedEventHandler) ((sender, args) => this._backupIntervalTimer.Stop());
      this.MaxHandInFileSizeInBytes = (long) configurationService.MaxHandInFileSizeInBytes;
      messenger.Register<OnNewBackupSettings>((object) this, new Action<OnNewBackupSettings>(this.OnNewBackupSettings));
      messenger.Register<OnHandInFilesizeLimitLoaded>((object) this, new Action<OnHandInFilesizeLimitLoaded>(this.OnHandInFilesizeLimitLoaded));
    }

    private long MaxHandInFileSizeInBytes { get; set; }

    private void OnHandInFilesizeLimitLoaded(OnHandInFilesizeLimitLoaded onHandInFilesizeLimitLoaded)
    {
      this.MaxHandInFileSizeInBytes = (long) onHandInFilesizeLimitLoaded.MaxHandInFileSizeInBytes;
    }

    private void OnNewBackupSettings(OnNewBackupSettings onNewBackupSettings)
    {
      if (onNewBackupSettings.EnableBackup && onNewBackupSettings.NoBackupAfterSeconds > 0)
      {
        this.UpdateBackupInterval(onNewBackupSettings.BackupIntervalInSeconds);
        this._noBackupAfterTimer.Stop();
        this._noBackupAfterTimer.Interval = (double) (onNewBackupSettings.NoBackupAfterSeconds * 1000);
        this._noBackupAfterTimer.Start();
      }
      else
        this._backupIntervalTimer.Stop();
    }

    private void UpdateBackupInterval(int backupIntervalInSeconds)
    {
      int num = backupIntervalInSeconds * 1000;
      if ((int) this._backupIntervalTimer.Interval == num)
        return;
      this._backupIntervalTimer.Stop();
      this._backupIntervalTimer.Interval = (double) num;
      this._backupIntervalTimer.Start();
    }

    public void StartBackup(string handInPath)
    {
      this.MakeBackup(handInPath);
      this._backupIntervalTimer.Elapsed += (ElapsedEventHandler) ((sender, e) => this.MakeBackup(handInPath));
      this._backupIntervalTimer.Start();
    }

    private void MakeBackup(string handInPath)
    {
      long inFileSizeInBytes = this.MaxHandInFileSizeInBytes;
      List<BackupFileMetadata> metadatasForBackupFiles;
      try
      {
        metadatasForBackupFiles = this.GetMetadatasForBackupFiles(handInPath, inFileSizeInBytes);
      }
      catch (Exception ex)
      {
        this._loggerService.Log(LogType.Warning, "Backup could not be made: " + ex.Message, ex.StackTrace);
        return;
      }
      if (!metadatasForBackupFiles.Any<BackupFileMetadata>())
        return;
      BackupCurrentMetadataResponse metadataResponse = this._flexClient.BackupSendCurrentMetadata(new BackupCurrentMetadataRequest() { Files = metadatasForBackupFiles });
      if (!metadataResponse.MissingFiles.Any<BackupFileMetadata>())
        return;
      this.SendMissingBackupFiles(handInPath, (IEnumerable<BackupFileMetadata>) metadataResponse.MissingFiles, inFileSizeInBytes);
    }

    private void SendMissingBackupFiles(string handInPath, IEnumerable<BackupFileMetadata> missingFiles, long maximumFileSizeForFileInBytes)
    {
      foreach (BackupFileMetadata missingFile in missingFiles)
      {
        string filepath = Path.Combine(handInPath, missingFile.FilePath);
        BackupFileModel fileMetadataForFile = this.GetBackupFileMetadataForFile(missingFile.FilePath, filepath, maximumFileSizeForFileInBytes);
        if (fileMetadataForFile != null)
          this._flexClient.SendBackupFile(new BackupRequest()
          {
            Content = fileMetadataForFile.Data,
            Metadata = new BackupFileMetadata()
            {
              Hash = fileMetadataForFile.Hash,
              FilePath = fileMetadataForFile.Path,
              LastChanged = fileMetadataForFile.LastChanged
            }
          });
      }
    }

    private List<BackupFileMetadata> GetMetadatasForBackupFiles(string handInPath, long maximumFileSizeForFileInBytes)
    {
      IEnumerable<string> filenamesInDirectory = this._directoryService.GetFilenamesInDirectory(handInPath);
      List<BackupFileMetadata> backupFileMetadataList = new List<BackupFileMetadata>();
      foreach (string filepath in filenamesInDirectory)
      {
        BackupFileModel fileMetadataForFile = this.GetBackupFileMetadataForFile(filepath.Replace(handInPath + Path.DirectorySeparatorChar.ToString(), ""), filepath, maximumFileSizeForFileInBytes);
        if (fileMetadataForFile != null)
          backupFileMetadataList.Add(new BackupFileMetadata()
          {
            FilePath = fileMetadataForFile.Path,
            Hash = fileMetadataForFile.Hash,
            LastChanged = fileMetadataForFile.LastChanged
          });
      }
      return backupFileMetadataList;
    }

    private BackupFileModel GetBackupFileMetadataForFile(string filename, string filepath, long maximumFileSizeForFileInBytes)
    {
      if (this._fileService.GetSizeInBytes(filepath) > maximumFileSizeForFileInBytes)
        return (BackupFileModel) null;
      byte[] numArray = this._fileService.ReadAllBytesFromFile(filepath);
      string hashAsBase64 = this._hashProvider.ComputeHashAsBase64(numArray);
      DateTime lastModifiedUtc = this._fileService.GetLastModifiedUtc(filepath);
      return new BackupFileModel(numArray, hashAsBase64, lastModifiedUtc, filename);
    }
  }
}
