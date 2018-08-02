// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Model.WorkspacePaths
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

namespace Itx.Flex.Client.Model
{
  public class WorkspacePaths
  {
    public string WorkspacePath { get; }

    public string AssignmentFilesPath { get; }

    public string HandInPath { get; }

    public WorkspacePaths(string workspacePath, string assignmentFilesPath, string handInPath)
    {
      this.WorkspacePath = workspacePath;
      this.AssignmentFilesPath = assignmentFilesPath;
      this.HandInPath = handInPath;
    }
  }
}
