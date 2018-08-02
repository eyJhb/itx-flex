// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.SelectFilesService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Itx.Flex.Client.Service
{
  public class SelectFilesService : ISelectFilesService
  {
    private readonly IPathService _pathService;

    public SelectFilesService(IPathService pathService)
    {
      this._pathService = pathService;
    }

    public FilesResult GetSelectedFile(string initialDirectory, string fileExtensionValidText, IEnumerable<string> fileExtensions)
    {
      return this.GetSelectedFiles(initialDirectory, fileExtensionValidText, fileExtensions, false);
    }

    private FilesResult GetSelectedFiles(string initialDirectory, string fileExtensionValidText, IEnumerable<string> fileExtensions, bool allowMultiselect)
    {
      List<string> fileExtensionsList = (fileExtensions != null ? fileExtensions.ToList<string>() : (List<string>) null) ?? new List<string>();
      CommonOpenFileDialog commonOpenFileDialog1;
      if (!fileExtensionsList.Any<string>())
      {
        CommonOpenFileDialog commonOpenFileDialog2 = new CommonOpenFileDialog();
        commonOpenFileDialog2.IsFolderPicker = false;
        commonOpenFileDialog2.AddToMostRecentlyUsedList = false;
        commonOpenFileDialog2.AllowNonFileSystemItems = false;
        commonOpenFileDialog2.EnsureFileExists = true;
        commonOpenFileDialog2.EnsurePathExists = true;
        commonOpenFileDialog2.EnsureReadOnly = false;
        commonOpenFileDialog2.EnsureValidNames = true;
        commonOpenFileDialog2.Multiselect = allowMultiselect;
        commonOpenFileDialog2.ShowPlacesList = true;
        commonOpenFileDialog2.InitialDirectory = initialDirectory;
        commonOpenFileDialog1 = commonOpenFileDialog2;
      }
      else
      {
        commonOpenFileDialog1 = new CommonOpenFileDialog();
        commonOpenFileDialog1.IsFolderPicker = false;
        commonOpenFileDialog1.AddToMostRecentlyUsedList = false;
        commonOpenFileDialog1.AllowNonFileSystemItems = false;
        commonOpenFileDialog1.EnsureFileExists = true;
        commonOpenFileDialog1.EnsurePathExists = true;
        commonOpenFileDialog1.EnsureReadOnly = false;
        commonOpenFileDialog1.EnsureValidNames = true;
        commonOpenFileDialog1.Multiselect = allowMultiselect;
        commonOpenFileDialog1.ShowPlacesList = true;
        commonOpenFileDialog1.InitialDirectory = initialDirectory;
        commonOpenFileDialog1.Filters.Add(new CommonFileDialogFilter(fileExtensionValidText, string.Join(",", fileExtensionsList.ToArray())));
      }
      CommonOpenFileDialog commonOpenFileDialog3 = commonOpenFileDialog1;
      if (commonOpenFileDialog3.ShowDialog() != CommonFileDialogResult.Ok)
        return FilesResult.CreateCancelledResult();
      if (fileExtensionsList.Any<string>())
        return FilesResult.CreateSelectedFilesFilteredResult(commonOpenFileDialog3.FileNames.Where<string>((Func<string, bool>) (f => fileExtensionsList.Contains(this._pathService.GetExtension(f)))).Select<string, HandInFileModel>((Func<string, HandInFileModel>) (f => new HandInFileModel(this._pathService.GetFileName(f), f))), commonOpenFileDialog3.FileNames.Where<string>((Func<string, bool>) (f => !fileExtensionsList.Contains(this._pathService.GetExtension(f)))).Select<string, HandInFileModel>((Func<string, HandInFileModel>) (f => new HandInFileModel(this._pathService.GetFileName(f), f))));
      return FilesResult.CreateSelectedFilesResult(commonOpenFileDialog3.FileNames.Select<string, HandInFileModel>((Func<string, HandInFileModel>) (f => new HandInFileModel(this._pathService.GetFileName(f), f))));
    }

    public FilesResult GetSelectedFilesNoFilter(string initialDirectory)
    {
      return this.GetSelectedFiles(initialDirectory, (string) null, (IEnumerable<string>) null, true);
    }
  }
}
