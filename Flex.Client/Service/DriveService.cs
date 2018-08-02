// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.DriveService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Itx.Flex.Client.Service
{
  public class DriveService : IDriveService
  {
    public bool HasRequiredAvailableDiskSpace(string driveLetter, int requiredSizeInMegabytes)
    {
      DriveInfo[] drives = DriveInfo.GetDrives();
      if (driveLetter != null)
        return ((IEnumerable<DriveInfo>) drives).First<DriveInfo>((Func<DriveInfo, bool>) (d => d.Name.Contains(driveLetter))).AvailableFreeSpace / 1048576L >= (long) requiredSizeInMegabytes;
      return false;
    }
  }
}
