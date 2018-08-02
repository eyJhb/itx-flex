// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.ConfigurationService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using System;

namespace Itx.Flex.Client.Service
{
  public class ConfigurationService : IConfigurationService
  {
    private static readonly string GlobalEndpointFromBuild = "https://itxflex.arcanic.dk";
    private static readonly string StageTextFromBuild = "";
    private static readonly string OfficialVersionNameFromCustomer = "v2.0";

    public string OfficialVersionName
    {
      get
      {
        if (string.IsNullOrEmpty(ConfigurationService.StageTextFromBuild))
          return ConfigurationService.OfficialVersionNameFromCustomer;
        return string.Format("{0} - {1}", (object) ConfigurationService.OfficialVersionNameFromCustomer, (object) ConfigurationService.StageTextFromBuild);
      }
    }

    public static string GlobalEndpoint
    {
      get
      {
        if (!string.IsNullOrEmpty(ConfigurationService.GlobalEndpointFromBuild))
          return ConfigurationService.GlobalEndpointFromBuild;
        return "https://itxflexapi.arcanic.dk/";
      }
    }

    public string TemporaryFolderPath
    {
      get
      {
        return AppDomain.CurrentDomain.BaseDirectory;
      }
    }

    public string TemporaryAssignmentFilesPath
    {
      get
      {
        return this.TemporaryFolderPath + "\\AssignmentFiles\\";
      }
    }

    public string EncryptedFileSuffix
    {
      get
      {
        return ".encrypted";
      }
    }

    public string EnGbLanguageFilePath
    {
      get
      {
        return this.TemporaryFolderPath + "\\Language\\EnGBLanguage.strings";
      }
    }

    public string DaDkLanguageFilePath
    {
      get
      {
        return this.TemporaryFolderPath + "\\Language\\DaDKLanguage.strings";
      }
    }

    public int BackupIntervalInSeconds
    {
      get
      {
        return 300;
      }
    }

    public int BoardingPassLoginAttemptsBeforeLocking
    {
      get
      {
        return 3;
      }
    }

    public int BoardingPassLoginLockDurationInSeconds
    {
      get
      {
        return 10;
      }
    }

    public GlobalResponse GlobalResponse { get; set; }

    public LogType LoggingLevel
    {
      get
      {
        return LogType.Info;
      }
    }

    public int LastSaveUpdateIntervalInSeconds
    {
      get
      {
        return 30;
      }
    }

    public int HealthCheckRetryIntervalInSeconds
    {
      get
      {
        return 10;
      }
    }

    public string GetEncryptedPath(string filename)
    {
      return this.TemporaryAssignmentFilesPath + filename + this.EncryptedFileSuffix;
    }

    public string LogPath
    {
      get
      {
        return this.TemporaryFolderPath + "\\log.txt";
      }
    }

    public int MaxHandInFileSizeInBytes
    {
      get
      {
        return 20971520;
      }
    }

    public string TemporaryHandInPath
    {
      get
      {
        return this.TemporaryFolderPath + "\\HandIn\\";
      }
    }
  }
}
