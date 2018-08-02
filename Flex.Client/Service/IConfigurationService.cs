// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.IConfigurationService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;

namespace Itx.Flex.Client.Service
{
  public interface IConfigurationService
  {
    GlobalResponse GlobalResponse { get; set; }

    string TemporaryAssignmentFilesPath { get; }

    string TemporaryFolderPath { get; }

    string EnGbLanguageFilePath { get; }

    string DaDkLanguageFilePath { get; }

    int BackupIntervalInSeconds { get; }

    int BoardingPassLoginAttemptsBeforeLocking { get; }

    int BoardingPassLoginLockDurationInSeconds { get; }

    LogType LoggingLevel { get; }

    int LastSaveUpdateIntervalInSeconds { get; }

    int HealthCheckRetryIntervalInSeconds { get; }

    string GetEncryptedPath(string filename);

    string OfficialVersionName { get; }

    string TemporaryHandInPath { get; }

    string LogPath { get; }

    int MaxHandInFileSizeInBytes { get; }
  }
}
