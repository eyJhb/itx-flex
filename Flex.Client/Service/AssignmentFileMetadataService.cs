// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.AssignmentFileMetadataService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using Arcanic.ITX.Flex.WebserviceClient;
using Itx.Flex.Client.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Itx.Flex.Client.Service
{
  public class AssignmentFileMetadataService : IAssignmentFileMetadataService
  {
    private readonly IFlexClient _flexClient;
    private readonly IConfigurationService _configurationService;
    private readonly ILoggerService _loggerService;
    private readonly IFileService _fileService;
    private readonly IHashValidator _hashValidator;

    public AssignmentFileMetadataService(IFlexClient flexClient, IConfigurationService configurationService, ILoggerService loggerService, IFileService fileService, IHashValidator hashValidator)
    {
      this._flexClient = flexClient;
      this._configurationService = configurationService;
      this._loggerService = loggerService;
      this._fileService = fileService;
      this._hashValidator = hashValidator;
    }

    public IEnumerable<AssignmentFileMetadata> GetMetadata()
    {
      return this._flexClient.GetAssigments().Assignments.Select<AssignmentMetadata, AssignmentFileMetadata>((Func<AssignmentMetadata, AssignmentFileMetadata>) (af =>
      {
        return new AssignmentFileMetadata() { Filename = af.Filename, Hash = af.Hash, Url = af.Url };
      }));
    }

    public IEnumerable<DecryptedAssignmentFileMetadata> GetNotDownloadedAssignmentFileMetadatas(IEnumerable<DecryptedAssignmentFileMetadata> assignmentFileMetadatas)
    {
      List<DecryptedAssignmentFileMetadata> list = assignmentFileMetadatas.ToList<DecryptedAssignmentFileMetadata>();
      try
      {
        return (IEnumerable<DecryptedAssignmentFileMetadata>) list.Where<DecryptedAssignmentFileMetadata>((Func<DecryptedAssignmentFileMetadata, bool>) (m => !this.HasDownloaded(this._configurationService.GetEncryptedPath(m.Filename), m.Hash))).ToList<DecryptedAssignmentFileMetadata>();
      }
      catch (Exception ex)
      {
        this._loggerService.Log(LogType.Error, "Could not check if files are downloaded, all files are downloaded again. " + ex.Message, ex.StackTrace);
        return (IEnumerable<DecryptedAssignmentFileMetadata>) list;
      }
    }

    private bool HasDownloaded(string path, string hash)
    {
      if (!this._fileService.Exists(path))
        return false;
      using (FileStream fileData = this._fileService.OpenStream(path))
        return this._hashValidator.IsValidHash(fileData, hash);
    }
  }
}
