// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.SelectDirectoryService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Microsoft.WindowsAPICodePack.Dialogs;

namespace Itx.Flex.Client.Service
{
  public class SelectDirectoryService : ISelectDirectoryService
  {
    public DirectoryResult GetDirectoryLocation(string defaultDirectory)
    {
      CommonOpenFileDialog commonOpenFileDialog1 = new CommonOpenFileDialog();
      commonOpenFileDialog1.IsFolderPicker = true;
      commonOpenFileDialog1.AddToMostRecentlyUsedList = false;
      commonOpenFileDialog1.AllowNonFileSystemItems = false;
      commonOpenFileDialog1.EnsureFileExists = true;
      commonOpenFileDialog1.EnsurePathExists = true;
      commonOpenFileDialog1.EnsureReadOnly = false;
      commonOpenFileDialog1.EnsureValidNames = true;
      commonOpenFileDialog1.Multiselect = false;
      commonOpenFileDialog1.ShowPlacesList = true;
      commonOpenFileDialog1.DefaultDirectory = defaultDirectory;
      CommonOpenFileDialog commonOpenFileDialog2 = commonOpenFileDialog1;
      if (commonOpenFileDialog2.ShowDialog() != CommonFileDialogResult.Ok)
        return DirectoryResult.CreateCancelledResult();
      return DirectoryResult.CreateSelectedDirectoryResult(commonOpenFileDialog2.FileName);
    }
  }
}
