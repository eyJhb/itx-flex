// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.FilesResult
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;
using System.Collections.Generic;

namespace Itx.Flex.Client.Service
{
  public class FilesResult
  {
    public static FilesResult CreateSelectedFilesResult(IEnumerable<HandInFileModel> handInFileModels)
    {
      return new FilesResult(true, handInFileModels, (IEnumerable<HandInFileModel>) null);
    }

    public static FilesResult CreateSelectedFilesFilteredResult(IEnumerable<HandInFileModel> handInFileModels, IEnumerable<HandInFileModel> invalidHandInFileModels)
    {
      return new FilesResult(true, handInFileModels, invalidHandInFileModels);
    }

    public static FilesResult CreateCancelledResult()
    {
      return new FilesResult(false, (IEnumerable<HandInFileModel>) null, (IEnumerable<HandInFileModel>) null);
    }

    private FilesResult(bool hasSelected, IEnumerable<HandInFileModel> handInFileModels = null, IEnumerable<HandInFileModel> invalidHandInFileModels = null)
    {
      this.HasSelected = hasSelected;
      this.HandInFileModels = handInFileModels;
      this.InvalidHandInFileModels = invalidHandInFileModels;
    }

    public bool HasSelected { get; }

    public IEnumerable<HandInFileModel> HandInFileModels { get; }

    public IEnumerable<HandInFileModel> InvalidHandInFileModels { get; }
  }
}
