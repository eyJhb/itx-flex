// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.HandInFieldValueStorageService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Itx.Flex.Client.Service
{
  public class HandInFieldValueStorageService : IHandInFieldValueStorageService
  {
    private readonly IConfigurationService _configurationService;
    private readonly IFileService _fileService;
    private readonly IPathService _pathService;
    private readonly IDirectoryService _directoryService;
    private const string HandInFieldFilename = "HandInField.json";

    public HandInFieldValueStorageService(IConfigurationService configurationService, IFileService fileService, IPathService pathService, IDirectoryService directoryService)
    {
      this._configurationService = configurationService;
      this._fileService = fileService;
      this._pathService = pathService;
      this._directoryService = directoryService;
    }

    public void Store(string boardingPass, Guid handInFieldId, IEnumerable<HandInFieldValueModel> handInFields)
    {
      try
      {
        byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject((object) handInFields));
        if (!this._directoryService.Exists(this.GetDirectoryPath(boardingPass)))
          this._directoryService.CreateDirectory(this.GetDirectoryPath(boardingPass));
        this._fileService.WriteToFile(this.GetFilePath(boardingPass, handInFieldId), bytes);
      }
      catch (Exception ex)
      {
      }
    }

    public IEnumerable<HandInFieldValueModel> Get(string boardingPass, Guid handInFieldId)
    {
      try
      {
        if (!this._fileService.Exists(this.GetFilePath(boardingPass, handInFieldId)))
          return (IEnumerable<HandInFieldValueModel>) new List<HandInFieldValueModel>();
        return JsonConvert.DeserializeObject<IEnumerable<HandInFieldValueModel>>(Encoding.UTF8.GetString(this._fileService.ReadAllBytesFromFile(this.GetFilePath(boardingPass, handInFieldId))));
      }
      catch (Exception ex)
      {
      }
      return (IEnumerable<HandInFieldValueModel>) new List<HandInFieldValueModel>();
    }

    public HandInFileModel GetFileMetadata(string boardingPass, Guid handInFieldId)
    {
      return new HandInFileModel(this.GetFilename(handInFieldId), this.GetFilePath(boardingPass, handInFieldId));
    }

    private string GetFilename(Guid id)
    {
      return id.ToString() + "HandInField.json";
    }

    private string GetFilePath(string id, Guid handInFieldId)
    {
      return this._pathService.Combine(this.GetDirectoryPath(id), this.GetFilename(handInFieldId));
    }

    private string GetDirectoryPath(string id)
    {
      return this._pathService.Combine(this._configurationService.TemporaryHandInPath, id);
    }
  }
}
