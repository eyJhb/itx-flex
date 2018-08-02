// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.HandInFileMetadataStorageService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Itx.Flex.Client.Service
{
  public class HandInFileMetadataStorageService : IHandInFileMetadataStorageService
  {
    private const string MainDocumentFilePathKey = "MainDocumentFilePath";
    private const string AttachmentFilePathsKey = "AttachmentFilePaths";
    private const string FilePathSplitterToken = "|";
    private const string NameSplitterToken = "?";
    private readonly IRegistryService _registryService;

    public HandInFileMetadataStorageService(IRegistryService registryService)
    {
      this._registryService = registryService;
    }

    public void StoreMainDocumentFilePath(HandInFileModel handInFileModel)
    {
      this._registryService.SetValue("MainDocumentFilePath", handInFileModel.Name + "?" + handInFileModel.Path);
    }

    public void ClearMainDocument()
    {
      this._registryService.ClearValue("MainDocumentFilePath");
    }

    public void StoreAttachmentFilePaths(IEnumerable<HandInFileModel> filePaths)
    {
      this._registryService.SetValue("AttachmentFilePaths", string.Join("|", filePaths.Select<HandInFileModel, string>((Func<HandInFileModel, string>) (h => h.Name + "?" + h.Path)).ToArray<string>()));
    }

    public HandInFileModel GetMainDocumentFilePath()
    {
      string[] strArray = this._registryService.GetValue("MainDocumentFilePath")?.Split("?".ToCharArray());
      if (strArray == null || strArray.Length != 2)
        return (HandInFileModel) null;
      return new HandInFileModel(strArray[0], strArray[1]);
    }

    public IEnumerable<HandInFileModel> GetAttachmentFilePaths()
    {
      string str = this._registryService.GetValue("AttachmentFilePaths");
      return (IEnumerable<HandInFileModel>) ((str != null ? ((IEnumerable<string>) str.Split("|".ToCharArray())).ToList<string>() : (List<string>) null) ?? new List<string>()).Select<string, string[]>((Func<string, string[]>) (args => args?.Split("?".ToCharArray()))).Where<string[]>((Func<string[], bool>) (args =>
      {
        if (args != null)
          return args.Length == 2;
        return false;
      })).Select<string[], HandInFileModel>((Func<string[], HandInFileModel>) (splitArgs => new HandInFileModel(splitArgs[0], splitArgs[1]))).ToList<HandInFileModel>();
    }

    public void Clear()
    {
      this.ClearMainDocument();
      this._registryService.ClearValue("AttachmentFilePaths");
    }
  }
}
