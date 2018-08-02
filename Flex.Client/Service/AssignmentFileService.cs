// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.AssignmentFileService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using Arcanic.ITX.Flex.WebserviceClient;
using Itx.Flex.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Itx.Flex.Client.Service
{
  public class AssignmentFileService : IAssignmentFileService
  {
    private readonly IAssignmentFileMetadataService _assignmentFileMetadataService;
    private readonly IDecryptionService _decryptionService;
    private readonly IAssignmentFileStorageService _assignmentFileStorageService;
    private readonly IFlexClient _flexClient;
    private readonly ILoggerService _loggerService;

    public AssignmentFileService(IAssignmentFileMetadataService assignmentFileMetadataService, IDecryptionService decryptionService, IAssignmentFileStorageService assignmentFileStorageService, IFlexClient flexClient, ILoggerService loggerService)
    {
      this._assignmentFileMetadataService = assignmentFileMetadataService;
      this._decryptionService = decryptionService;
      this._assignmentFileStorageService = assignmentFileStorageService;
      this._flexClient = flexClient;
      this._loggerService = loggerService;
    }

    public IEnumerable<AssignmentFileMetadata> SetupAssignmentFileMetadatas()
    {
      for (int index = 0; index < 3; ++index)
      {
        List<AssignmentFileMetadata> list = this._assignmentFileMetadataService.GetMetadata().ToList<AssignmentFileMetadata>();
        FileMetadataStatus temporaryDirectory = this._assignmentFileStorageService.DownloadAssignmentFilesToTemporaryDirectory(this._assignmentFileMetadataService.GetNotDownloadedAssignmentFileMetadatas(list.Cast<DecryptedAssignmentFileMetadata>()).ToList<DecryptedAssignmentFileMetadata>().Cast<AssignmentFileMetadata>());
        FileMetadataStatus fileMetadataStatus = this._decryptionService.ValidateEncryptedAssignmentFiles(temporaryDirectory.Successful);
        if (!temporaryDirectory.Failed.Any<AssignmentFileMetadata>() && !fileMetadataStatus.Failed.Any<AssignmentFileMetadata>())
          return (IEnumerable<AssignmentFileMetadata>) list;
      }
      throw new Exception("Could not download AssignmentFiles");
    }

    public IEnumerable<AssignmentFileMetadata> DownloadAnyNewAssignmentFiles(IEnumerable<AssignmentDecryptionKeyModel> decryptionKeys)
    {
      IEnumerable<DecryptedAssignmentFileMetadata> assignmentFileMetadatas = this._assignmentFileMetadataService.GetNotDownloadedAssignmentFileMetadatas(decryptionKeys.Select<AssignmentDecryptionKeyModel, DecryptedAssignmentFileMetadata>((Func<AssignmentDecryptionKeyModel, DecryptedAssignmentFileMetadata>) (dk => new DecryptedAssignmentFileMetadata() { Filename = dk.Filename, Hash = dk.CiphertextHash })));
      try
      {
        return assignmentFileMetadatas.Any<DecryptedAssignmentFileMetadata>() ? this.SetupAssignmentFileMetadatas() : (IEnumerable<AssignmentFileMetadata>) null;
      }
      catch (Exception ex)
      {
        this._loggerService.Log("Failed redownloading new AssignmentFileMetadata", ex);
        throw new DownloadFailedException("Failed redownloading new AssignmentFileMetadata", ex);
      }
    }

    public void MoveDecryptedAssignmentFilesToWorkspace(IEnumerable<AssignmentDecryptionKeyModel> assignmentDecryptionKeys, IEnumerable<AssignmentFileMetadata> assignmentFileMetadatas, string assignmentFilesPath)
    {
      this._assignmentFileStorageService.CopyFilesToWorkspace(this._decryptionService.DecryptAssignmentFiles(assignmentDecryptionKeys, assignmentFileMetadatas), assignmentFilesPath);
    }

    public IEnumerable<AssignmentDecryptionKeyModel> GetDecryptionKeys(string pinCode)
    {
      AssignmentDecryptionResponse assigmentDecryptionKeys = this._flexClient.GetAssigmentDecryptionKeys(new AssignmentDecryptionRequest(pinCode));
      if (!assigmentDecryptionKeys.ValidPinCode)
        throw new InvalidPinCodeException();
      return assigmentDecryptionKeys.AssignmentDecryptionKeys.Select<AssignmentDecryptionKey, AssignmentDecryptionKeyModel>((Func<AssignmentDecryptionKey, AssignmentDecryptionKeyModel>) (adk => new AssignmentDecryptionKeyModel(adk.Filename, adk.CiphertextHash, adk.Key, adk.Iv, adk.CleartextHash)));
    }
  }
}
