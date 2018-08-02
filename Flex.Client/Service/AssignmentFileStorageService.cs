// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.AssignmentFileStorageService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace Itx.Flex.Client.Service
{
  public class AssignmentFileStorageService : IAssignmentFileStorageService
  {
    private readonly IDirectoryService _directoryService;
    private readonly IConfigurationService _configurationService;
    private readonly IWebClientService _webClientService;
    private readonly IFileService _fileService;
    private readonly IHashValidator _hashValidator;

    public AssignmentFileStorageService(IDirectoryService directoryService, IConfigurationService configurationService, IWebClientService webClientService, IFileService fileService, IHashValidator hashValidator)
    {
      this._directoryService = directoryService;
      this._configurationService = configurationService;
      this._webClientService = webClientService;
      this._fileService = fileService;
      this._hashValidator = hashValidator;
    }

    public FileMetadataStatus DownloadAssignmentFilesToTemporaryDirectory(IEnumerable<AssignmentFileMetadata> assignmentFileMetadatas)
    {
      this._directoryService.CreateDirectory(this._configurationService.TemporaryAssignmentFilesPath);
      List<AssignmentFileMetadata> assignmentFileMetadataList1 = new List<AssignmentFileMetadata>();
      List<AssignmentFileMetadata> assignmentFileMetadataList2 = new List<AssignmentFileMetadata>();
      foreach (AssignmentFileMetadata assignmentFileMetadata in assignmentFileMetadatas)
      {
        try
        {
          this._webClientService.DownloadFile(assignmentFileMetadata.Url, this._configurationService.GetEncryptedPath(assignmentFileMetadata.Filename));
          assignmentFileMetadataList1.Add(assignmentFileMetadata);
        }
        catch (Exception ex)
        {
          assignmentFileMetadataList2.Add(assignmentFileMetadata);
        }
      }
      return new FileMetadataStatus((IEnumerable<AssignmentFileMetadata>) assignmentFileMetadataList1, (IEnumerable<AssignmentFileMetadata>) assignmentFileMetadataList2);
    }

    public void CopyFilesToWorkspace(IEnumerable<DecryptedAssignmentFileMetadata> storedFiles, string assignmentDirectoryPath)
    {
      foreach (DecryptedAssignmentFileMetadata storedFile in storedFiles)
      {
        string str = assignmentDirectoryPath + "\\" + storedFile.Filename;
        if (!this._fileService.Exists(str))
        {
          this._fileService.CopyFile(this._configurationService.TemporaryAssignmentFilesPath + storedFile.Filename, str, true);
        }
        else
        {
          bool flag;
          using (FileStream fileData = this._fileService.OpenStream(str))
            flag = this._hashValidator.IsValidHash(fileData, storedFile.Hash);
          if (!flag)
          {
            this._fileService.Delete(str);
            this._fileService.CopyFile(this._configurationService.TemporaryAssignmentFilesPath + storedFile.Filename, str, true);
          }
        }
      }
    }
  }
}
