// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.DecryptionService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Flex.WebserviceClient;
using Itx.Flex.Client.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Itx.Flex.Client.Service
{
  public class DecryptionService : IDecryptionService
  {
    private readonly IFlexClient _flexClient;
    private readonly IFileService _fileService;
    private readonly IConfigurationService _configurationService;
    private readonly IHashValidator _hashValidator;
    private readonly ILoggerService _loggerService;
    private readonly IFileDecrypter _fileDecrypter;

    public DecryptionService(IFlexClient flexClient, IFileService fileService, IConfigurationService configurationService, IHashValidator hashValidator, ILoggerService loggerService, IFileDecrypter fileDecrypter)
    {
      this._flexClient = flexClient;
      this._fileService = fileService;
      this._configurationService = configurationService;
      this._hashValidator = hashValidator;
      this._loggerService = loggerService;
      this._fileDecrypter = fileDecrypter;
    }

    public FileMetadataStatus ValidateEncryptedAssignmentFiles(IEnumerable<AssignmentFileMetadata> assignmentFileMetadatas)
    {
      List<AssignmentFileMetadata> assignmentFileMetadataList1 = new List<AssignmentFileMetadata>();
      List<AssignmentFileMetadata> assignmentFileMetadataList2 = new List<AssignmentFileMetadata>();
      foreach (AssignmentFileMetadata assignmentFileMetadata in assignmentFileMetadatas)
      {
        using (FileStream fileData = this._fileService.OpenStream(this._configurationService.GetEncryptedPath(assignmentFileMetadata.Filename)))
        {
          if (this._hashValidator.IsValidHash(fileData, assignmentFileMetadata.Hash))
            assignmentFileMetadataList1.Add(assignmentFileMetadata);
          else
            assignmentFileMetadataList2.Add(assignmentFileMetadata);
        }
      }
      return new FileMetadataStatus((IEnumerable<AssignmentFileMetadata>) assignmentFileMetadataList1, (IEnumerable<AssignmentFileMetadata>) assignmentFileMetadataList2);
    }

    public IEnumerable<DecryptedAssignmentFileMetadata> DecryptAssignmentFiles(IEnumerable<AssignmentDecryptionKeyModel> assignmentDecryptionKeys, IEnumerable<AssignmentFileMetadata> storedFiles)
    {
      List<AssignmentFileMetadata> list1 = storedFiles.ToList<AssignmentFileMetadata>();
      IEnumerable<string> first1 = list1.Select<AssignmentFileMetadata, string>((Func<AssignmentFileMetadata, string>) (sf => sf.Filename));
      IEnumerable<string> first2 = list1.Select<AssignmentFileMetadata, string>((Func<AssignmentFileMetadata, string>) (sf => sf.Hash));
      List<AssignmentDecryptionKeyModel> list2 = assignmentDecryptionKeys.ToList<AssignmentDecryptionKeyModel>();
      IEnumerable<string> second1 = list2.Select<AssignmentDecryptionKeyModel, string>((Func<AssignmentDecryptionKeyModel, string>) (adk => adk.Filename));
      IEnumerable<string> second2 = list2.Select<AssignmentDecryptionKeyModel, string>((Func<AssignmentDecryptionKeyModel, string>) (adk => adk.CiphertextHash));
      if (first1.Except<string>(second1).Any<string>() && first2.Except<string>(second2).Any<string>())
        this._loggerService.Log(LogType.Warning, "Missing decryption keys for stored files", (string) null);
      List<DecryptedAssignmentFileMetadata> assignmentFileMetadataList = new List<DecryptedAssignmentFileMetadata>();
      foreach (AssignmentDecryptionKeyModel decryptionKeyModel in list2)
      {
        using (FileStream fileData = this._fileService.OpenStream(this._configurationService.GetEncryptedPath(decryptionKeyModel.Filename)))
        {
          if (this._hashValidator.IsValidHash(fileData, decryptionKeyModel.CiphertextHash))
          {
            using (MemoryStream memoryStream = new MemoryStream())
            {
              fileData.Seek(0L, SeekOrigin.Begin);
              byte[] key = Convert.FromBase64String(decryptionKeyModel.Key);
              byte[] initializationVector = Convert.FromBase64String(decryptionKeyModel.InitializationVector);
              this._fileDecrypter.Process((Stream) fileData, (Stream) memoryStream, key, initializationVector);
              this._fileService.WriteToFile(this._configurationService.TemporaryAssignmentFilesPath + decryptionKeyModel.Filename, memoryStream.ToArray());
              assignmentFileMetadataList.Add(new DecryptedAssignmentFileMetadata()
              {
                Filename = decryptionKeyModel.Filename,
                Hash = decryptionKeyModel.CleartextHash
              });
            }
          }
        }
      }
      return (IEnumerable<DecryptedAssignmentFileMetadata>) assignmentFileMetadataList;
    }
  }
}
