// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.HandInFileService
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
  public class HandInFileService : IHandInFileService
  {
    private readonly IFlexClient _flexClient;
    private readonly IHashProvider _hashProvider;
    private readonly IFileService _fileService;
    private readonly IConfigurationService _configurationService;
    private readonly IPathService _pathService;
    private readonly IDirectoryService _directoryService;

    public HandInFileService(IFlexClient flexClient, IHashProvider hashProvider, IFileService fileService, IConfigurationService configurationService, IPathService pathService, IDirectoryService directoryService)
    {
      this._flexClient = flexClient;
      this._hashProvider = hashProvider;
      this._fileService = fileService;
      this._configurationService = configurationService;
      this._pathService = pathService;
      this._directoryService = directoryService;
    }

    public HandInResult TagHandIn(string id, HandInFileModel mainDocument, IEnumerable<HandInFileModel> attachments, HandInFileModel handInFieldsFile)
    {
      List<SubmitHandInFileModel> submitHandInFileModelList = new List<SubmitHandInFileModel>(attachments.Select<HandInFileModel, SubmitHandInFileModel>((Func<HandInFileModel, SubmitHandInFileModel>) (a => new SubmitHandInFileModel(a.Name, a.Path, SubmitHandInFileType.Attachment)))) { new SubmitHandInFileModel(mainDocument.Name, mainDocument.Path, SubmitHandInFileType.MainDocument) };
      List<SubmitHandInFileModel> storage = this.CopyToStorage(id, (IEnumerable<SubmitHandInFileModel>) submitHandInFileModelList);
      storage.Add(new SubmitHandInFileModel(handInFieldsFile.Name, handInFieldsFile.Path, SubmitHandInFileType.HandInFields));
      IEnumerable<PendingFileUpload> source = storage.Where<SubmitHandInFileModel>((Func<SubmitHandInFileModel, bool>) (h => h != null)).Select<SubmitHandInFileModel, PendingFileUpload>((Func<SubmitHandInFileModel, PendingFileUpload>) (h => new PendingFileUpload() { Filename = h.Name, Hash = this._hashProvider.ComputeHashAsBase64(this._fileService.ReadAllBytesFromFile(h.Path)) }));
      return new HandInResult(this._flexClient.TagHandin(new TagPendingHandinRequest() { PendingFileUploads = source.ToList<PendingFileUpload>() }).HandinStatus.ToHandInStatus(), (IEnumerable<SubmitHandInFileModel>) storage);
    }

    public void ClearStorage(string id)
    {
      this._directoryService.Delete(this._pathService.Combine(this._configurationService.TemporaryHandInPath, id));
    }

    private List<SubmitHandInFileModel> CopyToStorage(string id, IEnumerable<SubmitHandInFileModel> handInFiles)
    {
      string directoryPath = this._pathService.Combine(this._configurationService.TemporaryHandInPath, id);
      if (!this._directoryService.Exists(directoryPath))
        this._directoryService.CreateDirectory(directoryPath);
      List<SubmitHandInFileModel> submitHandInFileModelList = new List<SubmitHandInFileModel>();
      foreach (SubmitHandInFileModel handInFile in handInFiles)
      {
        string str = directoryPath + "\\" + handInFile.Name;
        this._fileService.CopyFile(handInFile.Path, str, true);
        submitHandInFileModelList.Add(new SubmitHandInFileModel(handInFile.Name, str, handInFile.SubmitHandInFileType));
      }
      return submitHandInFileModelList;
    }
  }
}
