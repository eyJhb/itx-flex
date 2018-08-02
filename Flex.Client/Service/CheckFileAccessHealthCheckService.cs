// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.CheckFileAccessHealthCheckService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;
using System;
using System.IO;

namespace Itx.Flex.Client.Service
{
  public class CheckFileAccessHealthCheckService : IHealthCheckService
  {
    private readonly IConfigurationService configurationService;
    private readonly IFileService fileService;
    private readonly IDriveService _driveService;

    public CheckFileAccessHealthCheckService(IConfigurationService configurationService, IFileService fileService, IDriveService driveService)
    {
      this.configurationService = configurationService;
      this.fileService = fileService;
      this._driveService = driveService;
    }

    public HealthCheckStatus Check()
    {
      HealthCheckStatus healthCheckStatus1 = new HealthCheckStatus() { DescriptionKey = "HealthCheckFileAccessHealthOkText", ImageSource = "..\\Resources\\okCheckMark.png", CanContinue = true };
      HealthCheckStatus healthCheckStatus2 = new HealthCheckStatus() { DescriptionKey = "HealthCheckFileAccessHealthErrorText", ImageSource = "..\\Resources\\errorCheckMark.png", CanContinue = false, ReadMoreKey = "HealthCheckFileAccessHealthFullDescriptionText" };
      string temporaryFolderPath = this.configurationService.TemporaryFolderPath;
      try
      {
        if (!this._driveService.HasRequiredAvailableDiskSpace(Path.GetPathRoot(temporaryFolderPath), this.configurationService.GlobalResponse.Configuration.MinimumDiskSpaceInMegabytes))
          return healthCheckStatus2;
        this.fileService.WriteToFile(temporaryFolderPath + "\\file.temp", new byte[8]);
        return healthCheckStatus1;
      }
      catch (Exception ex)
      {
        return healthCheckStatus2;
      }
    }

    public int RunOrder
    {
      get
      {
        return 30;
      }
    }
  }
}
